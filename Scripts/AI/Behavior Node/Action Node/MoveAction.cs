using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : Action {

    public SharedVector3 position;

    private NavMeshAgent agent;
    private Animator animator;

    public override void OnStart() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.SetBool("isRun", true);
        Debug.Log("设置animator为true");
        agent.SetDestination(position.Value);
    }

    public override TaskStatus OnUpdate() {
        Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance == 0) return TaskStatus.Running;
        Debug.Log("state:"+agent.isStopped);
        Debug.Log("remainingDistance:"+agent.remainingDistance);
        Debug.Log("stoppingDistance:"+agent.stoppingDistance);

        if (agent.remainingDistance <= agent.stoppingDistance) {
            Debug.Log("运动就结束，设置animator为false");
            animator.SetBool("isRun",false);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
