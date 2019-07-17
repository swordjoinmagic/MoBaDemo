using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class IsMoveOver : Conditional {

    private NavMeshAgent agent;
    private Animator animator;

    public SharedBool isStartAttack;
    public SharedBool isClieckEnermy;

    // 用于判断是否移动的变量
    public SharedBool isStartMove;

    private float stoppingDistance;
    private CharacterMono characterMono;

    public SharedBool isStartMoveAttack;
    public SharedBool isStartSpellMove;

    public override void OnStart() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterMono = GetComponent<CharacterMono>();
        if (isStartMoveAttack.Value)
            stoppingDistance = characterMono.characterModel.attackDistance;
        else if (isStartSpellMove.Value)
            stoppingDistance = characterMono.prepareSkill.SpellDistance;
        else
            stoppingDistance = agent.stoppingDistance;
    }

    public override TaskStatus OnUpdate() {

        //Debug.Log("agent.remainingDistance:"+ agent.remainingDistance);

        if (agent.remainingDistance == 0)
            return TaskStatus.Failure;
        if (agent.remainingDistance <= stoppingDistance) {
            //Debug.Log("运动就结束，设置animator为false");

            animator.SetBool("isRun", false);
            isStartMove.Value = false;
            return TaskStatus.Success;
        } else {
            // 重置攻击
            animator.ResetTrigger("attack");
            isStartAttack.Value = false;
            animator.SetBool("isRun", true);
        }
        return TaskStatus.Failure;
    }
}
