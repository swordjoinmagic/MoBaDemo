using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the dot product of two Vector2 values.")]
    public class Dot : Action
    {
        [Tooltip("The left hand side of the dot product")]
        public SharedVector2 leftHandSide;
        [Tooltip("The right hand side of the dot product")]
        public SharedVector2 rightHandSide;
        [Tooltip("The dot product result")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.Dot(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = rightHandSide = Vector2.zero;
            storeResult = 0;
        }
    }
}