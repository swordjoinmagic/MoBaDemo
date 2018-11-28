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

    //================================
    // 此技能开放的接口
    public int count;       // 闪电链跳转次数
    private Damage damage;   // 闪电链的伤害
    public float attenuationFactor; // 闪电链伤害衰减因子
    public LightningBoltScript lightningBoltScriptPrefab;     // 用于控制闪电链的LineRender对象
    
    // 存储生成的闪电链条，在固定时刻销毁单位
    private List<LightningBoltScript> lightningBolts = new List<LightningBoltScript>();

    public Damage Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    public override void Execute(CharacterMono speller, CharacterMono target) {
        // 结束递归
        if (count == 0) return;

        CreateChainEffect(speller,target);
        target.characterModel.Damaged(Damage);
        // 找到目标单位周围随机的单位
        if (target.arroundFriends.Count > 0) {
            CharacterMono nextTarget = target.arroundFriends[Random.Range(0, target.arroundFriends.Count)];
            count -= 1;
            Execute(target,nextTarget);
        }
    }

    /// <summary>
    /// 在两个单位之间创建链条特效
    /// </summary>
    public void CreateChainEffect(CharacterMono targetA, CharacterMono targetB) {
        LightningBoltScript lightningBolt = GameObject.Instantiate<LightningBoltScript>(lightningBoltScriptPrefab);
        lightningBolt.StartObject = targetA.gameObject;
        lightningBolt.EndObject = targetB.gameObject;
        lightningBolts.Add(lightningBolt);
    }
}

