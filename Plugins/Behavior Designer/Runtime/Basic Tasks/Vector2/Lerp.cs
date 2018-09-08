using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Lerp the Vector2 by an amount.")]
    public class Lerp : Action
    {
        [Tooltip("The from value")]
        public SharedVector2 fromVector2;
        [Tooltip("The to value")]
        public SharedVector2 toVector2;
        [Tooltip("The amount to lerp")]
        public SharedFloat lerpAmount;
        [Tooltip("The lerp resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.Lerp(fromVector2.Value, toVector2.Value, lerpAmount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromVector2 = toVector2 = storeResult = Vector2.zero;
            lerpAmount = 0;
        }
    }
}