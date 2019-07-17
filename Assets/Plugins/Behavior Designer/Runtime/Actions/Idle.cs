

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=112")]
    [TaskIcon("{SkinColor}IdleIcon.png")]
    public class Idle : Action
    {
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}