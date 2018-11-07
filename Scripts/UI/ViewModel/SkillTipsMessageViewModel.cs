using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;

public class SkillTipsMessageViewModel : ViewModelBase{
    public BindableProperty<string> skillName = new BindableProperty<string>();
    public BindableProperty<string> skillLevel = new BindableProperty<string>();
    public BindableProperty<string> needMp = new BindableProperty<string>();
    public BindableProperty<string> description = new BindableProperty<string>();

    public void Modify(BaseSkill baseSkill) {
        skillName.Value = baseSkill.SkillName;
        skillLevel.Value = baseSkill.SkillLevel.ToString() ;
        description.Value = baseSkill.Description;
        if (baseSkill is ActiveSkill)
            needMp.Value = ((ActiveSkill)baseSkill).Mp.ToString();
        else
            needMp.Value = 0.ToString();
    }
}

