using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsMoveOver : Conditional {

    private NavMeshAgent agent;
    private Animator animator;

    public override void OnStart() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate() {
        if (agent.remainingDistance == 0) return TaskStatus.Failure;
        if (agent.remainingDistance <= agent.stoppingDistance) {
            Debug.Log("运动就结束，设置animator为false");

            animator.SetBool("isRun", false);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
