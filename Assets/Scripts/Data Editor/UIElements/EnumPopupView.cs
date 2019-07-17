using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 枚举类型的单选下拉框
/// </summary>
public class EnumPopupView : MonoBehaviour {

    // 下拉框
    Dropdown dropdown;
    // 当前下拉框枚举的类型
    Type enumType;

    private Dropdown Dropdown{
        get{
            if (dropdown == null)
                dropdown = GetComponentInChildren<Dropdown>();
            return dropdown;
        }
    }

    public void Init(Enum @enum) {
        enumType = @enum.GetType();
        string[] enums = Enum.GetNames(enumType);
        Dropdown.AddOptions(new List<string>(enums));
    }

    public string Value {
        get {
            return Dropdown.options[Dropdown.value].text;
        }
    }

}

