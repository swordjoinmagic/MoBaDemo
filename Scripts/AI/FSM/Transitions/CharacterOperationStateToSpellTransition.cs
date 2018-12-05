using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

public class CharacterOperationStateToSpellTransition : FSMTransition {

    public CharacterMono characterMono;
    public CharacterModel characterModel;

    public override FSMState GetNextState() {
        return NextState;
    }

    public override bool IsValid() {
        characterMono = BlackBorad.CharacterMono;
        if (!BlackBorad.GetBool("isPrePareUseSkill") || characterMono.prepareSkill==null) return false;

        characterModel = characterMono.characterModel;


        // 获得当前要释放的技能
        // 判断该技能是否是原地释放技能,
        // 即判断该主动技能的施法距离是否为0,为0时,
        // 为原地释放技能
        if (characterMono.IsImmediatelySpell()) {
            // 原地释放技能,直接进入Spell状态
            BlackBorad.SetVector3("EnemryPosition", characterMono.transform.position);
            BlackBorad.SetBool("isPrePareUseSkill", false);

            return true;

        } else {
            // 指向型技能
            BlackBorad.SetBool("isImmediatelySpell", false);

            Debug.Log("当前准备施放的是指向型技能");

            // 当为指向型技能时,更改主角的鼠标Icon,
            // 判断主角是否点击敌人,当点击敌人时,进入Spell状态
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    if (characterMono.prepareSkill.IsMustDesignation && hit.collider.CompareTag("Enermy")) {
                        // 为黑板设置变量
                        BlackBorad.SetTransform("EnemryTransform", hit.collider.transform);
                        BlackBorad.SetComponent("Enemry", hit.collider.gameObject.GetComponent<CharacterMono>());
                        return true;
                    } else if(!characterMono.prepareSkill.IsMustDesignation) {
                        // 为黑板设置变量
                        BlackBorad.SetVector3("EnemryPosition", hit.point);
                        return true;
                    }
                }
            }

            // 如果此时玩家按下ESC键,结束这个准备施法的Transition
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
                BlackBorad.SetBool("isPrePareUseSkill", false);
                characterMono.isPrepareUseSkill = false;
                return false;
            }
        }

        return false;
        
    }

    public override void OnTransition() {
        
    }
}

