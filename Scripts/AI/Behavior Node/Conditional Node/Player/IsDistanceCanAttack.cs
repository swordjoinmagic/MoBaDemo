using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判断目前攻击距离下是否可以攻击
/// </summary>
public class IsDistanceCanAttack : Conditional {

    public SharedGameObject enemry;

    // 攻击距离
    public float attackDistance = 1f;

    public override TaskStatus OnUpdate() {
        if (enemry.Value == null) return TaskStatus.Failure;
        Vector3 position = transform.position;
        if (Vector3.Distance(position,enemry.Value.transform.position) <=attackDistance) {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
