using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录视图
/// </summary>
public class LoginUIView : MonoBehaviour {

    public RoomListView roomListView;
    public InputField userNameInputField;
    public InputField passwordInputField;

    private RectTransform rectTransform;
    private System.Action OnHideCompelteAction;

    private void Start() {
        
    }

    public void Show() {
        this.gameObject.SetActive(true);

        rectTransform = transform as RectTransform;

        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;

        rectTransform.sizeDelta = Vector2.zero;

        rectTransform.DOSizeDelta(new Vector2(width,height),1f);
    }

    public void Hide() {
        rectTransform.DOScale(new Vector3(0, 0, 0), 1f).onComplete += () => {
            if (OnHideCompelteAction != null) OnHideCompelteAction();
        };        
    }

    public void Login() {
        NetWorkManager.Instance.SendLogin(userNameInputField.text,passwordInputField.text);
        NetWorkManager.Instance.AddListener("LoginResultConn", OnLogin);
        Debug.Log("发送登录消息");
    }

    private void OnLogin(ProtocolBytes protocol) {
        string userName = protocol.GetString();
        string loginResult = protocol.GetString();
        if (loginResult == "Success") {
            Hide();
            OnHideCompelteAction += () => { roomListView.Show(); };

            // 设置本地客户端游玩玩家的ID为登录名
            NetWorkManager.Instance.OnLoginSuccess(userName);
        }
        Debug.Log("本地玩家的ID是："+NetWorkManager.Instance.NowPlayerID);
    }
}
