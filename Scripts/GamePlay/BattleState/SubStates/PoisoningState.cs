using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 中毒状态
/// </summary>
public class PoisoningState : BattleState{

    //============================================
    // 提供给外部调用的接口
    private Damage damage = Damage.Zero;     // 中毒时每间隔interval秒受到的伤害
    public GameObject effect = null;        // 中毒时的特效
    public float interval = 1f;             // 间隔的时间

    private GameObject effectObject = null;
    private float time = 0;

    public Damage Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    protected override void OnEnter(CharacterMono stateHolder) {
        base.OnEnter(stateHolder);
        time = interval;
        if (effect != null && effectObject == null) {
            // 创建临时特效对象
            effectObject = TransientGameObjectFactory.AcquireObject(EffectConditonalType.BattleState, templateObject: effect, battleState: this, target: stateHolder).gameObject;
            effectObject.transform.position = stateHolder.transform.position;
            effectObject.transform.SetParent(stateHolder.transform);
        }
    }

    protected override void OnUpdate(CharacterMono stateHolder) {
        base.OnUpdate(stateHolder);

        time += Time.smoothDeltaTime;
        if (time >= interval) {
            stateHolder.characterModel.Damaged(damage);
            
            // 置空时间
            time = 0;
        }
    }

    public override BattleState DeepCopy() {
        PoisoningState poisoningState = new PoisoningState() {
            damage = damage,
            effect = effect,
            Name = Name,
            Description = Description,
            IconPath = IconPath,
            Duration = Duration,
            statePassiveSkills = statePassiveSkills,
            stateHolderEffect = stateHolderEffect,
            IsStackable = IsStackable,
        };
        return poisoningState;
    }
}

