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
    private BindableProperty<float> forcePower = new BindableProperty<float>();
    // 力量成长
    private float forcePowerGrowthPoint;
    // 敏捷
    private BindableProperty<float> agilePower = new BindableProperty<float>();
    // 敏捷成长
    private float agilePowerGrowthPoint;
    // 智力
    private BindableProperty<float> intelligencePower = new BindableProperty<float>();
    // 智力成长
    private float intelligenceGrowthPoint;
    // 技能点
    private BindableProperty<int> skillPoint = new BindableProperty<int>();
    // 技能点成长
    private int skillPointGrowthPoint;
    // 经验值
    private BindableProperty<int> exp = new BindableProperty<int>();
    // 经验值因子（每次升级所需经验值关联系数）
    private float expfactor;
    // 指定每级需要经验值，不指定的情况下，
    // 该级经验值由经验因子和第0级升到第1级所需经验得出
    public int[] expList;
    // 升级所需经验值(指第0级升到第一级所需经验) 
    private float needExp;
    // 英雄头像图片的地址
    private string avatarImagePath;
    // 英雄主属性
    private HeroMainAttribute mainAttribute;

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
            if (expList[Level + 1] != 0) return expList[Level+1];
            return Mathf.FloorToInt(NeedExp * Expfactor * (Level+1));
        }
    }

    public int SkillPoint {
        get {
            return skillPoint.Value;
        }

        set {
            skillPoint.Value = value;
        }
    }
    public BindableProperty<int>.OnValueChangeHandler SkillPointChangedHandler {
        get { return skillPoint.OnValueChange; }
        set { skillPoint.OnValueChange = value; }
    }

    public float ForcePower {
        get {
            return forcePower.Value;
        }

        set {
            forcePower.Value = value;
        }
    }
    public BindableProperty<float>.OnValueChangeHandler ForcePowerHandler {
        get {
            return forcePower.OnValueChange;
        }

        set {
            forcePower.OnValueChange = value;
        }
    }

    public float AgilePower {
        get {
            return agilePower.Value;
        }

        set {
            agilePower.Value = value;
        }
    }
    public BindableProperty<float>.OnValueChangeHandler AgilePowerHandler {
        get {
            return agilePower.OnValueChange;
        }

        set {
            agilePower.OnValueChange = value;
        }
    }

    public float IntelligencePower {
        get {
            return intelligencePower.Value;
        }

        set {
            intelligencePower.Value = value;
        }
    }
    public BindableProperty<float>.OnValueChangeHandler IntelligencePowerHandler {
        get {
            return intelligencePower.OnValueChange;
        }

        set {
            intelligencePower.OnValueChange = value;
        }
    }

    public float NeedExp {
        get {
            return needExp;
        }

        set {
            needExp = value;
        }
    }

    public string AvatarImagePath {
        get {
            return avatarImagePath;
        }

        set {
            avatarImagePath = value;
        }
    }

    public HeroMainAttribute MainAttribute {
        get {
            return mainAttribute;
        }

        set {
            mainAttribute = value;
        }
    }

    public float Expfactor {
        get {
            return expfactor;
        }

        set {
            expfactor = value;
        }
    }

    public int SkillPointGrowthPoint {
        get {
            return skillPointGrowthPoint;
        }

        set {
            skillPointGrowthPoint = value;
        }
    }

    public float IntelligenceGrowthPoint {
        get {
            return intelligenceGrowthPoint;
        }

        set {
            intelligenceGrowthPoint = value;
        }
    }

    public float ForcePowerGrowthPoint {
        get {
            return forcePowerGrowthPoint;
        }

        set {
            forcePowerGrowthPoint = value;
        }
    }

    public float AgilePowerGrowthPoint {
        get {
            return agilePowerGrowthPoint;
        }

        set {
            agilePowerGrowthPoint = value;
        }
    }

    public override void Damaged(Damage damage) {
        base.Damaged(damage);
    }

    public override string ToString() {
        return "Name:" + Name + " hp:" + Hp + " exp:" + Exp;
    }

    // 获得当前CharacterModel对象的深拷贝对象
    public override CharacterModel DeepCopy() {
        CharacterModel deepCopyModel = new HeroModel() {
            maxHp = maxHp,
            maxMp = maxMp,
            Hp = Hp,
            Mp = Mp,
            Name = Name,
            attackDistance = attackDistance,
            BaseSkills = new List<BaseSkill>(),
            Attack = attack,
            AttackFloatingValue = attackFloatingValue,
            AttackSpeed = attackSpeed,
            Defense = defense,
            MovingSpeed = movingSpeed,
            TurningSpeed = turningSpeed,
            Level = Level,
            RestoreHpSpeed = RestoreHpSpeed,
            ResotreMpSpeed = ResotreMpSpeed,
            canBeAttacked = canBeAttacked,
            unitType = unitType,
            unitFaction = unitFaction,
            supportExp = supportExp,
            supportMoney = supportMoney,
            PhysicalResistance = PhysicalResistance,
            MagicalResistance = MagicalResistance,
            DodgeRate = DodgeRate,
            NeedExp = 2000,
            Expfactor = 0.5f,
            Radius = Radius
        };
        return deepCopyModel;
    }

}

