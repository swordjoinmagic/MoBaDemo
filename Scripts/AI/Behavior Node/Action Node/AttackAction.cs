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

    // 是否开始攻击
    private bool isStartAttack = false;


    public override void OnAwake() {
        animator = GetComponent<Animator>();
    }

    public override void OnStart() {
        character = enemry.Value.GetComponent<CharacterMono>();

        isStartAttack = false;
        animator.SetTrigger("attack");
    }

    public override TaskStatus OnUpdate() {

        CharacterModel characterModel = character.characterModel;

        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("attack")) {
            isStartAttack = true;
        }

        if (nextAnimatorStateInfo.IsName("Idle") && isStartAttack) {
            characterModel.Hp -= 50;
            character.SimpleCharacterViewModel.Modify(characterModel);

            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
