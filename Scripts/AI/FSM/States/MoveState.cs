using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FSM;
using UnityEngine.AI;

public class MoveState : FSMState {

    public Vector3 position;
    public CharacterMono characterMono;

    public void Init() {
        position = BlackBorad.GetVector3("ClickPosition");
        characterMono = BlackBorad.CharacterMono;
    }

    public override void OnEnter() {
        Init();
    }

    public override void OnExit() {
        
    }

    public override void OnUpdate() {
        // 单位进行移动
        if (!characterMono.Move(position)) {
            // 移动结束设置黑板中的IsMoveOver变量为True
            BlackBorad.SetBool("IsMoveOver",true);
        } else {
            BlackBorad.SetBool("IsMoveOver", false);
        }
    }
}

