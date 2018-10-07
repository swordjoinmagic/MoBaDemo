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

    // 设置当前单位是否开始攻击
    private bool isStartAttack = false;
    // 当前攻击是否已经完成
    private bool isAttackFinish = true;

    public void Init() {
        animator = BlackBorad.Animator;
        agent = BlackBorad.Agent;
        characterMono = BlackBorad.GameObject.GetComponent<CharacterMono>();
        EnemryTransform = BlackBorad.GetTransform("EnemryTransform");
        enemryMono = BlackBorad.GetGameObject("Enemry").GetComponent<CharacterMono>();
        enemryModel = enemryMono.characterModel;
        Debug.Log("攻击状态中,敌人是:"+enemryMono);
        agent.SetDestination(EnemryTransform.position);
    }

    public override void OnEnter() {
        Init();
    }

    public override void OnExit() {

    }

    public override void OnUpdate() {
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);


        //======================================
        // 追击部分
        // 当移动到小于攻击距离时，自动停止移动,
        // 否则继续移动,直到追上敌人,或者敌人消失在视野中
        if (characterMono.Chasing(EnemryTransform)) {
            // 追上敌人,准备开始攻击
            isStartAttack = true;
        } else {
            // 还未追上敌人之前,不进行攻击
            isStartAttack = false;
        }

        //======================================
        // 播放攻击动画
        // 如果准备开始攻击,那么播放动画
        if (isStartAttack && !currentAnimatorStateInfo.IsName("attack")) {
            animator.SetTrigger("attack");
            isAttackFinish = false;
        }


        //======================================
        // 伤害判断
        if (currentAnimatorStateInfo.IsName("attack") &&
            nextAnimatorStateInfo.IsName("Idle") &&
            !isAttackFinish) {

            if (characterMono.characterModel.projectileModel == null) {
                enemryModel.Hp -= 50;
                isAttackFinish = true;
            } else {
                Transform shotPosition = BlackBorad.GameObject.transform.Find("shootPosition");
                ProjectileMono projectileMono = GameObject.Instantiate(characterMono.characterModel.projectile, shotPosition.position, Quaternion.identity);
                projectileMono.targetPosition = EnemryTransform.position;
                projectileMono.target = enemryMono;
                projectileMono.damage = new Damage() { BaseDamage = 100};
                projectileMono.projectileModel = characterMono.characterModel.projectileModel;
                isAttackFinish = true;
            }
        }

    }
}
