using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the quaternion of a forward vector.")]
    public class LookRotation : Action
    {
        [Tooltip("The forward vector")]
        public SharedVector3 forwardVector;
        [Tooltip("The second Vector3")]
        public SharedVector3 secondVector3;
        [Tooltip("The stored quaternion")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Quaternion.LookRotation(forwardVector.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            forwardVector = Vector3.zero;
            storeResult = Quaternion.identity;
        }
    }
}