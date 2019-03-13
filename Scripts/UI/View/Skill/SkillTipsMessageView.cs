using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using uMVVM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTipsMessageView : UnityGuiView<SkillPanelViewModel> {

    //=========================
    // 此View管理的UI控件
    public Text skillNameText;
    public Text mpText;
    public Text longDescriptionText;
    public Text levelText;
    public Text targetDescriptionText;
    public Text shortDescriptionText;
    public Text spellDistanceText;
    public Text duringTimeText;
    public Text CDText;
    public Text nextLevelText;

    protected override void OnInitialize() {
        base.OnInitialize();

        // 对UI控件绑定监听变化的函数
        binder.Add<string>("skillName", OnSkillNameChanged);
        binder.Add<string>("skillLevel", OnSkillLevelChanged);
        binder.Add<string>("mp", OnNeedMpChanged);
        binder.Add<string>("longDescription", OnDescriptinoChanged);
        binder.Add<string>("shortDescription",OnShortDescriptionChanged);
        binder.Add<string>("spellDistance",OnSpellDistanceChanged);
        binder.Add<string>("targetDescription",OnTargetDescriptionChanged);
        binder.Add<string>("nextLevel",OnNextLevelChanged);
    }

    // 技能名称
    private void OnSkillNameChanged(string oldSkillName, string newSkillName) {
        skillNameText.text = newSkillName;
    }

    // 消耗mp
    private void OnNeedMpChanged(string oldMp, string newMp) {
        mpText.text = newMp;
    }

    // 技能等级
    private void OnSkillLevelChanged(string oldSkillLevel, string newSkillLevel) {
        levelText.text = "Lv" + newSkillLevel;
    }

    // 技能描述
    private void OnDescriptinoChanged(string oldDescription, string newDescription) {
        longDescriptionText.text = "技能描述：\n" + newDescription;
    }

    // 短技能描述
    private void OnShortDescriptionChanged(string oldDescription,string newDescription) {
        shortDescriptionText.text = newDescription;
    }

    // 技能伤害,施加状态,作用目标等描述
    private void OnTargetDescriptionChanged(string oldDescription, string newDescription) {
        targetDescriptionText.text = newDescription;
    }

    // 技能施法范围描述
    private void OnSpellDistanceChanged(string oldDistance,string newDistance) {
        spellDistanceText.text = "施法范围："+newDistance;
    }

    // 下一等级描述
    private void OnNextLevelChanged(string oldLevel,string newLevel) {
        nextLevelText.text = "下一次升级在英雄<color=#ffA555>" + newLevel + "</color>级时"; 
    }
}

