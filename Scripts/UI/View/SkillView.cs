using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using uMVVM;

/// <summary>
/// 用于管理一个连续多个技能的技能面板
/// </summary>
class SkillView : UnityGuiView<SkillViewModel>{
    public CharacterMono characterMono;
    private CharacterModel character;
    private List<ActiveSkill> activeSkills;

    public List<SkillPanelView> skillPanelViews;

    //===============================
    // UI控件
    public List<Image> images;

    private void Start() {
        for (int i=0;i<skillPanelViews.Count;i++) {

            // 对每一个技能面板进行初始化
            SkillPanelView skillPanelView = skillPanelViews[i];
            BaseSkill baseSkill = characterMono.characterModel.activeSkills[i];
            skillPanelView.BindingContext = new SkillPanelViewModel();
            skillPanelView.BindingContext.Modify(baseSkill);
        }
    }

    private void Update() {

        character = characterMono.characterModel;
        activeSkills = character.activeSkills;

        for (int i=0;i<activeSkills.Count;i++) {

            ActiveSkill activeSkill = activeSkills[i];

            float coolDown = activeSkill.CD;
            float finalSpellTime = activeSkill.FinalSpellTime;

            float different = Time.time - finalSpellTime;

            float rate = 1 - Mathf.Clamp01(different/coolDown);
            images[i].fillAmount = rate;
        }
    }
}

