using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

public class IdleState : FSMState {

    public override void OnEnter() {
        // 重置所有状态
        BlackBorad.GameObject.GetComponent<CharacterMono>().ResetIdle();
        
    }

    public override void OnExit() {
        
    }

    public override void OnUpdate() {
        
    }
}

