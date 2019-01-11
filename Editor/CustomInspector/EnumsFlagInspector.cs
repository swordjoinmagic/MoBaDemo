using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(EnumFlagsFieldView))]
public class EnumsFlagInspector : Editor{
    EnumFlagsFieldView flagsFieldView;
    List<string> gameTypes = new List<string>();
    int index = 0;

    private void OnEnable() {
        flagsFieldView = (EnumFlagsFieldView)target;

        // 初始化游戏类型
        LoadGameTypes();
    }

    public void LoadGameTypes() {
        Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblys) {
            foreach (var type in assembly.GetExportedTypes()) {
                object[] types = (GameType[])type.GetCustomAttributes(typeof(GameType), false);
                if (types.Length != 0)
                    gameTypes.Add(type.ToString());
            }
        }
    }

    public override void OnInspectorGUI() {

        if (gameTypes.Count > 0) {
            EditorGUILayout.LabelField("显示的类型:");

            flagsFieldView.index = EditorGUILayout.Popup(flagsFieldView.index, gameTypes.ToArray());

            flagsFieldView.typeName = gameTypes[flagsFieldView.index];
        }
    }
}

