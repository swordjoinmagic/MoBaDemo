using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Behavior_Node.Guard.Action_Node {
    [TaskCategory("guard Action")]
    class ReturningFromOriginalRoad : Action{
        public SharedVector3 initalPosition;

        private CharacterMono characterMono;
         
        public override void OnAwake() {
            characterMono = GetComponent<CharacterMono>();
        }

        public override TaskStatus OnUpdate() {
            if (!characterMono.Move(initalPosition.Value)) {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
    }
}
