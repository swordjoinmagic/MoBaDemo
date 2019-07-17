using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPhysics
{
    [TaskCategory("Basic/Physics")]
    [TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=117")]
    public class Linecast : Action
    {
        [Tooltip("The starting position of the linecast")]
        SharedVector3 startPosition;
        [Tooltip("The ending position of the linecast")]
        SharedVector3 endPosition;
        [Tooltip("Selectively ignore colliders.")]
        public LayerMask layerMask = -1;

        public override TaskStatus OnUpdate()
        {
            return Physics.Linecast(startPosition.Value, endPosition.Value, layerMask) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
            layerMask = -1;
        }
    }
}