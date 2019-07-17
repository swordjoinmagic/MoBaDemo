using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPhysics2D
{
    [TaskCategory("Basic/Physics2D")]
    [TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=118")]
    public class Linecast : Action
    {
        [Tooltip("The starting position of the linecast.")]
        SharedVector2 startPosition;
        [Tooltip("The ending position of the linecast.")]
        SharedVector2 endPosition;
        [Tooltip("Selectively ignore colliders.")]
        public LayerMask layerMask = -1;

        public override TaskStatus OnUpdate()
        {
            return Physics2D.Linecast(startPosition.Value, endPosition.Value, layerMask) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            layerMask = -1;
        }
    }
}
