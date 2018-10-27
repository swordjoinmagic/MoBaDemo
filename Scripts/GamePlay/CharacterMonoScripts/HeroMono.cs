using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 英雄单位的Mono类
/// </summary>
class HeroMono : CharacterMono{
    public HPViewModel HPViewModel;

    public HeroModel HeroModel {
        get {
            return characterModel as HeroModel;
        }
    }

    public override bool Attack(ref bool isAttackFinish, Transform targetTransform, CharacterMono target) {
        bool result = base.Attack(ref isAttackFinish, targetTransform, target);
        if (target.isDying || target == null) {
            HeroModel.exp += target.characterModel.supportExp;
        }
        return result;
    }

    public override void Update() {
        base.Update();
    }

    protected override void Bind() {
        base.Bind();
        characterModel.HpValueChangedHandler += OnHpChanged;
    }
    public void OnHpChanged(int oldHp, int newHp) {
        HPViewModel.Hp.Value = newHp;
    }
}

