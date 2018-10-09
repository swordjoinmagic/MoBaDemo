using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AI.Behavior_Node.Guard.Conditional_Node {
    [TaskCategory("guard Action")]
    class IsChasingEnemryFinish : Conditional{

        // 要攻击的敌人
        public SharedGameObject target;
        private CharacterMono targetCharacterMono;

        CharacterMono characterMono;

        public override void OnAwake() {
            characterMono = GetComponent<CharacterMono>();
        }

        public override void OnStart() {
            targetCharacterMono = target.Value.GetComponent<CharacterMono>();
        }

        public override TaskStatus OnUpdate() {
            if (characterMono.Chasing(targetCharacterMono.transform)) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
