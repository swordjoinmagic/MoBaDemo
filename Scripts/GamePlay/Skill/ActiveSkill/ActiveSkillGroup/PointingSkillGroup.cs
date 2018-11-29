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

    public override void Execute(CharacterMono speller, CharacterMono target) {
        base.Execute(speller, target);
        // 遍历所有技能组,对target目标释放效果
        foreach (var activeSkill in activeSkills) {
            activeSkill.Execute(speller,target);
        }
    }
}

