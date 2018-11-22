using System;
using System.Collections;
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
    public RectTransform slowDownHpImage;
    public RectTransform maxRectTransform;
    public Text nameText;


    private Coroutine slowDownHp = null;

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("Hp",OnHpValueChanged);
        binder.Add<string>("name",OnNameValueChanged);
    }



    private void OnHpValueChanged(int oldHp,int newHp) {
        float width = ((float)newHp / (float)BindingContext.maxHp.Value) * maxRectTransform.sizeDelta.x;
        hpImage.sizeDelta = new Vector2(width, hpImage.sizeDelta.y);

        if (slowDownHp != null) {
            StopCoroutine(slowDownHp);
        }
        slowDownHp = StartCoroutine(
            Util.SlowDown(hpImage, slowDownHpImage, maxRectTransform.sizeDelta.y)
        );
    }
    private void OnNameValueChanged(string oldName,string newName) {
        nameText.text = newName;
    }

}

