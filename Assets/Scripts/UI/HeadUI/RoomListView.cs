using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class RoomListView : MonoBehaviour {
    public Button createRoomButton;
    public Button attendRoomButton;
    public InputField roomNameInputField;
    public RoomView roomView;

    public InputField inputField;

    private System.Action OnHideCompelteAction;
    public OneRoom oneRoomPrefab;
    public RectTransform roomParentPanel;
    private bool isStopSendGetRoomList = false;

    public List<OneRoom> roomList;


    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        isStopSendGetRoomList = true;
        transform.DOScale(new Vector3(0, 0, 0), 1f).onComplete += () => {
            if (OnHideCompelteAction != null) OnHideCompelteAction();
        };
    }

    private void Start() {

        isStopSendGetRoomList = false;

        InitListener();

        roomList = new List<OneRoom>();

        createRoomButton.onClick.AddListener(SendCreateRoomProtocol);
        attendRoomButton.onClick.AddListener(SendAttendRoomProtocol);

        StartCoroutine(PeriodicallySendGetRoomList());
    }

    public void SendAttendRoomProtocol() {
        ProtocolBytes protocolBytes = new ProtocolBytes();
        // 协议名
        protocolBytes.AddString("AttendRoom");

        // 参数
        protocolBytes.AddString(inputField.text);
        protocolBytes.AddString(NetWorkManager.Instance.NowPlayerID);

        NetWorkManager.Instance.Send(protocolBytes);
    }

    public void SendCreateRoomProtocol() {
        ProtocolBytes protocolBytes = new ProtocolBytes();

        // 协议名
        protocolBytes.AddString("CreateRoom");

        // 协议参数
        protocolBytes.AddString(NetWorkManager.Instance.NowPlayerID);
        protocolBytes.AddString(inputField.text);

        Debug.Log("当前用户创建房间:"+ inputField.text);

        // 发送协议
        NetWorkManager.Instance.Send(protocolBytes);
    }

    public void SendGetRoomList() {
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("GetRoomList");

        NetWorkManager.Instance.Send(protocol);
    }

    /// <summary>
    /// 周期发送GetRoomList协议
    /// </summary>
    /// <returns></returns>
    public IEnumerator PeriodicallySendGetRoomList() {
        while (!isStopSendGetRoomList) {

            SendGetRoomList();

            yield return new WaitForSeconds(5f);
        }
        
    }

    public void InitListener() {
        NetWorkManager.Instance.AddListener("CreateRoomResult", TreateCreateRoomResult);
        NetWorkManager.Instance.AddListener("AttendRoomResult", TreateAttendRoomResult);
        NetWorkManager.Instance.AddListener("GetRoomList", TreateGetRoomList);

    }


    public void TreateCreateRoomResult(ProtocolBytes protocolBytes) {
        // 获得用户名
        string userName = protocolBytes.GetString();
        // 获得创建的房间的名字
        string roomName = protocolBytes.GetString();
        // 获得创建房间的结果
        string result = protocolBytes.GetString();
        // 失败原因,默认为空
        string failReason = null;
        if (result == "Fail") {
            failReason = protocolBytes.GetString();
        }

        Debug.Log("创建房间的结果是:"+result+" 失败原因:"+failReason);

        if (result == "Success") {

            // 设置当前玩家为房主
            NetWorkManager.Instance.IsHomeowner = true;

            // 创建成功,发送加入此房间的信息

            ProtocolBytes protocol = new ProtocolBytes();

            // 协议名
            protocol.AddString("AttendRoom");

            // 参数
            protocol.AddString(roomName);
            protocol.AddString(NetWorkManager.Instance.NowPlayerID);

            NetWorkManager.Instance.Send(protocol);
        }
    }

    public void TreateAttendRoomResult(ProtocolBytes protocolBytes) {

        // 获得房间名
        string roomName = protocolBytes.GetString();

        // 获得用户名
        string userName = protocolBytes.GetString();

        // 只处理当前玩家的登录事件
        if (userName != NetWorkManager.Instance.NowPlayerID) return;

        // 获得结果
        string result = protocolBytes.GetString();
        // 失败原因,默认为空
        string failReason = null;
        if (result == "Fail") {
            failReason = protocolBytes.GetString();
        }
        if (result == "Success") {

            // 更新NetworkManager中的房间名
            NetWorkManager.Instance.RoomName = roomName;

            Hide();
            OnHideCompelteAction += () => { roomView.Show(roomName); };
        }

    }

    /// <summary>
    /// 处理GetRoomList协议
    /// </summary>
    public void TreateGetRoomList(ProtocolBytes protocolBytes) {
        
        // 获得房间数量
        int roomCount = protocolBytes.GetInt();

        // 下面N行获得每个房间的状态
        for (int i=0;i<roomCount;i++) {
            // 房间名
            string roomName = protocolBytes.GetString();
            // 房间人数
            int roomPerson = protocolBytes.GetInt();
            // 房间状态
            string roomStatus  = protocolBytes.GetString();

            if (roomList.Count <= i) {
                OneRoom oneRoom = GameObject.Instantiate<OneRoom>(oneRoomPrefab, roomParentPanel);
                roomList.Add(oneRoom);
                Debug.Log("添加OneRoome,目前roomList的Count为:"+roomList.Count);
            }
            roomList[i].RoomName = roomName;
            roomList[i].RoomPerson = roomPerson;
            roomList[i].RoomStatus = roomStatus;
        }

        for (int i=roomCount;i<roomList.Count;i++) {
            Destroy(roomList[i].gameObject);
        }

    }
}

