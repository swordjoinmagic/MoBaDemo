using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[TaskCategory("guard Action")]
public class IsFindEnemry : Conditional{

    private CharacterMono characterMono;

    // 要攻击的敌人
    public SharedGameObject target;

    public SharedBool isFindEnermy;


    public override void OnAwake() {
        characterMono = GetComponent<CharacterMono>();
    }

    // 选择一个敌人来进攻
    public CharacterMono ChooseOneEnemry() {
        try {
            while (characterMono.arroundEnemies[0] == null) characterMono.arroundEnemies.RemoveAt(0);
            return characterMono.arroundEnemies[0];
        } catch (System.Exception e) {
            return null;
        }
    }

    public override TaskStatus OnUpdate() {

        //Debug.Log("IsFindEnemry ConditonalNode Running");

        if (characterMono.arroundEnemies.Count > 0) {

            CharacterMono enemry = ChooseOneEnemry();

            if (enemry == null) return TaskStatus.Failure;

            target.Value = enemry.gameObject;
            isFindEnermy.Value = true;
            
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}

