using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

/// <summary>
/// 人物操作状态机中的anyState,
/// 用于一些在通常情况下进行的行为或操作
/// </summary>
public class CharacterOperationExtraState : FSMState {

    public CharacterMono characterMono;
    public CharacterModel characterModel;

    // 是否是第一次进入anyState状态
    public bool isFirstEnter = true;

    public override void OnEnter() {

        // 初始化
        characterMono = BlackBorad.GameObject.GetComponent<CharacterMono>();
        characterModel = characterMono.characterModel;
    }

    public override void OnExit() {

    }

    /// <summary>
    /// 如果人物按下了主动技能中的任意一个,为黑板中设置变量
    /// isPrePareUseSkill为True
    /// </summary>
    public override void OnUpdate() {
        if (isFirstEnter) {
            OnEnter();
            isFirstEnter = false;
        }

        foreach (ActiveSkill skill in characterModel.activeSkills) {
            // 是否按下按键,如果按下,则令prepareSkill=skill
            if (Input.GetKeyDown(skill.KeyCode)) {
                Debug.Log("在AnyState中按下技能的按键!");
                characterMono.prepareSkill = skill;
                Debug.Log("为CharacterMono设置prepareSkill技能");
                characterMono.isPrepareUseSkill = true;
                BlackBorad.SetBool("isPrePareUseSkill",true);
            }
        }

    }
}

