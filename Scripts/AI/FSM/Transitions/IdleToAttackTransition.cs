using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;


class IdleToAttackTransition : FSMTransition {
    public override FSMState GetNextState() {
        return NextState;
    }


    // 选择一个敌人来进攻
    private CharacterMono ChooseOneEnemry() {
        try {
            while (BlackBorad.CharacterMono.arroundEnemies[0] == null || !BlackBorad.CharacterMono.arroundEnemies[0].IsCanBeAttack()) BlackBorad.CharacterMono.arroundEnemies.RemoveAt(0);
            return BlackBorad.CharacterMono.arroundEnemies[0];
        } catch (System.Exception e) {
            return null;
        }
    }


    public override bool IsValid() {
        Debug.Log("正处于IdleToAttackTransition中,周围敌人的数量是:" + BlackBorad.CharacterMono.arroundEnemies.Count);

        if (BlackBorad.CharacterMono.arroundEnemies.Count > 0) {
            CharacterMono target = ChooseOneEnemry();
            if (target == null) return false;
            BlackBorad.SetTransform("EnemryTransform", target.transform);
            BlackBorad.SetGameObject("Enemry", target.gameObject);
            return true;
        }
        return false;
    }

    public override void OnTransition() {
        
    }
}

