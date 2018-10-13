using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

class AttackToIdleTransition : FSMTransition {
    public override FSMState GetNextState() {
        return NextState;
    } 

    public override bool IsValid() {
        GameObject target = BlackBorad.GetGameObject("Enemry");

        //Debug.Log("AttackToIdleTransition:"+target.name);

        // 如果单位已被摧毁
        if (target == null) return true;

        CharacterMono targetMono = target.GetComponent<CharacterMono>();

        if (targetMono.isDying || targetMono==null || !targetMono.IsCanBeAttack()) return true;

        return false;
    }

    public override void OnTransition() {
        
    }
}

