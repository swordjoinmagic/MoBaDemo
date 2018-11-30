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
    private BattleState additionalState;

    public override string TargetDescription {
        get {
            string s = "";
            s += "技能目标:点目标\n";
            s += "技能伤害:"+(BaseDamage+PlusDamage)+"\n";
            s += "附加状态：中毒\n";
            s += "持续时间：10s";
            return s;
        }
    }

    public BattleState AdditionalState {
        get {
            return additionalState;
        }

        set {
            additionalState = value;
        }
    }

    public override void Execute(CharacterMono spller, CharacterMono target) {

        base.Execute(spller,target);

        if (additionalState!=null) {
            target.AddBattleState(additionalState);
        }

        target.characterModel.Damaged(new Damage() { BaseDamage=BaseDamage,PlusDamage=PlusDamage });
    }
}

