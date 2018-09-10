using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 伤害类,用于一切伤害的计算
/// </summary>
public class Damage {
    int baseDamage;
    int plusDamge;

    public int BaseDamage {
        get {
            return baseDamage;
        }

        set {
            baseDamage = value;
        }
    }

    public int PlusDamge {
        get {
            return plusDamge;
        }

        set {
            plusDamge = value;
        }
    }

    /// <summary>
    /// 用于计算总伤害的get方法
    /// </summary>
    public int TotalDamage {
        get {
            return baseDamage + plusDamge;
        }
    }
}

