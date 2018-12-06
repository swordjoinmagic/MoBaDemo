using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 判断攻击操作是否完成
/// </summary>
class IsAttackFinisth : Conditional{

    private CharacterMono character;
    private Animator animator;

    public SharedBool isStartAttack;
    public SharedGameObject enemry;


    public override void OnAwake() {
        animator = GetComponent<Animator>();
    }

    public override void OnStart() {
        character = enemry.Value.GetComponent<CharacterMono>();
    }

    public override TaskStatus OnUpdate() {
        CharacterModel characterModel = character.characterModel;

        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        //Debug.Log("此时的isStartAttack状态:"+isStartAttack.Value);

        //if (currentAnimatorStateInfo.IsName("attack")) {
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(enemry.Value.transform.position, transform.up), Time.deltaTime * 8);
        //}

        if (currentAnimatorStateInfo.IsName("attack") && 
            nextAnimatorStateInfo.IsName("Idle") &&
            isStartAttack.Value) {
            characterModel.Hp -= 50;
            //character.SimpleCharacterViewModel.Modify(characterModel);

            //Debug.Log("攻击完成,设置isStartAttack为False,并进行减血操作");
            isStartAttack.Value = false;
            


            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}

