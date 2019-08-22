using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 附加主动技能型的被动技能，
/// 当满足触发条件时，对目标执行主动技能
/// </summary>
public class AdditionalActiveSkill : PassiveSkill{

    public AdditionalActiveSkill(SkillModel skillModel) : base(skillModel){}

    //===========================================
    // 此技能开放的接口

    // 附加的主动技能
    public ActiveSkill AdditionalSkill {
        get {
            return (ActiveSkill)skillModel.ExtraAttributes["AdditionalSkill"];
        }
    }

    public override void Execute(CharacterMono speller, CharacterMono target) {
        base.Execute(speller, target);
        if (AdditionalSkill.IsMustDesignation)
            Execute(speller, target);
        else
            AdditionalSkill.Execute(speller,target.transform.position);
    }
}

