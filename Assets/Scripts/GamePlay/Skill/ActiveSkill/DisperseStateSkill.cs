using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 简单驱散状态技能，驱散目标单位身上某一状态
/// </summary>
public class DisperseStateSkill : ActiveSkill<DisperseStateSkillModel>{

    public DisperseStateSkill(DisperseStateSkillModel skillModel) : base(skillModel) {

    }

    public override bool IsMustDesignation {
        get {
            return true;
        }
    }

    public BattleStateType BattleStateType {
        get {
            return skillModel.BattleStateType;
        }
    }

    public override void Execute(CharacterMono speller, CharacterMono target) {
        base.Execute(speller, target);
        target.RemoveBattleState(BattleStateType);
    }
}

