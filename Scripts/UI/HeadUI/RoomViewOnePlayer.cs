using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class RoomViewOnePlayer : MonoBehaviour{

    //=======================================
    // 属性
    private string userName;
    private string userFaction;
    private string userStatus;
    private string roomName;    // 所属房间名


    //=========================================
    // UI控件
    public Text userNameText;
    public Text userFactionText;
    public Text userStatusText;
    public EventTrigger userFactionEventTrigger;
    public EventTrigger userStatusEventTrigger;
    public RectTransform userFactionBackground;
    public RectTransform userStatusBackground;

    public string UserName {
        get {
            return userName;
        }

        set {
            userName = value;
            userNameText.text = value;
            if (userName == NetWorkManager.Instance.NowPlayerID) BindButtonEvent();
        }
    }

    public string UserFaction {
        get {
            return userFaction;
        }

        set {
            userFaction = value;

            userFactionText.text = value;
        }
    }

    public string UserStatus {
        get {
            return userStatus;
        }

        set {
            userStatus = value;

            userStatusText.text = value;
        }
    }

    public string RoomName {
        get {
            return roomName;
        }

        set {
            roomName = value;
        }
    }

    private void Start() {

    }

    /// <summary>
    /// 发送玩家阵营改变的协议
    /// </summary>
    public void SendChangeUserFaction(string userFaction) {
        string userId = NetWorkManager.Instance.NowPlayerID;

        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();
        // 协议名
        protocol.AddString("ChangeUserFaction");

        // 参数
        protocol.AddString(roomName);
        protocol.AddString(userId);
        protocol.AddString(userFaction);

        NetWorkManager.Instance.Send(protocol);
    }

    /// <summary>
    /// 发送玩家准备状态改变的协议
    /// </summary>
    public void SendChangeUserStatus(string userStatus) {
        string userId = NetWorkManager.Instance.NowPlayerID;

        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();
        // 协议名
        protocol.AddString("ChangeUserStatus");

        // 参数
        protocol.AddString(roomName);
        protocol.AddString(userId);
        protocol.AddString(userStatus);

        NetWorkManager.Instance.Send(protocol);
    }

    /// <summary>
    /// 发送开始游戏协议
    /// </summary>
    public void SendStartGameProtocol() {

        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("StartGame");

        // 协议参数
        protocol.AddString(roomName);

        NetWorkManager.Instance.Send(protocol);
    }

    // 绑定按钮
    void BindButtonEvent() {

        userFactionEventTrigger.triggers.Clear();
        userStatusEventTrigger.triggers.Clear();


        // 鼠标进入事件
        EventTrigger.Entry onMouseEnter = new EventTrigger.Entry();
        onMouseEnter.eventID = EventTriggerType.PointerEnter;
        onMouseEnter.callback.AddListener((eventdata) => {
            Debug.Log("鼠标进入");
            userFactionBackground.DOSizeDelta(new Vector2(300f, userFactionBackground.sizeDelta.y), 0.5f);
        });

        // 鼠标离开事件
        EventTrigger.Entry onMouseLeave = new EventTrigger.Entry();
        onMouseLeave.eventID = EventTriggerType.PointerExit;
        onMouseLeave.callback.AddListener((eventdata) => {
            userFactionBackground.DOSizeDelta(new Vector2(0f, userFactionBackground.sizeDelta.y), 0.5f);
        });

        // 鼠标点击事件
        EventTrigger.Entry OnMouseClick = new EventTrigger.Entry();
        OnMouseClick.eventID = EventTriggerType.PointerClick;
        OnMouseClick.callback.AddListener((eventData) => {
            if (userFaction == "Red")
                SendChangeUserFaction("Blue");
            else
                SendChangeUserFaction("Red");
        });

        userFactionEventTrigger.triggers.Add(onMouseEnter);
        userFactionEventTrigger.triggers.Add(onMouseLeave);
        userFactionEventTrigger.triggers.Add(OnMouseClick);


        // 鼠标进入事件
        EventTrigger.Entry onMouseEnter1 = new EventTrigger.Entry();
        onMouseEnter1.eventID = EventTriggerType.PointerEnter;
        onMouseEnter1.callback.AddListener((eventdata) => {
            Debug.Log("鼠标进入");
            userStatusBackground.DOSizeDelta(new Vector2(300f, userFactionBackground.sizeDelta.y), 0.5f);
        });

        // 鼠标离开事件
        EventTrigger.Entry onMouseLeave1 = new EventTrigger.Entry();
        onMouseLeave1.eventID = EventTriggerType.PointerExit;
        onMouseLeave1.callback.AddListener((eventdata) => {
            userStatusBackground.DOSizeDelta(new Vector2(0f, userFactionBackground.sizeDelta.y), 0.5f);
        });

        // 鼠标点击事件
        EventTrigger.Entry OnMouseClick1 = new EventTrigger.Entry();
        OnMouseClick1.eventID = EventTriggerType.PointerClick;
        OnMouseClick1.callback.AddListener((eventData) => {
            Debug.Log("userStatus:" + userStatus);
            if (userStatus == "未准备~")
                SendChangeUserStatus("Ready");
            else if (userStatus == "已准备~")
                SendChangeUserStatus("Prepare");
            else if (userStatus == "房主") {
                SendStartGameProtocol();
            }
        });

        userStatusEventTrigger.triggers.Add(onMouseEnter1);
        userStatusEventTrigger.triggers.Add(onMouseLeave1);
        userStatusEventTrigger.triggers.Add(OnMouseClick1);
    }

}

