using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// State型生命周期,当状态消失时,特效对象消失
/// </summary>
public class StateConditional : EffectConditional {

    // 被附加状态的单位
    private CharacterMono target;

    // 被附加的状态
    private BattleState battleState;

    private bool isValid = true;

    public StateConditional(CharacterMono target, BattleState battleState) {
        this.battleState = battleState;
        this.target = target;

        this.target.OnRemoveBattleStatus += RemoveState;
    }

    private void RemoveState(BattleState battleState) {
        if (battleState == this.battleState) {
            isValid = false;
            target.OnRemoveBattleStatus -= RemoveState;
        }
    }

    public override bool IsValid() {
        return isValid;
    }
}

