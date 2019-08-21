using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PointingSkillGroupModel : BaseSkillModel{
    //===================================
    // 此技能开放的接口
    public ActiveSkill<BaseSkillModel>[] activeSkills;  // 技能组所包含的所有主动技能
    public SkillDelayAttribute[] skillDelayAttributes;  // 技能的延迟属性

}

