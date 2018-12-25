using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 用于显示在商店的物品视图
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class StoreItemPanelView : UnityGuiView<ItemViewModel>{
    //===============================
    // 此视图管理的UI元素
    public Image iconImage;
    public Text remainCountText;
    public Outline outline;
    public ItemGrid itemGrid;
    // 冷却的图形
    public Image coolDownImage;

    private void Init() {
        //==========================================
        // 监听CharacterMono的物品改变事件
        itemGrid.OnIconPathChanged += OnIconImageChanged;
        itemGrid.OnItemCountChanged += OnRemainCountChanged;
    }

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("itemCount", OnRemainCountChanged);
        binder.Add<string>("iconPath",OnIconImageChanged);

        if(outline != null)
            binder.Add<bool>("outlineColor",OnOutlineColorChanged);
        Init();
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

    public void OnOutlineColorChanged(bool oldValue,bool newValue) {
        if (newValue) {
            outline.effectColor = Color.green;
        } else {
            outline.effectColor = Color.red;
        }
    }

    private void Update() {
        if (itemGrid!=null && itemGrid.item != null && itemGrid.IsCoolDowning) {
            // 更新冷却条
            if (itemGrid.item.itemPayInteral != 0 && itemGrid.LatestBuyTime!=0) {
                float rate = Mathf.Clamp01((Time.time - itemGrid.LatestBuyTime) / itemGrid.item.itemPayInteral);
                coolDownImage.fillAmount = 1 - rate;
                if (rate == 1 && itemGrid.IsCoolDowning) {
                    itemGrid.IsCoolDowning = false;
                }
            }
        }
    }
}

