using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[TaskCategory("guard Action")]
public class IsFindEnemry : Conditional{

    private CharacterMono characterMono;

    // 要攻击的敌人
    public SharedGameObject target;

    public override void OnAwake() {
        characterMono = GetComponent<CharacterMono>();
    }

    // 选择一个敌人来进攻
    public CharacterMono ChooseOneEnemry() {
        try {
            return characterMono.arroundEnemies[0];
        } catch (System.Exception e) {
            return null;
        }
    }

    public override TaskStatus OnUpdate() {
        if (characterMono.arroundEnemies.Count > 0) {

            target.Value = ChooseOneEnemry().gameObject;

            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}

