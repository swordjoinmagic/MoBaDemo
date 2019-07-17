using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

public static class DictonaryTools {
    public static JsonData Get(this JsonData jsondatas,string key,Type type=null) {
        JsonData data;
        try {
            data = jsondatas[key];
        } catch (Exception) {
            if (type == null || type == typeof(string))
                data = "";
            else if (type == typeof(int))
                data = 0;
            else if (type == typeof(float)) {
                data = 0f;
                Debug.Log("data:"+data.ToJson());
            } else if (type == typeof(bool))
                data = false;
            else
                data = "";
        }
        
        return data;
    }
    public static JsonData Get(this JsonData jsondatas, int key, Type type = null) {
        JsonData data;
        try {
            data = jsondatas[key];
        } catch (Exception) {
            if (type == null || type == typeof(string))
                data = "";
            else if (type == typeof(int))
                data = 0;
            else if (type == typeof(float)) {
                data = 0f;
                Debug.Log("data:" + data.ToJson());
            } else if (type == typeof(bool))
                data = false;
            else
                data = "";
        }

        return data;
    }
}

