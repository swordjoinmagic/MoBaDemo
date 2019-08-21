using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RangeDamageSkillModel : BaseSkillModel{
    private Damage damage;

    public Damage Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }
}

