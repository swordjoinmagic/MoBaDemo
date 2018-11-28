using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LitJson;
using UnityEngine;

/// <summary>
/// 基于反射，对JSON中的数据进行属性的自动设置
/// 
/// 在此情况下，进行了一定的扩展，比如，对于GameObject类型的数据，在JSON中是字符串，
/// 而在运行程序中，自动对Resources文件夹进行读取该GameObject
/// </summary>
public class JSONTools {

    public static T JSONToObject<T>(string json) where T:new() {
        JsonData jsonData = JsonMapper.ToObject(json);
        return JSONToObject<T>(jsonData);
    }

    public static T JSONToObject<T>(JsonData jsonData) where T:new() {
        T newIntance = Activator.CreateInstance<T>();
        foreach (var key in jsonData.Keys) {
            Debug.Log("Key:"+key);
            PropertyInfo property = typeof(T).GetProperty(key);

            JsonData jsonValue = jsonData[key];

            object value = null;

            if (jsonValue.IsInt) {
                value = (int)jsonValue;
            } else if (jsonValue.IsString) {
                value = (string)jsonValue;
            } else if (jsonValue.IsLong) {
                value = (long)jsonValue;
            } else if (jsonValue.IsDouble) {
                value = (float)(double)jsonValue;
            } else if (jsonValue.IsBoolean) {
                value = (bool)jsonValue;
            } else if (jsonValue.IsArray) {
                Type arrayType = property.PropertyType.GetElementType();
                if (arrayType == typeof(int)) {
                    int[] list = new int[jsonValue.Count];
                    for (int i=0;i<jsonValue.Count;i++) {
                        list[i] = (int)jsonValue[i];
                    }
                    value = list;
                }
            } else if (jsonValue.IsObject) {
                value = (object)jsonValue;
            }

            // 对GameObject、Texture2D和Audiosource进行特殊处理
            if (property.PropertyType == typeof(GameObject)) {
                string loadPath = (string)jsonValue;
                // 判断是否是技能特效类别的GameObject
                if (key.Contains("Effect"))
                    loadPath = "SkillEffect/" + loadPath;

                value = Resources.Load<GameObject>(loadPath);
            } else if (property.PropertyType == typeof(BattleState)) {
                Debug.Log("该对象是BattleState对象:"+jsonValue);
                // 该对象是一个BattlState对象，说明此处获得的JSONValue也是一个字典
                value = JSONToObject<BattleState>(jsonValue);
            }

            property.SetValue(newIntance, value, null);
        }

        return newIntance;
    }
}

