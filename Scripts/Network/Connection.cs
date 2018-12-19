
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 用于管理一个Socket连接，包含了TCP粘包分包的解决方案
/// 同时，将所有收到的信息包解析为ProtocolBytes类，并将
/// 此消息Push到消息队列
/// </summary>
public class Connection {

    // Socket单次读取数据最多可以读取多少字节
    // 用于粘包分包
    private const int BUFFER_SIZE = 2048;

    // 通信Socket,用于连接服务器
    private Socket socket;

    // 数据暂存区,用于暂时缓存从Socket中读取到的数据包
    // 用于TCP协议的粘包分包
    private byte[] readBuff = new byte[BUFFER_SIZE];

    // 数据包的第一个数据,表示这个数据包的长度
    private int length = 0;

    // bufferCount表示在readBuff中的指针，
    // 可以认为bufferCount表示当前socket一共读取了多少字节
    // 对bufferCount的判断可以过滤 小于整形字节的数据包
    private int bufferCount = 0;

    // 消息队列
    private Queue<ProtocolBytes> msgList = new Queue<ProtocolBytes>();

    public Queue<ProtocolBytes> MsgList {
        get {
            return msgList;
        }
    }

    public Socket Socket {
        get {
            return socket;
        }
    }

    /// <summary>
    /// 处理某一个协议的委托
    /// </summary>
    /// <param name="protocolBytes"></param>
    public delegate void ProtocolHandler(ProtocolBytes protocolBytes);

    /// <summary>
    /// 用于处理协议信息的字典,字典的键是协议名,字典的Value是协议的处理方法
    /// </summary>
    private Dictionary<string, ProtocolHandler> eventDict = new Dictionary<string, ProtocolHandler>();

    // 要连接的IP地址和端口
    public Connection(string address, int port) {
        Connect(address, port);
    }

    /// <summary>
    /// 向服务器端发送一个消息
    /// </summary>
    /// <param name="protocolBytes"></param>
    public void Send(ProtocolBytes protocolBytes) {
        byte[] bytes = protocolBytes.Encode();
        byte[] lenBytes = BitConverter.GetBytes(bytes.Length).Reverse().ToArray();
        byte[] combineBytes = lenBytes.Concat(bytes).ToArray();
        socket.Send(combineBytes);
    }

    /// <summary>
    /// Socket连接,同时开启Socket异步接受消息
    /// </summary>
    /// <param name="address"></param>
    /// <param name="port"></param>
    public void Connect(string address, int port) {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 连接目标地址
        Socket.Connect(address, port);

        // 异步接受信息
        Socket.BeginReceive(readBuff, bufferCount, BUFFER_SIZE - bufferCount, SocketFlags.None, ReceiveCB, readBuff);
    }

    /// <summary>
    /// 异步接受数据包的方法,当接受到一个数据包后,
    /// 将此数据交由ProcessData方法处理
    /// </summary>
    /// <param name="asyncResult"></param>
    public void ReceiveCB(IAsyncResult asyncResult) {
        try {
            int count = Socket.EndReceive(asyncResult);

            //Debug.Log("受到字节：" + count);

            // bufferCount指针前移
            bufferCount += count;

            // 处理此次得到的数据包
            ProcessData();

            // 继续对数据进行异步接受
            Socket.BeginReceive(readBuff, bufferCount, BUFFER_SIZE - bufferCount, SocketFlags.None, ReceiveCB, readBuff);
        } catch (Exception e) { }
    }

    /// <summary>
    /// 处理一次接受的数据,期间要对数据的完整性进行判断
    /// </summary>
    public void ProcessData() {
        // 如果输入的数据的长度小于4，
        // 那么说明此消息还不足以构成一个数据包
        if (bufferCount < 4) {
            return;
        }

        // 获得此消息前4个字节，以此获得数据包长度
        byte[] lenBytes = new byte[4];
        for (int i = 0; i < 4; i++) {
            lenBytes[i] = readBuff[i];
        }
        length = ByteArrayToInt(lenBytes);

        // 还未读到完整数据包
        if (bufferCount < length + 4) {
            return;
        }

        // 处理消息
        ProtocolBytes protocolBytes = ProtocolBytes.Decode(readBuff, sizeof(Int32), length);
        MsgList.Enqueue(protocolBytes);


        // 清除已处理的消息
        int count = bufferCount - length - 4;
        Array.Copy(readBuff, 4 + length, readBuff, 0, count);
        bufferCount = count;

        if (bufferCount > 0)
            ProcessData();
    }

    public static int ByteArrayToInt(byte[] b) {
        return b[3] & 0xFF |
                (b[2] & 0xFF) << 8 |
                (b[1] & 0xFF) << 16 |
                (b[0] & 0xFF) << 24;
    }

    /// <summary>
    /// 用于为某一个具体的协议增加监听(回调)方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="protocolHandler"></param>
    public void AddListener(string name, ProtocolHandler protocolHandler) {
        if (eventDict.ContainsKey(name)) {
            eventDict[name] += protocolHandler;
        } else {
            eventDict[name] = protocolHandler;
        }
    }

    /// <summary>
    /// 根据协议名,处理一个具体的协议信息,
    /// 返回布尔型,True表示处理完成,False表示未对此协议添加监听方法
    /// </summary>
    /// <param name="name"></param>
    public bool TreateProtocol(string name, ProtocolBytes protocolBytes) {
        if (eventDict.ContainsKey(name)) {
            eventDict[name](protocolBytes);
            return true;
        }
        return false;
    }
}

