using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;

/// <summary>
/// 用于判断施法是否结束，施法结束后自动回到Idle状态
/// </summary>
public class IsSpellFinishTransition : FSMTransition {
    public override FSMState GetNextState() {
        return NextState;
    }

    public override bool IsValid() {
        bool IsUseSkillFinish = BlackBorad.GetBool("IsUseSkillFinish");

        if (IsUseSkillFinish) {
            BlackBorad.SetBool("IsUseSkillFinish",false);
            return true;
        } else
            return false;
    }

    public override void OnTransition() {
        
    }
}

