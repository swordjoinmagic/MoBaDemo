using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine.UI;

public class SkillPanelViewModel : ViewModelBase{
    public BindableProperty<string> hotKey = new BindableProperty<string>();
    public BindableProperty<string> skillName = new BindableProperty<string>();
    public BindableProperty<string> mp = new BindableProperty<string>();
    public BindableProperty<string> imagePath = new BindableProperty<string>();

    public void Modify(BaseSkill skill) {
        if (skill is ActiveSkill) {
            // 主动技能的情况下
            ActiveSkill activeSkill = skill as ActiveSkill;

            hotKey.Value = activeSkill.KeyCode.ToString();
            skillName.Value = activeSkill.skillName;
            mp.Value = activeSkill.Mp.ToString();
            imagePath.Value = activeSkill.iconPath;
        } else {
            // 被动技能的情况下
            hotKey.Value = "";
            skillName.Value = skill.skillName;
            mp.Value = "";
            imagePath.Value = skill.iconPath;
        }
    }
}

