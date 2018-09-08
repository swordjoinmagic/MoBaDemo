using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success when an object exits the 2D trigger.")]
    [TaskCategory("Physics")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
    public class HasExitedTrigger2D : Conditional
    {
        [Tooltip("The tag of the GameObject to check for a trigger against")]
        public SharedString tag = "";
        [Tooltip("The object that exited the trigger")]
        public SharedGameObject otherGameObject;

        private bool exitedTrigger = false;

        public override TaskStatus OnUpdate()
        {
            return exitedTrigger ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            exitedTrigger = false;
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(other.gameObject.tag)) {
                otherGameObject.Value = other.gameObject;
                exitedTrigger = true;
            }
        }

        public override void OnReset()
        {
            tag = "";
            otherGameObject = null;
        }
    }
}
