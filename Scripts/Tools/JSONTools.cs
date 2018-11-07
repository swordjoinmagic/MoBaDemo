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
            } else if (jsonValue.IsObject) {
                value = (object)jsonValue;
            }

            // 对GameObject和Texture2D和Audiosource进行特殊处理
            if (property.PropertyType==typeof(GameObject)) {
                string loadPath = (string)jsonValue;
                // 判断是否是技能特效类别的GameObject
                if (key.Contains("Effect"))
                    loadPath = "SkillEffect/" + loadPath;

                value = Resources.Load<GameObject>(loadPath);
            }

            property.SetValue(newIntance, value, null);
        }

        return newIntance;
    }
}

