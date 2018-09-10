using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 判断用户是否正在按下技能键准备释放技能
/// </summary>
public class IsPrepareUseSkills : Conditional{

    private CharacterMono characterMono;

    private CharacterModel character;

    public override void OnStart() {
        characterMono = GetComponent<CharacterMono>();
    }

    public override TaskStatus OnUpdate() {
        character = characterMono.characterModel;
        foreach (ActiveSkill skill in character.activeSkills) {
            // 是否按下按键,如果按下,则令prepareSkill=skill
            if (Input.GetKeyDown(skill.KeyCode)) {
                Debug.Log("按下技能的按键!");
                characterMono.prepareSkill = skill;
                return TaskStatus.Success;
            } else {
                characterMono.prepareSkill = null;
            }
        }
        return TaskStatus.Failure;
    }
}

