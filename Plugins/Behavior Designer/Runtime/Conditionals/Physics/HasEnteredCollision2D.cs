using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success when a 2D collision starts.")]
    [TaskCategory("Physics")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
    public class HasEnteredCollision2D : Conditional
    {
        [Tooltip("The tag of the GameObject to check for a collision against")]
        public SharedString tag = "";
        [Tooltip("The object that started the collision")]
        public SharedGameObject collidedGameObject;

        private bool enteredCollision = false;

        public override TaskStatus OnUpdate()
        {
            return enteredCollision ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            enteredCollision = false;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(collision.gameObject.tag)) {
                collidedGameObject.Value = collision.gameObject;
                enteredCollision = true;
            }
        }

        public override void OnReset()
        {
            tag = "";
            collidedGameObject = null;
        }
    }
}
