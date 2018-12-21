using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class HPView : UnityGuiView<HPViewModel>{

    //====================================
    // 此View管理的UI控件
    public Text hpText;
    public Text mpText;
    public RectTransform hpImage;
    public RectTransform mpImage;
    public int hpImageMaxWidth;
    public int mpImageMaxWidth;
    public int hpImageHeight;
    public int mpImageHeight;

    public HeroMono characterMono;

    public void Init(HeroMono characterMono) {
        this.characterMono = characterMono;
        characterMono.HeroModel.HpValueChangedHandler += OnHpChanged;
        characterMono.HeroModel.MpValueChangedHandler += OnMpValueChanged;
    }

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("Hp", OnHpChanged);
    }

    public void OnHpChanged(int oldValue,int newValue) {
        hpText.text = string.Format("{0}/{1}",newValue,BindingContext.maxHp.Value);

        //// 计算当前血量和新血量的差值
        float width = ((float)newValue / (float)BindingContext.maxHp.Value) * hpImageMaxWidth;
        hpImage.sizeDelta = new Vector2(width, hpImage.sizeDelta.y);
    }

    public void OnMpValueChanged(int oldValue,int newValue) {
        mpText.text = string.Format("{0}/{1}",newValue,BindingContext.maxMp.Value);

        // 计算当前Mp和新MP的差值
        float width = ((float)newValue / (float)BindingContext.maxMp.Value) * mpImageMaxWidth;
        mpImage.sizeDelta = new Vector2(width, mpImage.sizeDelta.y);
    }
}

