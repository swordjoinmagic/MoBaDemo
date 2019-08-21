using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 暴击技能类型,即有几率暴击,同时暴击后,在目标敌人上产生一定特效
/// 提供以下几个接口供外部调整:
///     1. 产生的特效
///     2. 暴击几率
///     3. 暴击倍数
/// </summary>
public class CritSkill : PassiveSkill<CritSkillModel>{

    public CritSkill(CritSkillModel skillModel) : base(skillModel) { }

    public GameObject Effect {
        get {
            return skillModel.TargetEffect;
        }
    }

    public float CritRate {
        get {
            return skillModel.CritRate;
        }
    }

    public float CritMultiple {
        get {
            return skillModel.CritMultiple;
        }
    }

    public override void Execute(CharacterMono speller,CharacterMono target,ref Damage damage) {
        // 约束暴击几率
        float critRate = Mathf.Clamp01(CritRate);
        EffectsLifeCycle tempTargetEffect = null;
        if (Random.Range(0f,1f) <= critRate) {

            // 执行暴击效果,将伤害*倍数对敌人进行计算
            damage *= CritMultiple;
            
            // 产生特效
            if (skillModel.TargetEffect!=null) {
                tempTargetEffect = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During, templateObject: skillModel.TargetEffect, during: 5f);
                tempTargetEffect.transform.position = target.transform.position;
            }
        }
        
    }
}

