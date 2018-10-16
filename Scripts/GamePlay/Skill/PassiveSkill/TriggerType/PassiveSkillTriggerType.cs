using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum PassiveSkillTriggerType {
    GainAttribute = 0,      // 属性增益型,属于那种触发条件为True的被动技能
    WhenAttack,             // 攻击时,会进行触发
    WhenBeAttacked,         // 被攻击时,会进行触发
    WhenNormalAttack,       // 当进行普通攻击时,会进行触发
    WhenSpellAttack,        // 当进行施法攻击时,会进行触发
}

