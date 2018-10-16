using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//=================================
// ACTIVESKILL TYPE : 指向伤害技能类

/// <summary>
/// 指向伤害技能类,此技能要求单击一个目标进行释放,对目标直接造成伤害
/// 有以下特性:
///     3.selfEffect:释放技能时自身的特效/动画
///     4.targetEffect:释放技能时敌人的特效/动画
/// </summary>
public class PointingSkill : ActiveSkill{

    //==============================================
    // 攻击时己方和地方的特效，预制体，定义技能时输入
    public GameObject selfEffect;
    public GameObject targetEffect;

    public override void Execute(CharacterMono spller, CharacterMono target) {

        FinalSpellTime = Time.time;

        GameObject tempSelfEffect = null;
        GameObject tempTargetEffect = null;
        if (selfEffect!=null)
            tempSelfEffect = GameObject.Instantiate(selfEffect, spller.transform);
        if(targetEffect!=null)
            tempTargetEffect = GameObject.Instantiate(targetEffect, target.transform);

        target.characterModel.Damaged(new Damage() { BaseDamage=BaseDamage,PlusDamage=PlusDamage });
    }
}

