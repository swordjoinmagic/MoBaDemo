using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Clamps the magnitude of the Vector2.")]
    public class ClampMagnitude : Action
    {
        [Tooltip("The Vector2 to clamp the magnitude of")]
        public SharedVector2 vector2Variable;
        [Tooltip("The max length of the magnitude")]
        public SharedFloat maxLength;
        [Tooltip("The clamp magnitude resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.ClampMagnitude(vector2Variable.Value, maxLength.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = storeResult = Vector2.zero;
            maxLength = 0;
        }
    }
}