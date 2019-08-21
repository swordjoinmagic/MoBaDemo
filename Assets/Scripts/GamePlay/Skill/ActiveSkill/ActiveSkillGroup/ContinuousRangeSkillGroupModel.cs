using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ContinuousRangeSkillGroupModel : BaseSkillModel{
    //======================================================
    // 此技能开放的接口
    private ActiveSkill<BaseSkillModel>[] activeSkills;                  // 技能组所包含的所有主动技能
    private SkillDelayAttribute[] skillDelayAttributes;  // 技能的延迟属性

    public ActiveSkill<BaseSkillModel>[] ActiveSkills {
        get {
            return activeSkills;
        }

        set {
            activeSkills = value;
        }
    }

    public SkillDelayAttribute[] SkillDelayAttributes {
        get {
            return skillDelayAttributes;
        }

        set {
            skillDelayAttributes = value;
        }
    }
}

