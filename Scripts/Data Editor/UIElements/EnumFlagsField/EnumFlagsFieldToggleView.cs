using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnumFlagsFieldToggleView : MonoBehaviour{
    List<Toggle> toggles = new List<Toggle>();
    Type enumType;

    public Toggle toggleTemplate;
    public EnumFlagsFieldDropDown dropDown;

    private int TotalValue {
        get {
            if (enumType == null) return -1;
            int v = 0;
            foreach (int i in Enum.GetValues(enumType)) {
                v += i;
            }
            return v+1;
        }
    }

    // 当前所有选项的总的值
    // 每增加一个选项即 当前值 = value |(或) 新增选项的值
    // 每减少一个选项即 当前值 = value ^(异或) 新增选项的值
    // 需要说明的是,当value为0时,None自动勾上,
    // 当value为当前枚举的最大值(即枚举里所有值的总和)时,Everything自动勾上
    int value = 0;      // 默认为None

    public int Value {
        get {
            return value;
        }

        set {            
            this.value = value;

            if (value == 0) {
                dropDown.CurrentValue = "None";
            } else if (value == TotalValue) {
                dropDown.CurrentValue = "Everything";
            } else {
                string currentValue = Enum.ToObject(enumType, value).ToString();
                try {
                    int number = int.Parse(currentValue);
                    dropDown.CurrentValue = "Mixed~";
                } catch (Exception e) {
                    Debug.LogWarning(e.Message);
                    dropDown.CurrentValue = currentValue;
                }
            }
        }
    }

    public void AddToggles(Type enumType) {
        this.enumType = enumType;
        AddToggles(Enum.GetNames(enumType));
    }
    public void AddToggles(Enum @enum) {
        enumType = @enum.GetType();
        AddToggles(Enum.GetNames(@enum.GetType()));
    }
    private void AddToggles(string[] toggleNames) {
        for (int i=0;i<toggleNames.Length;i++) {
            string toggleName = toggleNames[i];

            if (toggleName == "Everything" || toggleName == "None") {
                continue;
            }

            Toggle toggle = GameObject.Instantiate<Toggle>(toggleTemplate,transform);
            toggle.GetComponentInChildren<Text>().text = toggleName;
            toggle.gameObject.SetActive(true);

            // 当前枚举项的枚举值
            int enumValue = (int)Enum.Parse(enumType, toggleName);
            toggle.onValueChanged.AddListener((bool toggleValue) => {
                if (toggleValue)
                    Value |= enumValue;
                else
                    Value ^= enumValue;
            });
            

            toggles.Add(toggle);
        }
    }
}

