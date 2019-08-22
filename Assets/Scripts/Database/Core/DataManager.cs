using System;
using System.Linq;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;

/// <summary>
/// 游戏数据管理类,用于对游戏外部数据进行管理
/// <para>
///     该类首先读取外部的游戏数据,得到数据后,生成一系列的模板对象,
///     放进一个个列表中(这些列表放在一个字典中).
///     在游戏中,需要引用这些模板对象.
/// </para>
/// <para>
///     对于单位、技能、物品这三类数据,分别用了三个泛型列表来存储.
///     其原因是,如果使用一个字典来存上述数据,必须使用object类型,
///     到使用的时候进行强转,这会带来一定的危险性.
/// </para>
/// </summary>
class DataManager {

    private static DataManager instance;

    public static DataManager Instance {
        get {
            if (instance == null) {
                instance = new DataManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// 单位数据集合
    /// </summary>
    public List<CharacterModel> characterModelDataSet = new List<CharacterModel>();
    // 单位数据JSON的地址
    private string characterModelDataPath = "";

    /// <summary>
    /// 技能数据集合
    /// </summary>
    public List<BaseSkill> baseSkillDataSet = new List<BaseSkill>();
    // 技能数据JSON的地址
    private string skillDataPath = "TestData";

    /// <summary>
    /// 物品数据集合
    /// </summary>
    public List<Item> itemDataSet = new List<Item>();
    // 物品数据JSON的地址
    private string itemDataPath = "";

    private Type MethodType;

    private MethodInfo GenericMethod;

    /// <summary>
    /// 初始化操作
    /// 用于缓冲泛型方法
    /// </summary>
    private void Init() {

        // 获取要执行的泛型方法的类
        MethodType = typeof(JSONTools);

        // 获取泛型方法
        GenericMethod = MethodType.GetMethod("JSONToObject", new Type[] { typeof(JsonData) });

    }

    /// <summary>
    /// 对外部游戏数据进行读取,并以列表的形式保存
    /// </summary>
    public void LoadData() {

        float time = Time.time;

        // 缓存泛型方法
        Init();

        //============================================
        // 读取技能数据
        TestLoadSkillData();


    }

    /// <summary>
    /// 读取技能数据
    /// </summary>
    public void LoadSkillData() {
        string skillJson = Resources.Load<TextAsset>("Data/" + skillDataPath).text;

        skillJson = Regex.Replace(skillJson, " |\n|\t", "");
        // 获得在JSON数据中的技能列表数据
        JsonData skillJsonList = JsonMapper.ToObject(skillJson)["SkillList"];
        foreach (JsonData skillData in skillJsonList) {
            SkillType skillType = (SkillType)(int)skillData["SkillType"];

            // 根据skilltype获取对应技能类
            Type SkillClass = Type.GetType(skillType.ToString());

            // 根据skillType技能类型与泛型方法进行合并，生成最终的方法
            MethodInfo methodInfo = GenericMethod.MakeGenericMethod(SkillClass);

            // 执行该泛型函数
            BaseSkill skill = methodInfo.Invoke(null, new object[] { skillData }) as BaseSkill;

            baseSkillDataSet.Add(skill);
        }
    }

    public void TestLoadSkillData() {
        string skillJson = Resources.Load<TextAsset>("Data/TestData").text;
        skillJson = Regex.Replace(skillJson, " |\n|\t", "");
        Debug.Log(skillJson);
        // 获得在JSON数据中的技能列表数据
        JsonData skillJsonList = JsonMapper.ToObject(skillJson);
        Debug.Log("skillData:" + skillJsonList.ToJson());
        foreach (JsonData skillData in skillJsonList) {
            
            // 根据skilltype获取对应技能类
            Type SkillClass = Type.GetType(skillData["SkillType"].ToString());

            Debug.Log(skillData["SkillType"].ToString());

            // 根据skillType技能类型与泛型方法进行合并，生成最终的方法
            MethodInfo methodInfo = GenericMethod.MakeGenericMethod(SkillClass);

            // 执行该泛型函数
            BaseSkill skill = methodInfo.Invoke(null, new object[] { skillData }) as BaseSkill;

            baseSkillDataSet.Add(skill);
        }
    }
}

