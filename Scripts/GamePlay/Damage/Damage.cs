using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 伤害类,用于一切伤害的计算,
/// 值类型
/// </summary>
public struct Damage {
    float baseDamage;
    float plusDamage;

    public Damage(float baseDamage,float plusDamage ) {
        this.baseDamage = baseDamage;
        this.plusDamage = plusDamage;
    }

    public int BaseDamage {
        get {
            return Mathf.FloorToInt(baseDamage);
        }

        set {
            baseDamage = value;
        }
    }

    public int PlusDamage {
        get {
            return Mathf.FloorToInt(plusDamage);
        }

        set {
            plusDamage = value;
        }
    }

    /// <summary>
    /// 用于计算总伤害的get方法
    /// </summary>
    public int TotalDamage {
        get {
            return Mathf.FloorToInt(baseDamage + plusDamage);
        }
    }

    public static Damage Zero {
        get {
            return new Damage(0, 0);
        }
    }

    public static Damage operator *(Damage a, float d) {
        a.baseDamage *= d;
        a.plusDamage *= d;
        return a;
    }

}

