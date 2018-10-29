using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using uMVVM;

/// <summary>
/// 英雄单位,特殊的Character单位
/// </summary>
[Serializable]
public class HeroModel : CharacterModel{
    // 力量
    public float forcePower;
    // 力量成长
    public float forcePowerGrowthPoint;
    // 敏捷
    public float agilePower;
    // 敏捷成长
    public float agilePowerGrowthPoint;
    // 智力
    public float intelligencePower;
    // 智力成长
    public float intelligenceGrowthPoint;
    // 技能点
    public int skillPoint;
    // 技能点成长
    public int skillPointGrowthPoint;
    // 经验值
    private BindableProperty<int> exp = new BindableProperty<int>();
    // 经验值因子（每次升级所需经验值关联系数）
    public float expfactor;
    // 升级所需经验值(指第0级升到第一级所需经验) 
    public float needExp;
    // 英雄头像图片的地址
    public string AvatarImagePath;

    public int Exp {
        get {
            return exp.Value;
        }
        set {
            exp.Value = value;
        }
    }
    public BindableProperty<int>.OnValueChangeHandler ExpChangedHandler {
        get { return exp.OnValueChange; }
        set { exp.OnValueChange = value; }
    }

    public int NextLevelNeedExp {
        get {
            return Mathf.FloorToInt(needExp * expfactor * (level+1));
        }
    }

    public override void Damaged(Damage damage) {
        base.Damaged(damage);
    }

    public override string ToString() {
        return "Name:" + name + " hp:" + Hp + " exp:" + Exp;
    }
}

