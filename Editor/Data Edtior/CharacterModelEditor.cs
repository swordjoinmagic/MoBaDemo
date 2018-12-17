using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CharacterModelEditor : EditorWindow {

    //==================================
    // 编辑器列表选项
    // 当前选中的下标
    private int selectedIndex = 0;
    // 下标的标号数组
    private static List<string> s;
    // 下滑区域的滑块
    private Vector2 slider = Vector2.zero;

    // 技能对象列表
    private static JsonData SkillObjectList;

    // 人物对象列表
    private static JsonData CharacterModelList;

    // 程序集对象
    private static Assembly assembly;

    // 单个人物编辑时用于存储数据的JSON对象
    private JsonData ObjectJsonData = new JsonData();

    private Vector2 objectSlider = Vector2.zero;        // 对一个对象编辑区域的下滑块


    private void OnDestroy() {
        bool isSave = EditorUtility.DisplayDialog("警告！", "是否要保存数据", "Y", "N");
        if (isSave)
            Save();
    }

    /// <summary>
    /// 读取人物数据
    /// </summary>
    /// <returns></returns>
    public static JsonData Load() {
        string jsonText = Resources.Load<TextAsset>("Data/TestCharacterModelData").text;
        Debug.Log(jsonText);
        return JsonMapper.ToObject(jsonText);
    }

    /// <summary>
    /// 删除当前指定的人物对象
    /// </summary>
    private void Delete() {
        int oldIndex = selectedIndex;
        s.RemoveAt(oldIndex);
        selectedIndex = oldIndex == s.Count ? oldIndex - 1 : oldIndex + 1;
        Debug.Log(CharacterModelList.ToJson());
        Debug.Log(CharacterModelList.Count);
        ((IList)CharacterModelList).RemoveAt((int)oldIndex);
        Debug.Log(CharacterModelList.ToJson());
        Debug.Log(CharacterModelList.Count);
    }

    /// <summary>
    /// 保存人物数据至JSON文件中
    /// </summary>
    private static void Save() {
        string path = Application.dataPath + "/Resources/Data/TestCharacterModelData.json";
        File.WriteAllText(path, CharacterModelList.ToJson(), Encoding.UTF8);
        AssetDatabase.Refresh();
    }

    private static string[] GetAllSkillName() {
        List<string> strs = new List<string>();
        for (int i=0;i<SkillObjectList.Count;i++) {
            JsonData data = SkillObjectList[i];
            strs.Add(i+":"+data["SkillName"].ToString());
        }
        return strs.ToArray();
    }

    [MenuItem("Data Editor/CharacterModel Editor")]
    public static void CreateWindows() {

        // 读取技能数据
        SkillObjectList = SkillEditor.Load();
        CharacterModelList = CharacterModelEditor.Load();

        // 创建窗口
        Rect rect = new Rect(0, 0, 800, 500);
        CharacterModelEditor characterModelEditor = GetWindowWithRect<CharacterModelEditor>(rect, true, "人物编辑器");
        characterModelEditor.Show();

        s = new List<string>();
        for (int i = 0; i < CharacterModelList.Count; i++) {
            s.Add(i.ToString()+":"+ CharacterModelList[i]["Name"]);
        }
    }

    private JsonData CharacterModelPanel() {

        ObjectJsonData = CharacterModelList[selectedIndex];

        // 获得对应技能类
        Type characterModelClass = GetTypeWithAnotherAsm("CharacterModel");

        // 反射获得其所有属性
        PropertyInfo[] propertyInfos = characterModelClass.GetProperties();
        foreach (var propertyInfo in propertyInfos) {            
            if (propertyInfo.GetSetMethod() == null) continue;

            if (propertyInfo.PropertyType == typeof(string)) {
                if (!propertyInfo.Name.Contains("Description"))
                    ObjectJsonData[propertyInfo.Name] = EditorGUILayout.TextField(propertyInfo.Name, ObjectJsonData.Get(propertyInfo.Name).ToString());
                else {
                    EditorGUILayout.LabelField(propertyInfo.Name + ":");
                    ObjectJsonData[propertyInfo.Name] = EditorGUILayout.TextArea(ObjectJsonData.Get(propertyInfo.Name).ToString());
                }
                if (propertyInfo.Name.Contains("Name")) {
                    s[selectedIndex] = selectedIndex + ":" + CharacterModelList[selectedIndex][propertyInfo.Name].ToString();
                }
            }
            if (propertyInfo.PropertyType == typeof(float)) {
                ObjectJsonData[propertyInfo.Name] = EditorGUILayout.FloatField(propertyInfo.Name, (float)(double)ObjectJsonData.Get(propertyInfo.Name, type: typeof(float)));
            }
            if (propertyInfo.PropertyType == typeof(int)) {
                ObjectJsonData[propertyInfo.Name] = EditorGUILayout.IntField(propertyInfo.Name, (int)ObjectJsonData.Get(propertyInfo.Name, typeof(int)));
            }

            // 对整形数组的处理
            if (propertyInfo.PropertyType.IsArray && propertyInfo.PropertyType.GetElementType() == typeof(int)) {

            }

            // 对技能列表的处理
            if (propertyInfo.PropertyType == typeof(List<BaseSkill>)) {
                EditorGUILayout.LabelField("========List<BaseSkill>=========================");
                int maxCount = 1;
                try {
                    maxCount = ObjectJsonData[propertyInfo.Name].Count;
                } catch (Exception e) {
                    ObjectJsonData[propertyInfo.Name] = new JsonData();
                    ObjectJsonData[propertyInfo.Name].Add(0);
                }

                Debug.Log("maxCount:"+maxCount);

                if (GUILayout.Button("增加技能")) {
                    ObjectJsonData[propertyInfo.Name].Add(0); 
                }

                for (int i=0;i<maxCount; i++)
                    ObjectJsonData[propertyInfo.Name][i] = EditorGUILayout.Popup((int)ObjectJsonData[propertyInfo.Name].Get(i, typeof(int)), GetAllSkillName());
                
                EditorGUILayout.LabelField("============================================");
            }

            if (propertyInfo.PropertyType.BaseType == typeof(Enum)) {
                if (propertyInfo.Name.Contains("TargetType")) {
                    // 多重枚举
                    ObjectJsonData[propertyInfo.Name] = (int)(UnitType)EditorGUILayout.EnumFlagsField(propertyInfo.Name, (UnitType)(int)ObjectJsonData.Get(propertyInfo.Name, typeof(int)));
                } else {
                    // 普通枚举
                    ObjectJsonData[propertyInfo.Name] = (int)(Enum.ToObject(propertyInfo.PropertyType, EditorGUILayout.EnumPopup(propertyInfo.Name, (Enum)Enum.ToObject(propertyInfo.PropertyType, (int)ObjectJsonData.Get(propertyInfo.Name, typeof(int))))));
                }
            }
        }
        return ObjectJsonData;
    }



    private void OnGUI() {
        //========================================================
        // 人物编号
        GUILayout.BeginArea(new Rect(0, 0, 300, 400));
        slider = GUILayout.BeginScrollView(slider, false, true);
        selectedIndex = GUILayout.SelectionGrid(selectedIndex, s.ToArray(), 1);
        GUILayout.EndScrollView();
        if (GUILayout.Button("创建新的单位")) {
            s.Add(s.Count.ToString());
            CharacterModelList.Add(new JsonData());
        }
        if (GUILayout.Button("Save")) {
            Save();
        }
        if (GUILayout.Button("Delete")) {
            Delete();
        }
        GUILayout.EndArea();

        //===============================================
        // 对单个对象进行编辑
        GUILayout.BeginArea(new Rect(325, 0, 375, 400));
        objectSlider = GUILayout.BeginScrollView(objectSlider, false, true);
        CharacterModelPanel();
        GUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    private void OnInspectorUpdate() {
        this.Repaint();
    }

    private Type GetTypeWithAnotherAsm(string type) {
        Type typeClass = null;
        // 遍历程序集
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
            Type t = asm.GetType(type);
            if (t != null) typeClass = t;
        }
        return typeClass;
    }


}

