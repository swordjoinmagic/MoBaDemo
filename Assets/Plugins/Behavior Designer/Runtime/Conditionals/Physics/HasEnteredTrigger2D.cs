using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success when an object enters the 2D trigger.")]
    [TaskCategory("Physics")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
    public class HasEnteredTrigger2D : Conditional
    {
        [Tooltip("The tag of the GameObject to check for a trigger against")]
        public SharedString tag = "";
        [Tooltip("The object that entered the trigger")]
        public SharedGameObject otherGameObject;

        private bool enteredTrigger = false;

        public override TaskStatus OnUpdate()
        {
            return enteredTrigger ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            enteredTrigger = false;
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(other.gameObject.tag)) {
                otherGameObject.Value = other.gameObject;
                enteredTrigger = true;
            }
        }

        public override void OnReset()
        {
            tag = "";
            otherGameObject = null;
        }
    }
}
