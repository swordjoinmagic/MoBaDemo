using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 所有主动技能类的基类,包含了主动技能的基本特性:
///     1.消耗的mp
///     2.造成的基本伤害
///     3.附加伤害
///     4.释放键位
///     5.允许释放目标
/// 同时,该技能类有一个通用的用来计算伤害的方法,
/// 该方法将会产生一个Damage类,用于给Character类计算伤害
/// </summary>
public class ActiveSkill : BaseSkill{
    public int mp;
    protected int baseDamage;
    protected int plusDamage;
    protected KeyCode keyCode;

    // 施法距离，等于0时是原地释放技能
    protected float spellDistance;

    // 技能影响范围，是一个以r为半径的圆
    protected float skillInfluenceRadius;

    // 技能CD时间
    protected float cooldown;

    // 最后一次释放技能的时间
    protected float finalSpellTime;

    // 此技能允许释放的目标，是一个多重枚举
    protected UnitType skillTargetType;

    public int Mp {
        get {
            return mp;
        }

        set {
            mp = value;
        }
    }

    public int BaseDamage {
        get {
            return baseDamage;
        }

        set {
            baseDamage = value;
        }
    }

    public int PlusDamage {
        get {
            return plusDamage;
        }

        set {
            plusDamage = value;
        }
    }

    public KeyCode KeyCode {
        get {
            return keyCode;
        }

        set {
            keyCode = value;
        }
    }

    public float SpellDistance {
        get {
            return spellDistance;
        }

        set {
            spellDistance = value;
        }
    }

    public float CD {
        get {
            return cooldown;
        }

        set {
            cooldown = value;
        }
    }

    public float CDRate {
        get {
            return (Time.time - finalSpellTime)/CD;
        }
    }

    public float SkillInfluenceRadius {
        get {
            return skillInfluenceRadius;
        }

        set {
            skillInfluenceRadius = value;
        }
    }

    // 判断此技能的释放是否必须指定敌人对象，true为必须指定
    public virtual bool IsMustDesignation {
        get {
            return true;
        }
    }

    public UnitType SkillTargetType {
        get {
            return skillTargetType;
        }

        set {
            skillTargetType = value;
        }
    }

    /// <summary>
    /// 判断此技能的指向目标是否包含某个目标（即targetType）,
    /// 举个例子，此技能的指向目标是 Enermy | Firend | Tree,此时询问此技能指向目标是否包含Enermy，
    /// 返回True
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public bool ContainsTarget(UnitType targetType) {
        return (SkillTargetType & targetType) == targetType;
    }


    /// <summary>
    /// 判断当前技能是否处于冷却中
    /// </summary>
    /// <returns></returns>
    public bool IsCoolDown() {
        return Time.time - finalSpellTime <= CD;
    }

    /// <summary>
    /// 判断某个单位可以被准备释放的技能攻击
    /// </summary>
    /// <returns></returns>
    protected bool CanBeExecuteToTarget(CharacterMono speller,CharacterMono target) {
        // 1. 施法者和目标阵营不一样，且目标阵营不属于中立阵营，则目标属于敌人
        if (!speller.CompareOwner(target)) {
            if (ContainsTarget(UnitType.Enermy))
                return true;
        } else {
            // 2. 施法者和目标阵营一样，则目标属于朋友单位
            if (ContainsTarget(UnitType.Friend)) {
                return true;
            }
        }

        // 3. 目标是英雄单位
        if (target is HeroMono) {
            if (ContainsTarget(UnitType.Hero)) {
                return true;
            }
        }

        // 4. 目标是建筑物
        if ((target.characterModel.unitType & UnitType.Building) == UnitType.Building)
            if (ContainsTarget(UnitType.Building))
                return true;

        // 5. 目标是守卫
        if ((target.characterModel.unitType & UnitType.Guard) == UnitType.Guard) {
            if (ContainsTarget(UnitType.Guard))
                return true;
        }

        // default:
        return false;
    }

    /// <summary>
    /// 用来计算伤害的虚方法,返回一个伤害类
    /// </summary>
    /// <returns></returns>
    public override Damage CalculateDamage() {
        Damage damage = new Damage { BaseDamage = baseDamage, PlusDamage = plusDamage };
        return damage;
    }

    /// <summary>
    /// Execute方法表示应用此技能的特效,
    /// 即 施法开始-产生特效-造成伤害这一系列操作.
    /// 更准确的说,这个virtual方法使得技能类几乎可以实现所有效果,
    /// 只要后面的子技能类重写就OK了.
    /// </summary>
    /// <param name="speller">施法者</param>
    /// <param name="target">受到法术伤害的目标</param>
    public virtual void Execute(CharacterMono speller,CharacterMono target) {
        Execute(speller,target.transform.position);
    }

    /// <summary>
    /// Execute方法表示应用此技能的特效,
    /// 即 施法开始-产生特效-造成伤害这一系列操作.
    /// 更准确的说,这个virtual方法使得技能类几乎可以实现所有效果,
    /// 只要后面的子技能类重写就OK了.
    /// </summary>
    /// <param name="speller">施法者</param>
    /// <param name="target">受到法术伤害的目标</param>
    public virtual void Execute(CharacterMono speller,Vector3 position) {
        finalSpellTime = Time.time;
    }
}

