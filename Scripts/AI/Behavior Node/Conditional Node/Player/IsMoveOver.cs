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

    public override void OnStart() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate() {

        //Debug.Log("agent.remainingDistance:"+ agent.remainingDistance);

        if (agent.remainingDistance == 0)
            return TaskStatus.Failure;
        if (agent.remainingDistance <= agent.stoppingDistance) {
            //Debug.Log("运动就结束，设置animator为false");

            animator.SetBool("isRun", false);
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
