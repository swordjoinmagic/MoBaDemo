using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

public class IdleState : FSMState {

    public Animator animator;

    public override void OnEnter() {
        animator = BlackBorad.Animator;

        // 重置所有状态
        animator.ResetTrigger("spell");
        animator.ResetTrigger("attack");
        animator.SetBool("isRun",false);
    }

    public override void OnExit() {
        
    }

    public override void OnUpdate() {
        
    }
}

