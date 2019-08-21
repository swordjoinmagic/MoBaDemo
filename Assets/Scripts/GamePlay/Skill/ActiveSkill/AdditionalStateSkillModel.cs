using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AdditionalStateSkillModel : BaseSkillModel{
    private BattleState additionalState;

    public BattleState AdditionalState {
        get {
            return additionalState;
        }

        set {
            additionalState = value;
        }
    }
}

