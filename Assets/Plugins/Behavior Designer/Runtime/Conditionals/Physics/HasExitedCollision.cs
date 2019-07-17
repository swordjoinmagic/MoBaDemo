using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success when a collision ends.")]
    [TaskCategory("Physics")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
    public class HasExitedCollision : Conditional
    {
        [Tooltip("The tag of the GameObject to check for a collision against")]
        public SharedString tag = "";
        [Tooltip("The object that exited the collision")]
        public SharedGameObject collidedGameObject;

        private bool exitedCollision = false;

        public override TaskStatus OnUpdate()
        {
            return exitedCollision ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            exitedCollision = false;
        }

        public override void OnCollisionExit(Collision collision)
        {
            if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(collision.gameObject.tag)) {
                collidedGameObject.Value = collision.gameObject;
                exitedCollision = true;
            }
        }

        public override void OnReset()
        {
            collidedGameObject = null;
        }
    }
}