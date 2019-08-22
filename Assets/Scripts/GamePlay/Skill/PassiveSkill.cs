using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 被动技能基类
/// </summary>
public class PassiveSkill : BaseSkill{
    public float FinalSpellTime {
        get {
            return finalSpellTime;
        }

        set {
            finalSpellTime = value;
        }
    }

    public PassiveSkill(SkillModel skillModel) : base(skillModel){}

    public float CD {
        get {
            return skillModel.Cooldown;
        }
    }
    /// <summary>
    /// 判断当前技能是否处于冷却中
    /// </summary>
    /// <returns></returns>
    public bool IsCoolDown() {
        return Time.time - FinalSpellTime <= CD;
    }

    // 被动技能触发类型(在脚本中指定)
    private PassiveSkillTriggerType triggerType;
    public virtual PassiveSkillTriggerType TiggerType {
        get {
            return triggerType;
        }
        set {
            triggerType = value;
        }
    }

    #region 用于单位攻击/被攻击/升级/死亡时执行的被动技能,基类技能方法签名为Execute(CharacterMono speller, CharacterMono target,ref Damage damage)
    /// <summary>
    /// 执行该被动技能效果
    /// 具体的效果由子类进行覆写,此方法主要是用于非增益型被动技能，
    /// 一般用于直接对敌人造成伤害
    /// </summary>
    /// <param name="speller"></param>
    /// <param name="target"></param>
    public virtual void Execute(CharacterMono speller, CharacterMono target) {

    }
    public virtual void Execute(CharacterMono speller) {
        Execute(speller,null);
    }
    /// <summary>
    /// 执行该被动技能效果,此方法一般用于对传输进来的伤害进行一个倍数或加减操作
    /// </summary>
    /// <param name="speller"></param>
    /// <param name="result"></param>
    public virtual void Execute(CharacterMono speller, CharacterMono target,ref Damage damage) { }
    #endregion

    #region 用于增加属性/光环的被动技能
    /// <summary>
    /// 执行该被动技能效果,并将计算得到的附加值输出至result变量(注意是附加值)
    /// 此被动技能主要是用于属性增益型被动技能
    /// </summary>
    /// <param name="speller">被动技能施法者</param>
    /// <param name="result">输出此增益型技能增加的值</param>
    /// <param name="characterAttribute">表示此被动技能增加的是什么属性,用来在外部执行时，可以获得此被动技能关联的属性</param>
    /// <param name="attributeValue">该属性原先的值</param>
    public virtual void Execute(CharacterModel speller,out int result,out CharacterAttribute characterAttribute) {
        result = 0;
        characterAttribute = CharacterAttribute.Attack;
    }
    public virtual void Execute(CharacterModel speller, out float result, out CharacterAttribute characterAttribute) {
        result = 0f;
        characterAttribute = CharacterAttribute.Attack;
    }
    #endregion
}