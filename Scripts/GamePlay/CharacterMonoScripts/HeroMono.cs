using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 英雄单位的Mono类
/// </summary>
public class HeroMono : CharacterMono{
    public HPViewModel HPViewModel;
    public AvatarViewModel avatarViewModel;

    public HeroModel HeroModel {
        get {
            return characterModel as HeroModel;
        }
    }

    public override bool Attack(ref bool isAttackFinish, Transform targetTransform, CharacterMono target) {
        bool result = base.Attack(ref isAttackFinish, targetTransform, target);
        if (target.isDying || target == null) {
            HeroModel.Exp += target.characterModel.supportExp;
        }
        return result;
    }

    public override void Update() {
        base.Update();
    }

    protected override void Bind() {
        base.Bind();
        characterModel.HpValueChangedHandler += OnHpChanged;
        HeroModel.ExpChangedHandler += OnExpChanged;
    }
    public void OnHpChanged(int oldHp, int newHp) {
        HPViewModel.Hp.Value = newHp;
    }
    public void OnExpChanged(int oldExp,int newExp) {
        int expRate = Mathf.Clamp(Mathf.FloorToInt(((float)newExp / HeroModel.NextLevelNeedExp) * 100),0,100);
        Debug.Log(" newExp:"+newExp+"NextLevelExp:"+ HeroModel.NextLevelNeedExp+" ExpRate:"+expRate);
        avatarViewModel.ExpRate.Value = expRate;
    }
}

