using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction11 : Action {
    public override TaskStatus OnUpdate() {
        Debug.Log("执行节点TestAction1");
        return TaskStatus.Success;
    }
}
