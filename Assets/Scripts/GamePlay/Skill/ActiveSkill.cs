using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public delegate void OnSkillCompeleteHandler();
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
public class ActiveSkill<T> : BaseSkill<T> where T:BaseSkillModel {

    public ActiveSkill(T skillModel) : base(skillModel) { }

    #region 技能基类属性
    public int Mp {
        get {
            return skillModel.Mp;
        }
    }
    public KeyCode KeyCode {
        get {
            return skillModel.KeyCode;
        }
    }
    public float SpellDistance {
        get {
            return skillModel.SpellDistance;
        }
    }
    public float CD {
        get {
            return skillModel.Cooldown;
        }
    }
    public float CDRate {
        get {
            return (Time.time - finalSpellTime) / CD;
        }
    }
    public float SkillInfluenceRadius {
        get {
            return skillModel.SkillInfluenceRadius;
        }
    }
    public GameObject SelfEffect {
        get {
            return skillModel.SelfEffect;
        }
    }
    public GameObject TargetEffect {
        get {
            return skillModel.TargetEffect;
        }
    }
    public float SpellDuration {
        get {
            return skillModel.SpellDuration;
        }
    }
    public float CanBeInterrupt {
        get {
            return skillModel.CanBeInterrupt;
        }
    }
    #endregion

    // 判断此技能的释放是否必须指定敌人对象
    public virtual bool IsMustDesignation { get { return true; } }

    // 当技能释放完毕后，执行的回调函数
    public event OnSkillCompeleteHandler OnCompelte;

    /// <summary>
    /// 为技能产生一般性特效，即释放时，
    /// 自身的特效 selfEffect;
    /// 敌方的特效 targetEffect;
    /// </summary>
    protected void CreateEffect(CharacterMono speller, Vector3 position,CharacterMono target=null) {
        EffectsLifeCycle tempSelfEffect = null;
        EffectsLifeCycle tempTargetEffect = null;
        if (SelfEffect != null) {
            tempSelfEffect = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During, templateObject: SelfEffect, during: 5f);
            tempSelfEffect.transform.position = position;
        }
        if (TargetEffect != null) {
            tempTargetEffect = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During, templateObject: TargetEffect, during: 5f);
            tempTargetEffect.transform.position = position;
        }

        // 如果有技能要延迟执行，默认根据目标特效对象的生命周期进行延迟执行
        if (OnCompelte != null) {
            tempTargetEffect.OnFinshied += OnCompelte;
            // 清除委托
            OnCompelte -= OnCompelte;
        }
    }

    /// <summary>
    /// 判断当前技能是否处于冷却中
    /// </summary>
    /// <returns></returns>
    public bool IsCoolDown() {
        return Time.time - finalSpellTime <= CD;
    }

    /// <summary>
    /// 持续施法技能释放,非持续施法技能不会重写这个方法,
    /// 施法结束(施法成功/施法失败都算施法结束)返回True
    /// 施法未完成返回False
    /// </summary>
    /// <param name="speller"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public virtual bool ContinuousExecute(CharacterMono speller, CharacterMono target) {
        finalSpellTime = Time.time;
        CreateEffect(speller, target.transform.position);        
        return true;
    }
    public virtual bool ContinuousExecute(CharacterMono speller, Vector3 position) {
        finalSpellTime = Time.time;
        CreateEffect(speller, position);
        return true;
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

        #region 耦合,待重构
        speller.characterModel.Mp -= this.Mp;
        #endregion

        finalSpellTime = Time.time;
        CreateEffect(speller, target.transform.position);
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

        #region 耦合,待重构
        speller.characterModel.Mp -= this.Mp;
        #endregion

        finalSpellTime = Time.time;
        CreateEffect(speller,position);
    }
}

