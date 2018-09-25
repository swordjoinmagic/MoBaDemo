using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTipsMessageView : UnityGuiView<SkillTipsMessageViewModel>{

    //=========================
    // 此View管理的UI控件
    public Text skillNameText;
    public Text mpText;
    public Text descriptionText;
    public Text LevelText;

    protected override void OnInitialize() {
        base.OnInitialize();

        // 对UI控件绑定监听变化的函数
        binder.Add<string>("skillName",OnSkillNameChanged);
        binder.Add<string>("skillLevel",OnSkillLevelChanged);
        binder.Add<string>("needMp",OnNeedMpChanged);
        binder.Add<string>("description",OnDescriptinoChanged);
    }

    // 技能名称
    private void OnSkillNameChanged(string oldSkillName,string newSkillName) {
        skillNameText.text = newSkillName;
    }

    // 消耗mp
    private void OnNeedMpChanged(string oldMp,string newMp) {
        mpText.text = "消耗MP："+newMp;
    }

    // 技能等级
    private void OnSkillLevelChanged(string oldSkillLevel,string newSkillLevel) {
        LevelText.text = "Level  "+newSkillLevel;
    }

    // 技能描述
    private void OnDescriptinoChanged(string oldDescription,string newDescription) {
        descriptionText.text = "技能描述：\n" + newDescription;
    }
}

