using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Behavior_Node.Guard.Action_Node {
    [TaskCategory("OffensiveAI")]
    class SetNearestPositionAction : Action{

        CharacterMono characterMono;
        public SharedBool isFindEnermy;
        public override void OnAwake() {
            characterMono = GetComponent<CharacterMono>();
        } 
        public override void OnStart() {
            if (isFindEnermy.Value) {
                characterMono.wayPointsUnit.GetNearestWayPoint(transform.position);
                isFindEnermy.Value = false;
            }
        }
    }
}
