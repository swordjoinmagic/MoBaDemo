using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Behavior_Node.OffensiveAI.Action_Node {
    [TaskCategory("OffensiveAI")]
    class ChooseATowerToAttackAction : Action{
        public SharedGameObject target;
        public SharedGameObjectList targetList;

        public override TaskStatus OnUpdate() {
            foreach (GameObject enemry in targetList.Value) {
                if (enemry != null) {
                    target.Value = enemry;
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
    }
}
