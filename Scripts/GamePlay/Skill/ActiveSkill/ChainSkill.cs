using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 链型技能
/// 在几个单位之间跳转的，带有lineRender技能特效的主动技能
/// 典型代表：闪电链、治疗波
/// </summary>
class ChainSkill : ActiveSkill{

    public override bool IsMustDesignation {
        get {
            return true;
        }
    }

    //================================
    // 此技能开放的接口
    private int count;       // 闪电链跳转次数
    private Damage damage;   // 闪电链的伤害
    public float attenuationFactor; // 闪电链伤害衰减因子
    
    public Damage Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    public int Count {
        get {
            return count;
        }

        set {
            count = value;
            nowCount = value;
        }
    }

    private int nowCount;
    
    public override void Execute(CharacterMono speller, CharacterMono target) {
        if (nowCount >= 1)
            CreateChainEffect(speller, target);
        target.characterModel.Damaged(Damage);
    }

    /// <summary>
    /// 在两个单位之间创建链条特效
    /// </summary>
    public void CreateChainEffect(CharacterMono speller, CharacterMono target) {
        EffectsLifeCycle lifeCycle = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During,during:1f,templateObject:TargetEffect);
        lifeCycle.transform.position = target.transform.position;
        lifeCycle.OnFinshied += ()=> {
            CharacterMono nextTarget = null;
            // 找到目标单位周围随机的单位
            Collider[] colliders = Physics.OverlapSphere(target.transform.position, SkillInfluenceRadius);
            foreach (var collider in colliders) {
                CharacterMono characterMono = collider.GetComponent<CharacterMono>();
                if (characterMono != null && characterMono != target) {
                    nextTarget = characterMono;
                    break;
                }
            }
            if (nextTarget != null && nowCount >= 1) {
                Execute(speller, nextTarget);
                nowCount -= 1;
            } else {
                // 复原
                nowCount = Count;
            }
        };
    }
}

