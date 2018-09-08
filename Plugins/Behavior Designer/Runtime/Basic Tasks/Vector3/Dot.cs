using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Stores the dot product of two Vector3 values.")]
    public class Dot : Action
    {
        [Tooltip("The left hand side of the dot product")]
        public SharedVector3 leftHandSide;
        [Tooltip("The right hand side of the dot product")]
        public SharedVector3 rightHandSide;
        [Tooltip("The dot product result")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector3.Dot(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = rightHandSide = Vector3.zero;
            storeResult = 0;
        }
    }
}