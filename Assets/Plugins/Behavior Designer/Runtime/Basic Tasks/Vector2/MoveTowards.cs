using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Move from the current position to the target position.")]
    public class MoveTowards : Action
    {
        [Tooltip("The current position")]
        public SharedVector2 currentPosition;
        [Tooltip("The target position")]
        public SharedVector2 targetPosition;
        [Tooltip("The movement speed")]
        public SharedFloat speed;
        [Tooltip("The move resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.MoveTowards(currentPosition.Value, targetPosition.Value, speed.Value * Time.deltaTime);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            currentPosition = targetPosition = storeResult = Vector2.zero;
            speed = 0;
        }
    }
}