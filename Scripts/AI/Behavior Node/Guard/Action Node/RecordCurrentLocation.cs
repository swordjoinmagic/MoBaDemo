using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AI.Behavior_Node.Guard.Action_Node {
    [TaskCategory("guard Action")]
    public class RecordCurrentLocation : Action{
        public SharedVector3 initalPosition;
        public override void OnStart() {
            initalPosition.Value = transform.position;
        }
    }
}
