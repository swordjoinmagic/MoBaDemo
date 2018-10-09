using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;

/// <summary>
/// 攻击状态,当进入攻击状态时,会自动对敌人进行追击,
/// 在update中判断一次攻击是否完成,当攻击完成时,敌人血量减少
/// </summary>
public class AttackState : FSMState {

    Animator animator;
    NavMeshAgent agent;
    CharacterMono characterMono;
    Transform EnemryTransform;
    CharacterMono enemryMono;
    CharacterModel enemryModel;

    // 当前攻击是否已经完成
    private bool isAttackFinish = true;

    public void Init() {
        characterMono = BlackBorad.GameObject.GetComponent<CharacterMono>();
        EnemryTransform = BlackBorad.GetTransform("EnemryTransform");
        enemryMono = BlackBorad.GetGameObject("Enemry").GetComponent<CharacterMono>();
    }

    public override void OnEnter() {
        Init();
    }

    public override void OnExit() {

    }

    public override void OnUpdate() {
        // 如果当前单位还未移动到目标敌人的位置,进行追击
        if (enemryMono!=null && characterMono.Chasing(EnemryTransform,characterMono.characterModel.attackDistance)) {
            // 追击完成后,对敌方进行攻击
            characterMono.Attack(ref isAttackFinish,EnemryTransform,enemryMono);
        }
    }
}
