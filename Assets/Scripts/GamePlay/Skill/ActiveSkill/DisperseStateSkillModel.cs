using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DisperseStateSkillModel : BaseSkillModel{
    private BattleStateType battleStateType;    // 要驱散的状态的类型

    public BattleStateType BattleStateType {
        get {
            return battleStateType;
        }

        set {
            battleStateType = value;
        }
    }
}

