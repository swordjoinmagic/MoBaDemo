using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
    [TaskCategory("Basic/Behaviour")]
    [TaskDescription("Enables/Disables the object. Returns Success.")]
    public class SetIsEnabled : Action
    {
        [Tooltip("The Object to use")]
        public SharedObject specifiedObject;
        [Tooltip("The enabled/disabled state")]
        public SharedBool enabled;

        public override TaskStatus OnUpdate()
        {
            if (specifiedObject == null && !(specifiedObject.Value is UnityEngine.Behaviour)) {
                Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
                return TaskStatus.Failure;
            }

            (specifiedObject.Value as Behaviour).enabled = enabled.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            if (specifiedObject != null) {
                specifiedObject.Value = null;
            }
            enabled = false;
        }
    }
}