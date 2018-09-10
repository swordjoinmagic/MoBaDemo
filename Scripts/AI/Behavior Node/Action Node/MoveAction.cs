using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : Action {

    public SharedVector3 position;
    public SharedBool isStartAttack;

    private NavMeshAgent agent;
    private Animator animator;

    public override void OnStart() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate() {
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        animator.SetBool("isRun", true);
        Debug.Log("设置animator为true");

        agent.SetDestination(position.Value);
        return TaskStatus.Success;
    }
}
