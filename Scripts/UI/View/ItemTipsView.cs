using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.UI;

public class ItemTipsView : UnityGuiView<ItemViewModel> {

    //===================================
    // 此View管理的UI控件
    public Image iconImage;
    public Text itemNameText;
    public Text passiveDescriptionText;
    public Text useMethodDescriptionText;
    public Text itemCDText;
    public Text activeDescriptionText;
    public Text backgroundDescriptionText;

    //======================================
    // 用于高度自适应的UI控件
    public RectTransform ItemTipsPanel;
    public RectTransform PassiveEffectPanel;
    public RectTransform ActiveEffectDescriptionPanel;
    public RectTransform UseMethodAndCDPanel;
    public RectTransform ItemImageNamePanel;
    public RectTransform ItemBackgroundDescriptionPanel;

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<string>("name", OnNameChanged);
        binder.Add<string>("passiveDescription", OnPassiveDescriptionChanged);
        binder.Add<string>("useMethodDescription", OnUseMethodDescriptionChanged);
        binder.Add<string>("activeDescription", OnActiveDescriptionChanged);
        binder.Add<string>("backgroundDescription", OnBackgroundDescriptionChanged);
        binder.Add<string>("itemCD", OnItemCDTextChanged);
    }

    private void OnIconPathChanged(string oldIconPath, string newIconPath) {
        iconImage.sprite = Resources.Load("UIImage/" + newIconPath, typeof(Sprite)) as Sprite;
    }
    private void OnNameChanged(string oldName, string newName) {
        itemNameText.text = newName;
    }
    private void OnPassiveDescriptionChanged(string oldDescription, string newDescription) {
        passiveDescriptionText.text = newDescription;

        int cows = newDescription.FindAnyCharCount('\n') + newDescription.Length / 14;
        Debug.Log("passiveDescritpion Cows:"+cows);
        PassiveEffectPanel.sizeDelta = new Vector2(
            PassiveEffectPanel.sizeDelta.x,
            cows*12.5f
        );
        HeightAdaptive();
    }
    private void OnUseMethodDescriptionChanged(string oldDescription, string newDescription) {
        useMethodDescriptionText.text = newDescription;
    }
    private void OnItemCDTextChanged(string oldCD, string newCD) {
        itemCDText.text = newCD;
    }
    private void OnActiveDescriptionChanged(string oldDescription, string newDescription) {
        activeDescriptionText.text = newDescription;

        // 当技能描述为空时，说明此物品不具有主动技能，
        // 将主动技能面板的高度设为0
        if (newDescription!=null && newDescription != "") {
            int cows = newDescription.FindAnyCharCount('\n') + newDescription.Length / 14;
            Debug.Log("ActiveDescription cows:"+cows);
            ActiveEffectDescriptionPanel.sizeDelta = new Vector2(
                ActiveEffectDescriptionPanel.sizeDelta.x,
                cows * 12.5f + UseMethodAndCDPanel.sizeDelta.y + 7
            );
        } else {
            ActiveEffectDescriptionPanel.sizeDelta = new Vector2(
                ActiveEffectDescriptionPanel.sizeDelta.x,
                0
            );
            UseMethodAndCDPanel.sizeDelta = new Vector2(
                UseMethodAndCDPanel.sizeDelta.x,
                0
            );
        }
        HeightAdaptive();
    }
    private void OnBackgroundDescriptionChanged(string oldDescription, string newDescription) {
        backgroundDescriptionText.text = "物品描述：\n"+newDescription;

        if (newDescription != null && newDescription != "") {
            int cows = newDescription.Length / 15 + 2;
            ItemBackgroundDescriptionPanel.sizeDelta = new Vector2(
                ItemBackgroundDescriptionPanel.sizeDelta.x,
                cows * 11f
            );
        } else {
            ItemBackgroundDescriptionPanel.sizeDelta = new Vector2(
                ItemBackgroundDescriptionPanel.sizeDelta.x,
                0
            );
        }

        HeightAdaptive();
    }


    /// <summary>
    /// 高度自适应
    /// </summary>
    public void HeightAdaptive() {
        // ActiveEffectPanel的位置由PassiveEffectPanel的高度决定
        ActiveEffectDescriptionPanel.anchoredPosition = new Vector2(
            ActiveEffectDescriptionPanel.anchoredPosition.x,
            -PassiveEffectPanel.sizeDelta.y - 4
        );
        ItemBackgroundDescriptionPanel.anchoredPosition = new Vector2(
            ItemBackgroundDescriptionPanel.anchoredPosition.x,
            ActiveEffectDescriptionPanel.anchoredPosition.y - 
            ActiveEffectDescriptionPanel.sizeDelta.y - 
            ItemBackgroundDescriptionPanel.sizeDelta.y
        );
        Debug.Log("ActiveEffectDescriptionPanel.anchoredPosition.y - ActiveEffectDescriptionPanel.sizeDelta.y - ItemBackgroundDescriptionPanel.sizeDelta.y:"+ (ActiveEffectDescriptionPanel.anchoredPosition.y - ActiveEffectDescriptionPanel.sizeDelta.y - ItemBackgroundDescriptionPanel.sizeDelta.y));
        ItemTipsPanel.sizeDelta = new Vector2(
            ItemTipsPanel.sizeDelta.x,
            ItemImageNamePanel.sizeDelta.y+
            PassiveEffectPanel.sizeDelta.y+
            ActiveEffectDescriptionPanel.sizeDelta.y+
            ItemBackgroundDescriptionPanel.sizeDelta.y+
            25
        );
    }
}

