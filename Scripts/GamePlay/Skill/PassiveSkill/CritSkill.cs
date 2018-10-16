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
public class CritSkill : PassiveSkill{
    //==============================
    // 可供外界调整的参数(一律以大写字母开头)

    public GameObject Effect;       // 暴击后,敌人身上产生的特效
    public float CritRate;          // 暴击几率(Range(0,1))
    public float CritMultiple;      // 暴击倍数

    public override void Execute(CharacterMono speller,CharacterMono target,ref Damage damage) {
        // 约束暴击几率
        CritRate = Mathf.Clamp01(CritRate);

        if (Random.Range(0f,1f) <= CritRate) {

            Debug.Log("执行被动技能---暴击！");

            // 执行暴击效果,将伤害*倍数对敌人进行计算
            damage *= CritMultiple;

            // 产生特效
            GameObject.Instantiate(Effect, target.transform);
        }
        
    }
}

