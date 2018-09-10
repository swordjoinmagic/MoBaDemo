using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 进行一次攻击操作的Action节点
/// </summary>
public class AttackAction : Action {

    public SharedGameObject enemry;

    private CharacterMono character;
    private Animator animator;
    public SharedBool isStartAttack;


    public override void OnAwake() {
    }

    public override void OnStart() {
        character = enemry.Value.GetComponent<CharacterMono>();
        animator = GetComponent<Animator>();

    }

    public override TaskStatus OnUpdate() {

        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);
        if (!currentAnimatorStateInfo.IsName("attack") && !isStartAttack.Value) {
            animator.SetTrigger("attack");
            isStartAttack.Value = true;
            Debug.Log("将isStartAttack设置为true");
            return TaskStatus.Success;
        } else {
            return TaskStatus.Failure;
        }
    }
}
