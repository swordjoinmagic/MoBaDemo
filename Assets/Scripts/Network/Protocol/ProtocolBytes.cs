using Assets.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProtocolBytes : IProtocolBase {

    private byte[] bytes;
    private int start = 0;

    public string Name {
        get {
            return GetString();
        }
    }

    public string Desc {
        get {
            return "";
        }
    }

    public void ResetIndex() {
        start = 0;
    }

    public static ProtocolBytes Decode(byte[] readBuff, int start, int length) {
        ProtocolBytes protocolBytes = new ProtocolBytes();
        protocolBytes.bytes = new byte[length];
        Array.Copy(readBuff, start, protocolBytes.bytes, 0, length);
        //for (int i=0;i<protocolBytes.bytes.Length;i++) {
        //    Debug.Log("byts["+i+"]:"+protocolBytes.bytes[i]);
        //}
        return protocolBytes;
    }

    public byte[] Encode() {
        return bytes;
    }

    // 为字节流协议内容添加字符串的方法
    public void AddString(String str) {
        int length = str.Length;
        byte[] lenBytes = BitConverter.GetBytes(length);
        byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str);
        byte[] combineBytes = lenBytes.Reverse().Concat(strBytes).ToArray();
        if (bytes == null) {
            bytes = combineBytes;
        } else {
            bytes = bytes.Concat(combineBytes).ToArray();
        }
    }

    // 从协议中获取字符串内容
    public String GetString() {
        if (bytes == null)
            return "";
        if (bytes.Length < start + 4)
            return "";

        // 获得协议中字符串的长度
        int strLen = MyBitConverter.ToInt32(bytes, start);
        if (bytes.Length < start + 4 + strLen)
            return "";

        // 获得协议中具体的字符串的内容
        String str = System.Text.Encoding.UTF8.GetString(bytes, start+sizeof(Int32), strLen);

        // start指针前移
        this.start = start + 4 + strLen;

        return str;
    }

    public void AddInt(int num) {
        byte[] numBytes = BitConverter.GetBytes(num).Reverse().ToArray();
        if (bytes == null) {
            bytes = numBytes;
        } else {
            bytes = bytes.Concat(numBytes).ToArray();
        }
    }

    public int GetInt() {
        if (bytes == null)
            return 0;
        if (bytes.Length < start + 4)
            return 0;

        int result = MyBitConverter.ToInt32(bytes, start);
        start = start + 4;
        return result;
    }

    public void AddFloat(float num) {
        byte[] numBytes = BitConverter.GetBytes(num).Reverse().ToArray();
        if (bytes == null)
            bytes = numBytes;
        else
            bytes = bytes.Concat(numBytes).ToArray();
    }
    public float GetFloat() {
        if (bytes == null)
            return 0;
        if (bytes.Length < start + 4)
            return 0;
        float result = MyBitConverter.ToFloat(bytes, start);

        start = start + 4;

        return result;
    }
}

