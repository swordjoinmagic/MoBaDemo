using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CritSkillModel : BaseSkillModel{
    //==============================
    // 可供外界调整的参数
    private float critRate;          // 暴击几率(Range(0,1))
    private float critMultiple;      // 暴击倍数

    public float CritRate {
        get {
            return critRate;
        }

        set {
            critRate = value;
        }
    }

    public float CritMultiple {
        get {
            return critMultiple;
        }

        set {
            critMultiple = value;
        }
    }
}

