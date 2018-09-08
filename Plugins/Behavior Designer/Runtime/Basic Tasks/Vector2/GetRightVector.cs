using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the right vector value.")]
    public class GetRightVector : Action
    {
        [Tooltip("The stored result")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.right;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = Vector2.zero;
        }
    }
}