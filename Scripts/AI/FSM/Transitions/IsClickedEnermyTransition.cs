using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

/// <summary>
/// 用于判断用户是否点击敌人的Transition,
/// 如果对敌人进行了点击,设置敌人对象和它的Transform为
/// 公共变量,进入攻击状态
/// </summary>
public class IsClickedEnermyTransition : FSMTransition {
    public override FSMState GetNextState() {
        OnTransition();
        return NextState;
    }

    public override bool IsValid() {
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("用户按下鼠标对敌人进行攻击");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {

                if (hit.collider.CompareTag("Enemry")) {
                    // 为黑板设置变量
                    BlackBorad.SetTransform("EnemryTransform", hit.collider.transform);
                    BlackBorad.SetComponent("Enemry",hit.collider.gameObject.GetComponent<CharacterMono>());
                    return true;
                }

            }
        }
        // 如果没有进行攻击,对黑板中的EnemryTransform和Enemry变量进行清空操作
        //BlackBorad.SetObject("EnemryTransform", null);
        //BlackBorad.SetObject("Enemry",null);


        return false;
    }

    public override void OnTransition() {
        
    }
}

