using UnityEngine;
using UnityEditor;

public class ApplicationQuitTest : MonoBehaviour{
    private void OnApplicationQuit() {
        Debug.Log("退出游戏");
        //bool result = EditorUtility.DisplayDialog("是否要退出游戏!", "是否要退出游戏?", "是","否");
        //if (!result) {
        //    Application.CancelQuit(); 
        //}
        
    }
}
