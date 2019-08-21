using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 简单附加状态技能，为目标单位附加一个状态
/// </summary>
public class AdditionalStateSkill : ActiveSkill<AdditionalStateSkillModel>{

    public AdditionalStateSkill(AdditionalStateSkillModel skillModel) : base(skillModel){ }

    public BattleState AdditionalState {
        get {
            return skillModel.AdditionalState;
        }
    }

    public override bool IsMustDesignation {
        get {
            return true;
        }
    }

    public override void Execute(CharacterMono speller, CharacterMono target) {
        base.Execute(speller, target);

        target.AddBattleState(AdditionalState.DeepCopy());
    }
}

