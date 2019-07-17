using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
    [TaskCategory("Basic/Time")]
    [TaskDescription("Returns the scale at which time is passing.")]
    public class GetTimeScale : Action
    {
        [Tooltip("The variable to store the result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Time.timeScale;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult.Value = 0;
        }
    }
}