using UnityEngine;
using System;
using System.Reflection;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Sets the property to the value specified. Returns success if the property was set.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=150")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class SetPropertyValue : Action
    {
        [Tooltip("The GameObject to set the property on")]
        public SharedGameObject targetGameObject;
        [Tooltip("The component to set the property on")]
        public SharedString componentName;
        [Tooltip("The name of the property")]
        public SharedString propertyName;
        [Tooltip("The value to set")]
        public SharedVariable propertyValue;

        public override TaskStatus OnUpdate()
        {
            if (propertyValue == null) {
                Debug.LogWarning("Unable to get field - field value is null");
                return TaskStatus.Failure;
            }
            
            var type = TaskUtility.GetTypeWithinAssembly(componentName.Value);
            if (type == null) {
                Debug.LogWarning("Unable to set property - type is null");
                return TaskStatus.Failure;
            }

            var component = GetDefaultGameObject(targetGameObject.Value).GetComponent(type);
            if (component == null) {
                Debug.LogWarning("Unable to set the property with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            // If you are receiving a compiler error on the Windows Store platform see this topic:
            // http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=46 
            var property = component.GetType().GetProperty(propertyName.Value);
            property.SetValue(component, propertyValue.GetValue(), null);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            componentName = null;
            propertyName = null;
            propertyValue = null;
        }
    }
}