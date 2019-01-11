using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 多重枚举复选框
/// </summary>
public class EnumFlagsFieldDropDown : Selectable, IPointerClickHandler {

    // 下拉的多选框的整体
    public GameObject contentView;
    // 多选框视图
    public EnumFlagsFieldToggleView toggleView;
    // 该下拉框当前显示的Text控件
    public Text currentValueText;


    // 该下拉框当前显示的字符
    private string currentValue;

    Enum @enum;
    public Enum Enum {
        set {
            @enum = value;
            toggleView.AddToggles(value);
        }
    }

    public string CurrentValue {
        get {
            return currentValue;
        }

        set {
            currentValue = value;
            currentValueText.text = value;
        }
    }    

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) {
        contentView.SetActive(!contentView.activeSelf);
    }
}

