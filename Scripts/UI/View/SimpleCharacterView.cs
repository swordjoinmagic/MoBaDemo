using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using uMVVM;

/// <summary>
/// 简易人物窗口，用于简单显示人物头顶上的hp条及名字
/// </summary>
public class SimpleCharacterView : UnityGuiView<SimpleCharacterViewModel>{

    //=============================
    // 此View管理的控件
    public RectTransform hpImage;
    public RectTransform maxRectTransform;
    public Text nameText;


    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("Hp",OnHpValueChanged);
        binder.Add<string>("name",OnNameValueChanged);
    }



    private void OnHpValueChanged(int oldHp,int newHp) {
        StartCoroutine(
            Util.SlowDown(Math.Abs(oldHp-newHp),oldHp,newHp,
            BindingContext.maxHp.Value,hpImage, (int)(maxRectTransform.sizeDelta.y), (int)(maxRectTransform.sizeDelta.x))
        );
    }
    private void OnNameValueChanged(string oldName,string newName) {
        nameText.text = newName;
    }

}

