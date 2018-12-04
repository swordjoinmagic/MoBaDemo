
using UnityEngine;

/// <summary>
/// 光环技能(基类),
/// 一种特殊的被动技能,在学习此技能时,会自动在单位身上增加一个光环触发器,
/// 光环触发器记录了每个进入和离开此光环范围的单位,同时给这些单位附加了光环状态.
/// </summary>
public class HaloSkill : PassiveSkill{

    // 光环触发器
    public HaloTrigger Trigger;

    public float inflenceRadius;
    public UnitFaction targetFaction;
    public GameObject HaloEffect;

    // 给单位附加的状态
    public BattleState additiveState;

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
        // 给单位增加光环触发器

        //==========================================
        // 新建光环触发器组件所依附的GameObject对象
        GameObject haloTrigger = new GameObject(SkillName+"HaloTrigger");
        haloTrigger.transform.SetParent(speller.transform);
        haloTrigger.transform.localPosition = Vector3.zero;

        //===============================================
        // 初始化光环触发器
        Trigger = haloTrigger.AddComponent<HaloTrigger>();

        // 设置触发器组件
        SphereCollider sphereCollider = haloTrigger.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = inflenceRadius;

        // 设置光环触发器的一些属性
        Trigger.TiggerRadius = inflenceRadius;
        Trigger.UnitType = targetFaction;
        Trigger.HaloSkillExecute += Execute;
        Trigger.HaloSkillCancelExecute += CancelExecute;
        Trigger.gameObject.layer = 2;
    }

    /// <summary>
    /// 遗忘光环技能时,触发的方法
    /// </summary>
    /// <param name="speller"></param>
    public void CancelExecute(CharacterMono speller) {
        //=========================================
        // 取消对Trigger事件的监听
        Trigger.HaloSkillExecute -= Execute;
        Trigger.HaloSkillCancelExecute -= CancelExecute;

        //=================================
        // 摧毁光环触发器
        GameObject.Destroy(Trigger.gameObject);
    }

    /// <summary>
    /// 当单位进入光环时,触发的方法
    /// </summary>
    /// <param name="speller"></param>
    /// <param name="target"></param>
    public override void Execute(CharacterMono speller, CharacterMono target) {
        // 给目标附加一个持续时间为永久的中毒状态
        target.AddBattleState(additiveState.DeepCopy());

    }
    public void CancelExecute(CharacterMono speller, CharacterMono target) {
        target.RemoveBattleState(additiveState.Name);
    }
}

