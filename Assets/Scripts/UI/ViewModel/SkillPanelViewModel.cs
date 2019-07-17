using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine.UI;

/// <summary>
/// 用于技能视图以及技能详情视图
/// </summary>
public class SkillPanelViewModel : ViewModelBase{
     
    // 当用户鼠标进入技能面板时触发的函数
    public delegate void EnterSkillPanelView();

    // 技能快捷键
    public BindableProperty<string> hotKey = new BindableProperty<string>();
    // 技能名
    public BindableProperty<string> skillName = new BindableProperty<string>();
    // 技能消耗mp
    public BindableProperty<string> mp = new BindableProperty<string>();
    // 技能图片地址
    public BindableProperty<string> imagePath = new BindableProperty<string>();
    // 技能冷却时间
    public BindableProperty<string> CD = new BindableProperty<string>();
    // 技能长描述
    public BindableProperty<string> longDescription = new BindableProperty<string>();
    // 技能短描述
    public BindableProperty<string> shortDescription = new BindableProperty<string>();
    // 技能施法范围
    public BindableProperty<string> spellDistance = new BindableProperty<string>();
    // 技能伤害,施加状态,作用目标等描述
    public BindableProperty<string> targetDescription = new BindableProperty<string>();
    // 技能等级
    public BindableProperty<string> skillLevel = new BindableProperty<string>();
    // 下一级需要的英雄等级
    public BindableProperty<string> nextLevel = new BindableProperty<string>();


    public void Modify(BaseSkill skill) {

        skillName.Value = skill.SkillName;
        imagePath.Value = skill.IconPath;
        longDescription.Value = skill.LongDescription;
        shortDescription.Value = skill.ShortDescription;
        targetDescription.Value = skill.TargetDescription;
        skillLevel.Value = skill.SkillLevel.ToString();
        nextLevel.Value = skill.NextLevelNeedHeroLevel.ToString();
        if (skill is ActiveSkill) {
            // 主动技能的情况下
            ActiveSkill activeSkill = skill as ActiveSkill;
            hotKey.Value = activeSkill.KeyCode.ToString();
            mp.Value = activeSkill.Mp.ToString();
            CD.Value = activeSkill.CD.ToString();
            spellDistance.Value = activeSkill.SpellDistance.ToString();

        } else {
            // 被动技能的情况下
            PassiveSkill passiveSkill = skill as PassiveSkill;
            hotKey.Value = "";
            mp.Value = "";
            CD.Value = "";
            spellDistance.Value = "";
        }
    }
}

