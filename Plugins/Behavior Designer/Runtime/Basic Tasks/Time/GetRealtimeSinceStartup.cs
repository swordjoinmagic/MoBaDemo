using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
    [TaskCategory("Basic/Time")]
    [TaskDescription("Returns the real time in seconds since the game started.")]
    public class GetRealtimeSinceStartup : Action
    {
        [Tooltip("The variable to store the result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Time.realtimeSinceStartup;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult.Value = 0;
        }
    }
}