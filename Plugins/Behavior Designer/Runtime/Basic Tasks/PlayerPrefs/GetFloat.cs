using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
    public class GetFloat : Action
    {
        [Tooltip("The key to store")]
        public SharedString key;
        [Tooltip("The default value")]
        public SharedFloat defaultValue;
        [Tooltip("The value retrieved from the PlayerPrefs")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = PlayerPrefs.GetFloat(key.Value, defaultValue.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            key = "";
            defaultValue = 0;
            storeResult = 0;
        }
    }
}