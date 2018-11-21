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
        characterMono = BlackBorad.CharacterMono;
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

        // 判断单位是否按下它的主动技能
        foreach (ActiveSkill skill in characterModel.activeSkills) {
            // 是否按下按键,如果按下,则令prepareSkill=skill
            if (skill.SkillLevel != 0 && Input.GetKeyDown(skill.KeyCode) && !skill.IsCoolDown()) {
                characterMono.prepareSkill = skill;

                Debug.Log("为CharacterMono设置prepareSkill技能,技能是:" + characterMono.prepareSkill.SkillName);

                characterMono.isPrepareUseSkill = true;
                BlackBorad.SetBool("isPrePareUseSkill", true);
                return;
            }
        }

        // 判断单位是否按下它的物品技能
        foreach (ItemGrid itemGrid in characterModel.itemGrids) {
            if (itemGrid.item == null) return;
            ActiveSkill activeSkill = itemGrid.item.itemActiveSkill;
            if (Input.GetKeyDown(itemGrid.hotKey) && !activeSkill.IsCoolDown()) {
                Debug.Log("按下了物品特技："+itemGrid.item.name);
                characterMono.prepareSkill = activeSkill;
                characterMono.isPrepareUseSkill = true;
                BlackBorad.SetBool("isPrePareUseSkill", true);

                characterMono.isPrepareUseItemSkill = true;
                characterMono.prepareItemSkillItemGrid = itemGrid;
                return;
            }
        }
    }
}

