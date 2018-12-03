using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 技能组,包含一系列主动技能的主动技能,
/// 当此技能释放时,会执行该技能组所有技能的Execute(speller,target)方法
/// 该类表示对单个目标释放的点目标技能
/// </summary>
public class PointingSkillGroup : ActiveSkill{

    public override bool IsMustDesignation {
        get {
            return true;
        }
    }

    //===================================
    // 此技能开放的接口
    public ActiveSkill[] activeSkills;  // 技能组所包含的所有主动技能
    public SkillDelayAttribute[] skillDelayAttributes;  // 技能的延迟属性

    public override void Execute(CharacterMono speller, CharacterMono target) {
        base.Execute(speller, target);

        //========================================
        // 为延迟技能增加监听事件
        for (int i=0;i<activeSkills.Count();i++) {
            var skill = activeSkills[i];
            var delayAttribute = skillDelayAttributes[i];
            if (delayAttribute.isDelay) {
                var activeSkill = activeSkills[delayAttribute.index];

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
        for (int i = 0; i < activeSkills.Count(); i++) {
            var skill = activeSkills[i];
            var delayAttribute = skillDelayAttributes[i];
            if (!delayAttribute.isDelay) {
                if (skill.IsMustDesignation)
                    skill.Execute(speller, target);
                else
                    skill.Execute(speller, target.transform.position);
            }
        }
    }
}

