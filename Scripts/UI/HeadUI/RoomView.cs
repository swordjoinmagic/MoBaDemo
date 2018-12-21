using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoomView : MonoBehaviour{

    private string roomName;
    public List<RoomViewOnePlayer> roomPlayers;

    private void Start() {
        InitListener();
    }

    // 找到一个未被赋值的RoomOnePlayer
    RoomViewOnePlayer FindOnePlayerWithNoValue() {
        foreach (var onePlayer in roomPlayers) {
            if (onePlayer.userStatusText.text == "无玩家")
                return onePlayer;
        }
        return null;
    }

    public void Show(string roomName) {
        this.roomName = roomName;        
        gameObject.SetActive(true);

        SendClientGetRoom(this.roomName);
    }

    public void Hide() {

    }

    public void InitListener() {
        NetWorkManager.Instance.AddListener("GetRoom", TreateGetRoomResult);
        NetWorkManager.Instance.AddListener("AttendRoomResult", TreateAttendRoomResult);
    }

    /// <summary>
    /// 发送客户端的GetRoom消息,已获得当前房间的房间信息
    /// </summary>
    public void SendClientGetRoom(string roomName) {
        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("GetRoom");

        // 协议参数
        protocol.AddString(roomName);

        // 发送协议
        NetWorkManager.Instance.Send(protocol);
    }

    /// <summary>
    /// 处理GetRoom协议
    /// </summary>
    public void TreateGetRoomResult(ProtocolBytes protocolBytes) {

        NetWorkManager.Instance.roomPlayers.Clear();

        //===============================
        // 房间名
        string roomName = protocolBytes.GetString();

        // 房间人数
        int roomPerson = protocolBytes.GetInt();

        for (int i=0;i<roomPerson;i++) {
            // 用户名
            string userName = protocolBytes.GetString();
            // 用户阵营
            string userFaction = protocolBytes.GetString();
            // 用户状态
            string userStatus = protocolBytes.GetString();

            Debug.Log("USerFaction:"+userFaction+" userStatus:"+userStatus);

            // 设置UI
            roomPlayers[i].UserName = userName;
            roomPlayers[i].UserFaction = userFaction;
            roomPlayers[i].RoomName = roomName;
            switch (userStatus) {
                case "HomeOwner":
                    userStatus = "房主";
                    break;
                case "Prepare":
                    userStatus = "未准备~";
                    break;
                case "Ready":
                    userStatus = "已准备~";
                    break;
            }
            roomPlayers[i].UserStatus = userStatus;

            NetWorkManager.Instance.roomPlayers.Add(new RoomPlayer { UserName=userName,UserFaction=userFaction });

            if (userName == NetWorkManager.Instance.NowPlayerID)
                NetWorkManager.Instance.NowPlayerFaction = userFaction;
        }

    }

    /// <summary>
    /// 处理加入房间的协议
    /// </summary>
    /// <param name="protocolBytes"></param>
    public void TreateAttendRoomResult(ProtocolBytes protocolBytes) {

        protocolBytes.ResetIndex();

        // 获得协议名
        string protocolName =  protocolBytes.GetString();
        Debug.Log("协议名是:"+protocolName);

        // 获得房间名
        string roomName = protocolBytes.GetString();

        Debug.Log("RoomName是:"+roomName);

        // 获得用户名
        string userName = protocolBytes.GetString();

        // 当用户名和本地用户不一样时,才进行触发
        if (userName == NetWorkManager.Instance.NowPlayerID) return;

        // 发送更新房间信息的消息
        SendClientGetRoom(roomName);
    }


}

