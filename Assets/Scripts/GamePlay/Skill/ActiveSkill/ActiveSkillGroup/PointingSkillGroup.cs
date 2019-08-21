using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 技能组,包含一系列主动技能的主动技能,
/// 当此技能释放时,会执行该技能组所有技能的Execute(speller,target)方法
/// 该类表示对单个目标释放的点目标技能
/// </summary>
public class PointingSkillGroup : ActiveSkill<PointingSkillGroupModel>{

    public override bool IsMustDesignation {
        get {
            return true;
        }
    }

    public PointingSkillGroup(PointingSkillGroupModel skillGroupModel):base(skillGroupModel) {}

    public ActiveSkill<BaseSkillModel>[] ActiveSkills {
        get {
            return skillModel.activeSkills;
        }
    }
    public SkillDelayAttribute[] SkillDelayAttributes {
        get {
            return skillModel.skillDelayAttributes;
        }
    }

    public override void Execute(CharacterMono speller, CharacterMono target) {
        base.Execute(speller, target);

        //========================================
        // 为延迟技能增加监听事件
        for (int i=0;i<ActiveSkills.Count();i++) {
            var skill = ActiveSkills[i];
            var delayAttribute = SkillDelayAttributes[i];
            if (delayAttribute.isDelay) {
                var activeSkill = ActiveSkills[delayAttribute.index];

                OnSkillCompeleteHandler delayExcute = null;
                delayExcute = () => {
                    if(skill.IsMustDesignation)
                        skill.Execute(speller,target);
                    else
                        skill.Execute(speller,target.transform.position);
                };
                activeSkill.OnCompelte += delayExcute;
            }
        }

        //=============================================
        // 执行每一个非延迟技能
        for (int i = 0; i < ActiveSkills.Count(); i++) {
            var skill = ActiveSkills[i];
            var delayAttribute = SkillDelayAttributes[i];
            if (!delayAttribute.isDelay) {
                if (skill.IsMustDesignation)
                    skill.Execute(speller, target);
                else
                    skill.Execute(speller, target.transform.position);
            }
        }
    }
}

