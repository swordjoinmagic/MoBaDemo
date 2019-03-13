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
    }
    private void OnUseMethodDescriptionChanged(string oldDescription, string newDescription) {
        useMethodDescriptionText.text = newDescription;
    }
    private void OnItemCDTextChanged(string oldCD, string newCD) {
        itemCDText.text = newCD;
    }
    private void OnActiveDescriptionChanged(string oldDescription, string newDescription) {
        activeDescriptionText.text = newDescription;
    }
    private void OnBackgroundDescriptionChanged(string oldDescription, string newDescription) {
        backgroundDescriptionText.text = "物品描述：\n"+newDescription;
    }
}

