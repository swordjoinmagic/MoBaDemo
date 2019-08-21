using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PointingSkillModel : BaseSkillModel{
    private Damage baseDamge;

    public Damage BaseDamge {
        get {
            return baseDamge;
        }

        set {
            baseDamge = value;
        }
    }
}

