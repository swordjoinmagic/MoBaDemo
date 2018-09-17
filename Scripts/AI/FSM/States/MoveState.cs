using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FSM;
using UnityEngine.AI;

public class MoveState : FSMState {

    public Vector3 position;
    public Animator animator;
    public NavMeshAgent agent;

    public void Init() {
        position = BlackBorad.GetVector3("ClickPosition");
        animator = BlackBorad.Animator;
        agent = BlackBorad.Agent;
    }

    public override void OnEnter() {

        Init();

        animator.SetBool("isRun",true);
        agent.isStopped = false;
        agent.SetDestination(position);
        // 重置攻击
        animator.ResetTrigger("attack");

    }

    public override void OnExit() {
        
    }

    public override void OnUpdate() {

        //=================================
        // 判断移动是否结束

        if (agent.remainingDistance == 0)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance) {
            animator.SetBool("isRun", false);
        }
    }
}

