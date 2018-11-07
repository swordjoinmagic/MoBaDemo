using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 用于显示在物品栏的物品视图
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class ItemPanelView : UnityGuiView<ItemViewModel>{
    //===============================
    // 此视图管理的UI元素
    public Image iconImage;
    public Text remainCountText;

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("itemCount", OnRemainCountChanged);
        binder.Add<string>("iconPath",OnIconImageChanged);
    }

    /// <summary>
    /// 当物品的图片地址发生改变
    /// </summary>
    public void OnIconImageChanged(string oldIconImagePath,string newIconImagePath) {
        if (newIconImagePath != null) {
            iconImage.sprite = Resources.Load("UIImage/" + newIconImagePath, typeof(Sprite)) as Sprite;
            iconImage.color = new Color(1, 1, 1, 1);
        } else
            iconImage.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// 当物品剩余数量发生改变时
    /// </summary>
    public void OnRemainCountChanged(int oldItemCount,int newItemCount) {
        if (newItemCount != 0)
            remainCountText.text = newItemCount.ToString();
        else
            remainCountText.text = "";
    }
}

