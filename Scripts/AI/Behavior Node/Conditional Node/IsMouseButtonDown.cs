using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMouseButtonDown : Conditional {

    /// <summary>
    /// 保存鼠标点击的位置，存储在黑板上
    /// </summary>
    public SharedVector3 clickPosition = new SharedVector3();

    /// <summary>
    /// 判断是否点击敌军的bool变量
    /// </summary>
    public SharedBool isClickedEnermy = false;

    /// <summary>
    /// 用于保存到黑板的敌人变量
    /// </summary>
    public SharedGameObject enemry ;

    public override TaskStatus OnUpdate() {
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("用户按下鼠标");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                clickPosition.Value = hit.point;
                if (hit.collider.CompareTag("Enermy")) {
                    isClickedEnermy.Value = true;
                    enemry.Value = hit.collider.gameObject;
                }
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }
}
