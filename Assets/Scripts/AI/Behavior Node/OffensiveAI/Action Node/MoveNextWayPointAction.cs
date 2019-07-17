using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Behavior_Node.OffensiveAI.Action_Node {
    [TaskCategory("OffensiveAI")]
    class MoveNextWayPointAction : Action{

        CharacterMono characterMono;
        public SharedVector3 moveTargetPosition;

        public override void OnAwake() {
            characterMono = GetComponent<CharacterMono>();
            if(characterMono.wayPointsUnit!=null)
                moveTargetPosition.Value = characterMono.wayPointsUnit.GetNextWayPoint();
        }

        public override TaskStatus OnUpdate() {
            if (characterMono.wayPointsUnit == null) return TaskStatus.Failure;
            moveTargetPosition.Value = characterMono.wayPointsUnit.GetNowWayPoint();
            if (!characterMono.Move(moveTargetPosition.Value)) {
                // 如果移动结束，那么下一次就移动到下一个路径点
                moveTargetPosition = characterMono.wayPointsUnit.GetNextWayPoint();
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
