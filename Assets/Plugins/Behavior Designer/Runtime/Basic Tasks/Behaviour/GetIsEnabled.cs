using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
    [TaskCategory("Basic/Behaviour")]
    [TaskDescription("Stores the enabled state of the object. Returns Success.")]
    public class GetIsEnabled : Action
    {
        [Tooltip("The Object to use")]
        public SharedObject specifiedObject;
        [Tooltip("The enabled/disabled state")]
        [RequiredField]
        public SharedBool storeValue;

        public override TaskStatus OnUpdate()
        {
            if (specifiedObject == null && !(specifiedObject.Value is UnityEngine.Behaviour)) {
                Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
                return TaskStatus.Failure;
            }

            storeValue.Value = (specifiedObject.Value as Behaviour).enabled;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            if (specifiedObject != null) {
                specifiedObject.Value = null;
            }
            storeValue = false;
        }
    }
}