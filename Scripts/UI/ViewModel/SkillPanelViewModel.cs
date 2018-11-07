using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine.UI;

public class SkillPanelViewModel : ViewModelBase{

    // 当用户鼠标进入技能面板时触发的函数
    public delegate void EnterSkillPanelView();

    public BindableProperty<string> hotKey = new BindableProperty<string>();
    public BindableProperty<string> skillName = new BindableProperty<string>();
    public BindableProperty<string> mp = new BindableProperty<string>();
    public BindableProperty<string> imagePath = new BindableProperty<string>();

    public void Modify(BaseSkill skill) {
        if (skill is ActiveSkill) {
            // 主动技能的情况下
            ActiveSkill activeSkill = skill as ActiveSkill;

            hotKey.Value = activeSkill.KeyCode.ToString();
            skillName.Value = activeSkill.SkillName;
            mp.Value = activeSkill.Mp.ToString();
            imagePath.Value = activeSkill.IconPath;
        } else {
            // 被动技能的情况下
            hotKey.Value = "";
            skillName.Value = skill.SkillName;
            mp.Value = "";
            imagePath.Value = skill.IconPath;
        }
    }
}

