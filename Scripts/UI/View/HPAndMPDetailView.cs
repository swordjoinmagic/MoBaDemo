using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

/// <summary>
/// Hp条和Mp条的详细UI视图
/// </summary>
public class HPAndMPDetailView : MonoBehaviour{

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

    public void Refresh() {
        OnHpChanged(characterMono.HeroModel.Hp, characterMono.HeroModel.Hp);
        OnMpValueChanged(characterMono.HeroModel.Mp, characterMono.HeroModel.Mp);
    }

    public void OnHpChanged(int oldValue,int newValue) {
        hpText.text = string.Format("{0}/{1}",newValue,characterMono.HeroModel.maxHp);

        //// 计算当前血量和新血量的差值
        float width = ((float)newValue / (float)characterMono.HeroModel.maxHp) * hpImageMaxWidth;
        hpImage.sizeDelta = new Vector2(width, hpImage.sizeDelta.y);
    }

    public void OnMpValueChanged(int oldValue,int newValue) {
        mpText.text = string.Format("{0}/{1}",newValue, characterMono.HeroModel.maxMp);

        // 计算当前Mp和新MP的差值
        float width = ((float)newValue / (float)characterMono.HeroModel.maxMp) * mpImageMaxWidth;
        mpImage.sizeDelta = new Vector2(width, mpImage.sizeDelta.y);
    }
}

