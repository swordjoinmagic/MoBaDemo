using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Deletes all entries from the PlayerPrefs.")]
    public class DeleteAll : Action
    {
        public override TaskStatus OnUpdate()
        {
            PlayerPrefs.DeleteAll();

            return TaskStatus.Success;
        }
    }
}