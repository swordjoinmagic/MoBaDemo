using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 判断是否进行移动的Transition,如果
/// 玩家点击鼠标左键进行移动,那么IsValid返回True
/// </summary>
public class IsClickedMoveTransition : FSMTransition {
    public override FSMState GetNextState() {
        OnTransition();
        return NextState;
    }

    /// <summary>
    /// 用户按下鼠标右键后,判断是否点击到了敌人,
    /// 没有点击到的情况下,进行移动
    /// </summary>
    /// <returns></returns>
    public override bool IsValid() {
        // 当用户按下鼠标且鼠标没有停留在UI上时，才进行移动，防止鼠标穿透UI
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            Debug.Log("用户按下鼠标");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (!hit.collider.CompareTag("Enermy")) {
                    // 为黑板设置变量
                    BlackBorad.SetVector3("ClickPosition",hit.point);
                    BlackBorad.SetBool("IsMoveOver", false);
                    return true;
                }
            }
        }
        // 如果没有进行移动,对黑板中的ClickPosition变量进行清空操作
        BlackBorad.SetObject("ClickPosition",null);

        return false;
    }
    
    /// <summary>
    /// 处理其他状态向移动状态迁移的函数,在这里
    /// 简单的为移动地点添加一个移动特效
    /// </summary>
    public override void OnTransition() {
        GameObject.Instantiate(BlackBorad.GetGameObject("MoveEffect"),BlackBorad.GetVector3("ClickPosition"),Quaternion.identity);
    }
}

