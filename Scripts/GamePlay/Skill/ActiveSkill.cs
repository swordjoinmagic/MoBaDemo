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
/// 同时,该技能类有一个通用的用来计算伤害的方法,
/// 该方法将会产生一个Damage类,用于给Character类计算伤害
/// </summary>
//[System.Serializable]
public class ActiveSkill : BaseSkill{
    public int mp;
    protected int baseDamage;
    protected int plusDamage;
    protected KeyCode keyCode;

    // 施法距离，等于0时是原地释放技能
    protected float spellDistance;

    // 技能CD时间
    protected float cooldown;

    // 最后一次释放技能的时间
    protected float finalSpellTime;

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

    public float FinalSpellTime {
        get {
            return finalSpellTime;
        }

        set {
            finalSpellTime = value;
        }
    }

    /// <summary>
    /// 判断当前技能是否处于冷却中
    /// </summary>
    /// <returns></returns>
    public bool IsCoolDown() {
        Debug.Log("Time.time - finalSpellTime"+(Time.time - finalSpellTime).ToString()+"  CD:"+CD);
        return Time.time - finalSpellTime <= CD;
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
    }
}

