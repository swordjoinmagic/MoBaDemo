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

        Debug.Log("判断是否要释放技能中~~");
        characterModel = characterMono.characterModel;


        // 获得当前要释放的技能
        Debug.Log("当前要释放的技能时:"+ characterMono.prepareSkill);

        // 判断该技能是否是原地释放技能,
        // 即判断该主动技能的施法范围是否为0,为0时,
        // 为原地释放技能
        if (characterMono.IsImmediatelySpell()) {

            // 原地释放技能,直接进入Spell状态
            BlackBorad.SetBool("isImmediatelySpell", true);
            return true;

        } else {
            // 指向型技能
            BlackBorad.SetBool("isImmediatelySpell", false);

            Debug.Log("当前准备施放的是指向型技能");

            // 当为指向型技能时,更改主角的鼠标Icon,
            // 判断主角是否点击敌人,当点击敌人时,进入Spell状态
            if (Input.GetMouseButtonDown(1)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.CompareTag("Enermy")) {
                        // 为黑板设置变量
                        BlackBorad.SetTransform("EnemryTransform", hit.collider.transform);
                        BlackBorad.SetGameObject("Enemry", hit.collider.gameObject);
                        return true;
                    }
                }
            }

            // 如果此时玩家按下ESC键,结束这个准备施法的Transition
            if (Input.GetKeyDown(KeyCode.Escape)) {
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

