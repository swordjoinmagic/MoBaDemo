using System.IO;
using UnityEngine;
using UnityEditor;

public class SkillEditor : EditorWindow {

    private string text;
    private string a;
    private GameObject gameObject;
    private int b = 0;
    private static string[] s;
    private Vector2 vector2 = Vector2.zero;

    /// <summary>
    /// 读取技能数据
    /// </summary>
    private void Load() {

    }

    [MenuItem("Data Editor/Skill Editor")]
    public static void CreateWindows() {
        // 创建窗口
        Rect rect = new Rect(0,0,500,500);
        SkillEditor skillEditor = GetWindowWithRect<SkillEditor>(rect,true,"技能编辑器");
        skillEditor.Show();

        s = new string[100];
        for (int i=0;i<100;i++) {
            s[i] = i.ToString();
        }
    }

    private void OnGUI() {


        //========================================================
        // 技能编号
        GUILayout.BeginArea(new Rect(0,0,100,400));
        vector2 = GUILayout.BeginScrollView(vector2, false,true);
        b = GUILayout.SelectionGrid(b,s,1);
        Debug.Log(b);
        GUILayout.EndScrollView();
        GUILayout.EndArea();


        SkillPanel();
    }

    private void OnInspectorUpdate() {
        Debug.Log("窗口面板更新");
        this.Repaint();
    }

    private void SkillPanel() {
        GUILayout.BeginArea(new Rect(125,0,375,400));
        GUILayout.BeginScrollView(Vector2.zero,false,true);
        GUILayout.TextField("技能名：");
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
