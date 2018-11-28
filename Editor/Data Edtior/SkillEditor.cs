using System.IO;
using UnityEngine;
using UnityEditor;
using LitJson;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;

public class SkillEditor : EditorWindow {

    // 技能对象列表
    private static JsonData SkillObjectList;

    // 程序集对象
    private static Assembly assembly;
    // 单个技能编辑时用于存储数据的JSON对象
    private JsonData ObjectJsonData = new JsonData();

    //==================================
    // 编辑器列表选项
    // 当前选中的下标
    private int selectedIndex = 0;
    // 下标的标号数组
    private static List<string> s ;
    // 下滑区域的滑块
    private Vector2 slider = Vector2.zero;


    //===================================
    // 对一个对象的编辑选项
    private SkillType skillType = SkillType.CritSkill;    // 当前选中的技能的类型
    private Vector2 objectSlider = Vector2.zero;        // 对一个对象编辑区域的下滑块

    /// <summary>
    /// 读取技能数据
    /// </summary>
    private static JsonData Load() {
        string jsonText = Resources.Load<TextAsset>("Data/TestData").text;

        return JsonMapper.ToObject(jsonText);
    }

    /// <summary>
    /// 保存技能数据至JSON文件中
    /// </summary>
    private static void Save() {
        string path = Application.dataPath+ "/Resources/Data/TestData.json";
        File.WriteAllText(path, SkillObjectList.ToJson(), Encoding.UTF8);
        AssetDatabase.Refresh();
    }

    [MenuItem("Data Editor/Skill Editor")]
    public static void CreateWindows() {
        SkillObjectList = Load();
        // 创建窗口
        Rect rect = new Rect(0,0,500,500);
        SkillEditor skillEditor = GetWindowWithRect<SkillEditor>(rect,true,"技能编辑器");
        skillEditor.Show();

        s = new List<string>();
        for (int i = 0; i < SkillObjectList.Count; i++) {
            s.Add(i.ToString());
        }

    }

    private void OnDestroy() {
        bool isSave = EditorUtility.DisplayDialog("警告！","是否要保存数据","Y","N");
        if(isSave)
            Save();
    }

    private void OnGUI() {


        //========================================================
        // 技能编号
        GUILayout.BeginArea(new Rect(0,0,100,400));
        slider = GUILayout.BeginScrollView(slider, false,true);
        selectedIndex = GUILayout.SelectionGrid(selectedIndex,s.ToArray(),1);
        GUILayout.EndScrollView();
        if (GUILayout.Button("创建新的技能")) {
            s.Add(s.Count.ToString());
            SkillObjectList.Add(new JsonData());
        }
        if (GUILayout.Button("Save")) {
            Save();
        }
        GUILayout.EndArea();

        //===============================================
        // 对单个技能对象进行编辑
        GUILayout.BeginArea(new Rect(125, 0, 375, 400));
        objectSlider = GUILayout.BeginScrollView(objectSlider, false, true);
        JsonData value = SkillPanel();
        if (GUILayout.Button("Save")) {
            Debug.Log(value.ToJson());
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void OnInspectorUpdate() {
        this.Repaint();
    }

    private JsonData SkillPanel() {

        ObjectJsonData = SkillObjectList[selectedIndex];

        //===========================================================
        // 用于保存当前设计的对象的字段的字典，键是字段名，值是字段值
        skillType = (SkillType)EditorGUILayout.EnumPopup("技能类型",skillType);
        if (skillType.ToString() == "None" || skillType.ToString()== "Everything") return ObjectJsonData;

        string skillClassName = skillType.ToString();
        // 获得对应技能类
        Type skillClass = null;
        // 遍历程序集
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
            Type t = asm.GetType(skillClassName);
            if (t != null) skillClass = t;
        }

        // 反射获得其所有属性
        ObjectJsonData["SkillType"] = skillType.ToString();
        PropertyInfo[] propertyInfos = skillClass.GetProperties();
        foreach (var propertyInfo in propertyInfos) {
            if (propertyInfo.PropertyType == typeof(string)) {
                if (!propertyInfo.Name.Contains("Description"))
                    ObjectJsonData[propertyInfo.Name] = EditorGUILayout.TextField(propertyInfo.Name, ObjectJsonData.Get(propertyInfo.Name).ToString());
                else {
                    EditorGUILayout.LabelField(propertyInfo.Name + ":");
                    ObjectJsonData[propertyInfo.Name] = EditorGUILayout.TextArea(ObjectJsonData.Get(propertyInfo.Name).ToString());
                }
            }
            if (propertyInfo.PropertyType == typeof(float)) {
                ObjectJsonData[propertyInfo.Name] = EditorGUILayout.FloatField(propertyInfo.Name, (float)(double)ObjectJsonData.Get(propertyInfo.Name, type: typeof(float)));
            }
            if (propertyInfo.PropertyType == typeof(int)) {
                ObjectJsonData[propertyInfo.Name] = EditorGUILayout.IntField(propertyInfo.Name, (int)ObjectJsonData.Get(propertyInfo.Name, typeof(int)));
            }
            if (propertyInfo.PropertyType == typeof(BattleState)) {
                EditorGUILayout.LabelField("============================================");
                ObjectJsonData[propertyInfo.Name] = CreateFiledPanel(propertyInfo.PropertyType, ObjectJsonData.Get(propertyInfo.Name));
                EditorGUILayout.LabelField("============================================");
            }
            if (propertyInfo.PropertyType == typeof(Damage)) {
                EditorGUILayout.LabelField("============================================");
                ObjectJsonData[propertyInfo.Name] = CreateFiledPanel(propertyInfo.PropertyType, ObjectJsonData.Get(propertyInfo.Name));
                EditorGUILayout.LabelField("============================================");
            }
        }
        return ObjectJsonData;
    }


    /// <summary>
    /// 根据一个对象生成其所有可填充字段，
    /// 并将这些可填充字段保存在一个JSON中，
    /// 在这个JSON中，可填充字段的字段名为键，可填充字段的值为值
    /// </summary>
    /// <param name="type"></param>
    /// <param name="preData">上一次的值</param>
    /// <returns></returns>
    public JsonData CreateFiledPanel(Type type,JsonData preData) {
        JsonData result = new JsonData();

        // 反射获得其所有属性
        PropertyInfo[] propertyInfos = type.GetProperties();

        EditorGUILayout.LabelField(type.Name + "属性：");
        foreach (var propertyInfo in propertyInfos) {
            if (propertyInfo.PropertyType == typeof(string)) {
                if (!propertyInfo.Name.Contains("Description"))
                    result[propertyInfo.Name] = EditorGUILayout.TextField(propertyInfo.Name, preData.Get(propertyInfo.Name).ToString());
                else {
                    EditorGUILayout.LabelField(propertyInfo.Name+":");
                    result[propertyInfo.Name] = EditorGUILayout.TextArea(preData.Get(propertyInfo.Name).ToString());
                }
            }
            if (propertyInfo.PropertyType == typeof(float)) {
                result[propertyInfo.Name] = EditorGUILayout.FloatField(propertyInfo.Name, (float)(double)preData.Get(propertyInfo.Name, type: typeof(float)));
            }
            if (propertyInfo.PropertyType == typeof(int)) {
                result[propertyInfo.Name] = EditorGUILayout.IntField(propertyInfo.Name, (int)preData.Get(propertyInfo.Name, typeof(int)));
            }
        }

        return result;
    }
}
