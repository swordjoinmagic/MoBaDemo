
using UnityEngine;

/// <summary>
/// 光环技能(基类),
/// 一种特殊的被动技能,在学习此技能时,会自动在单位身上增加一个光环触发器,
/// 光环触发器记录了每个进入和离开此光环范围的单位,同时给这些单位附加了光环状态.
/// </summary>
public class HaloSkill : PassiveSkill{

    public float inflenceRadius;
    public UnitFaction targetFaction;
    public GameObject HaloEffect;

    // 给单位附加的状态
    private BattleState additiveState;

    public override PassiveSkillTriggerType TiggerType {
        get {
            return PassiveSkillTriggerType.Halo;
        }
    }

    /// <summary>
    /// 学习光环技能时,触发的方法
    /// </summary>
    /// <param name="speller"></param>
    public override void Execute(CharacterMono speller) {
        //base.Execute(speller);

        //Debug.Log("Enter");

        // 给单位增加光环触发器
        GameObject haloTrigger = new GameObject(SkillName+"HaloTrigger");
        haloTrigger.transform.SetParent(speller.transform);
        haloTrigger.transform.localPosition = Vector3.zero;
        HaloTrigger Trigger = haloTrigger.AddComponent<HaloTrigger>();
        SphereCollider sphereCollider = haloTrigger.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        Trigger.TiggerRadius = inflenceRadius;
        Trigger.UnitType = targetFaction;
        sphereCollider.radius = inflenceRadius;
        Trigger.HaloSkillExecute += Execute;
        Trigger.HaloSkillCancelExecute += CancelExecute;
        Trigger.gameObject.layer = 2;

        additiveState = new PoisoningState {
            damage = new Damage(30, 0),
            Description = "中毒光环,每秒-30生命值,增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述增长描述",
            Duration = -1,
            IconPath = "0046",
            Name = "中毒光环",
            IsStackable = false,
            effect = HaloEffect
        };
        //Debug.Log("Finish");
    }

    /// <summary>
    /// 当单位进入光环时,触发的方法
    /// </summary>
    /// <param name="speller"></param>
    /// <param name="target"></param>
    public override void Execute(CharacterMono speller, CharacterMono target) {
        //base.Execute(speller, target);

        Debug.Log("Enter  Execute(CharacterMono speller, CharacterMono target)");

        additiveState.isFirstEnterState = true;
        // 给目标附加一个持续时间为永久的中毒状态
        target.AddBattleState(additiveState);

    }
    public void CancelExecute(CharacterMono speller, CharacterMono target) {
        target.RemoveBattleState(additiveState);
    }
}

