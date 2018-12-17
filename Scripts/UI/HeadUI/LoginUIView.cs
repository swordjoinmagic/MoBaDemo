using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录视图
/// </summary>
public class LoginUIView : MonoBehaviour {

    public InputField userNameInputField;
    public InputField passwordInputField;

    public void Login() {
        NetWorkManager.Instance.SendLogin(userNameInputField.text,passwordInputField.text);
        NetWorkManager.Instance.AddListener("LoginResultConn", (protocolBytes) => {
            Debug.Log("处理LoginResultConn协议");
            Debug.Log("用户名:" + protocolBytes.GetString());
            Debug.Log("登录状态:" + protocolBytes.GetString());
        });
        Debug.Log("发送登录消息");
    }
}
