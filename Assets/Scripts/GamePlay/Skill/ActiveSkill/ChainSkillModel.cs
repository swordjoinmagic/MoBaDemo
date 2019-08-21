using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChainSkillModel : BaseSkillModel{
    //================================
    // 此技能开放的接口
    private int count;       // 闪电链跳转次数
    private Damage damage;   // 闪电链的伤害
    private float attenuationFactor; // 闪电链伤害衰减因子

    public Damage Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    public float AttenuationFactor {
        get {
            return attenuationFactor;
        }

        set {
            attenuationFactor = value;
        }
    }

    public int Count {
        get {
            return count;
        }
        set {
            count = value;
        }
    }
}

