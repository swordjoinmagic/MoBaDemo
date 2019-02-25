using System.IO;
using UnityEngine;
using UnityEditor;
using LitJson;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;
using System.Collections;

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
    private SkillType skillType = SkillType.ChainSkill;    // 当前选中的技能的类型
    private BattleStateType battleStateType = BattleStateType.PoisoningState;       // 当前选中的技能所附加的状态的类型
    private Vector2 objectSlider = Vector2.zero;        // 对一个对象编辑区域的下滑块

    /// <summary>
    /// 读取技能数据
    /// </summary>
    public static JsonData Load() {
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

    /// <summary>
    /// 删除当前指定的技能对象
    /// </summary>
    private void Delete() {
        int oldIndex = selectedIndex;
        s.RemoveAt(oldIndex);
        selectedIndex = oldIndex == s.Count ? oldIndex-1:oldIndex+1;
        Debug.Log(SkillObjectList.ToJson());
        Debug.Log(SkillObjectList.Count);
        ((IList)SkillObjectList).RemoveAt((int)oldIndex);
        Debug.Log(SkillObjectList.ToJson());
        Debug.Log(SkillObjectList.Count);
    }

    [MenuItem("Data Editor/Skill Editor")]
    public static void CreateWindows() {
        SkillObjectList = Load();
        // 创建窗口
        Rect rect = new Rect(0,0,800,500);
        SkillEditor skillEditor = GetWindowWithRect<SkillEditor>(rect,true,"技能编辑器");
        skillEditor.Show();

        s = new List<string>();
        for (int i = 0; i < SkillObjectList.Count; i++) {
            s.Add(i.ToString()+":"+SkillObjectList[i]["SkillName"]);            
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
        GUILayout.BeginArea(new Rect(0,0,300,400));
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
        if (GUILayout.Button("Delete")) {
            Delete();
        }
        GUILayout.EndArea();

        

        //===============================================
        // 对单个技能对象进行编辑
        GUILayout.BeginArea(new Rect(325, 0, 375, 400));
        objectSlider = GUILayout.BeginScrollView(objectSlider, false, true);
        JsonData value = SkillPanel();
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

    private JsonData SkillPanel() {

        ObjectJsonData = SkillObjectList[selectedIndex];

        //===========================================================
        // 用于保存当前设计的对象的字段的字典，键是字段名，值是字段值
        skillType = (SkillType)EditorGUILayout.EnumPopup("技能类型",skillType);
        if (skillType.ToString() == "None" || skillType.ToString()== "Everything") return ObjectJsonData;

        // 获得对应技能类
        Type skillClass = GetTypeWithAnotherAsm(skillType.ToString());


        // 反射获得其所有属性
        ObjectJsonData["SkillType"] = skillType.ToString();
        PropertyInfo[] propertyInfos = skillClass.GetProperties();
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
                    s[selectedIndex] = selectedIndex+":"+SkillObjectList[selectedIndex][propertyInfo.Name].ToString();
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

            if (propertyInfo.PropertyType == typeof(BattleState)) {
                EditorGUILayout.LabelField("========BattleState=========================");
                battleStateType = (BattleStateType)EditorGUILayout.EnumPopup("状态类型", battleStateType);
                Type tempType = GetTypeWithAnotherAsm(battleStateType.ToString());
                ObjectJsonData[propertyInfo.Name] = CreateFiledPanel(tempType, ObjectJsonData.Get(propertyInfo.Name));
                EditorGUILayout.LabelField("============================================");
            }
            if (propertyInfo.PropertyType == typeof(Damage)) {
                EditorGUILayout.LabelField("========Damage==============================");
                ObjectJsonData[propertyInfo.Name] = CreateFiledPanel(propertyInfo.PropertyType, ObjectJsonData.Get(propertyInfo.Name));
                EditorGUILayout.LabelField("============================================");
            }
            if (propertyInfo.PropertyType.BaseType == typeof(Enum)) {
                if (propertyInfo.Name.Contains("TargetType")) {
                    // 多重枚举
                    ObjectJsonData[propertyInfo.Name] = (int)(UnitType)EditorGUILayout.EnumFlagsField(propertyInfo.Name, (UnitType)(int)ObjectJsonData.Get(propertyInfo.Name, typeof(int)));
                    Debug.Log(propertyInfo.Name+":"+ObjectJsonData[propertyInfo.Name]);
                } else {
                    // 普通枚举
                    ObjectJsonData[propertyInfo.Name] = (int)(Enum.ToObject(propertyInfo.PropertyType, EditorGUILayout.EnumPopup(propertyInfo.Name, (Enum)Enum.ToObject(propertyInfo.PropertyType, (int)ObjectJsonData.Get(propertyInfo.Name, typeof(int))))));
                }
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
        result["Type"] = type.Name;

        // 反射获得其所有属性
        PropertyInfo[] propertyInfos = type.GetProperties();

        EditorGUILayout.LabelField(type.Name + "属性：");
        foreach (var propertyInfo in propertyInfos) {
            if (propertyInfo.GetSetMethod() == null) continue;

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
