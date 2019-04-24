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
public class SimpleHPView : UnityGuiView<SimpleCharacterViewModel>{

    //=============================
    // 此View管理的控件
    public RectTransform hpImage;
    public RectTransform slowDownHpImage;
    public RectTransform maxRectTransform;
    public Text nameText;

    // 此视图依附的单位
    public CharacterMono characterMono;

    private void Init() {
        characterMono.characterModel.HpValueChangedHandler += OnHpValueChanged;
    }

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("Hp",OnHpValueChanged);
        binder.Add<string>("name",OnNameValueChanged);
        Init();
    }


    private void OnHpValueChanged(int oldHp,int newHp) {
        float width = ((float)newHp / (float)BindingContext.maxHp.Value) * maxRectTransform.sizeDelta.x;
        hpImage.sizeDelta = new Vector2(width, hpImage.sizeDelta.y);
    }
    private void OnNameValueChanged(string oldName,string newName) {
        nameText.text = newName;
    }

    
    private void FixedUpdate() {
        // 当dataRectTransform的宽度小于hpImageRectTransform时，停止更新
        if (Mathf.Abs(slowDownHpImage.sizeDelta.x - hpImage.sizeDelta.x) > 0.1f) {

            // 每次变化的量，每次按变化的5%来递减\增(最小变化量为0.1)
            float step = (hpImage.sizeDelta.x - slowDownHpImage.sizeDelta.x) * 0.02f;

            // 血条变化
            slowDownHpImage.sizeDelta = new Vector2(slowDownHpImage.sizeDelta.x + step, maxRectTransform.sizeDelta.y);

        }
    }
}

