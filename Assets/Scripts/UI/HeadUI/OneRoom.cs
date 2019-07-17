using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OneRoom :MonoBehaviour{
    private string roomName;
    private int roomPerson;
    private string roomStatus;

    //====================================
    // 管理的UI控件
    public Text roomNameText;
    public Text roomPersonText;
    public Text roomStatusText;

    public string RoomName {
        get {
            return roomName;
        }

        set {
            roomName = value;
            roomNameText.text = value;
        }
    }

    public int RoomPerson {
        get {
            return roomPerson;
        }

        set {
            roomPerson = value;
            roomPersonText.text = value.ToString()+"/10";

        }
    }

    public string RoomStatus {
        get {
            return roomStatus;
        }

        set {
            roomStatus = value;
            roomStatusText.text = value;
        }
    }
}

