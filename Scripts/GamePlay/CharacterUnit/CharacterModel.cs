using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using uMVVM;

/// <summary>
/// 基本单位,所有单位的基类
/// </summary>
[Serializable]
public class CharacterModel : IFOVUnit{
    // 生命值
    private BindableProperty<int> hp = new BindableProperty<int>();
    // 魔力值
    private BindableProperty<int> mp = new BindableProperty<int>();
    // 最大生命值
    public int maxHp;
    // 最大魔法值
    public int maxMp;
    // 单位名称
    public string name;
    // 该单位的普通攻击距离
    public float attackDistance;
    // 当前角色的所有技能
    public List<BaseSkill> baseSkills = new List<BaseSkill>();
    // 当前角色的所有主动技能
    public List<ActiveSkill> activeSkills = new List<ActiveSkill>();
    // 当前角色所有被动技能
    public List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
    // 单位身上拥有的所有物品
    public List<ItemGrid> itemGrids;
    // 单位攻击力
    public int attack;
    // 单位攻击类型
    public AttackType attackType;
    // 单位攻击速度,百分比单位,当为1的时候,表示原始速度100%,当为1.5的时候,表示比原始速度快50%,即150%.
    // 最快的攻击速度是一帧一次
    public float attackSpeed;
    // 单位防御力
    public int defense;
    // 单位防御类型
    public DefenseType defenseType;
    // 移动速度
    public int movingSpeed;
    // 转身速度
    public int turningSpeed;
    // 投射物Model类属性
    public ProjectileModel projectileModel;
    // 投射物Mono类
    public ProjectileMono projectile;
    // 等级
    public int level;
    // 回血速度，以秒为单位，即以秒回多少血
    public float restoreHpSpeed;
    // 回魔速度，以秒为单位，即以秒回多少Mp
    public float resotreMpSpeed;
    // 是否可被攻击(即是否无敌)
    public bool canBeAttacked = true;
    // 单位的类型
    public UnitType unitType;
    // 单位所属阵营
    public UnitFaction unitFaction;
    // 单位被杀死后将提供给英雄单位多少经验
    public int supportExp;
    // 单位被杀死后将提供给玩家单位多少金钱
    public int supportMoney;

    //=====================================
    // 用于战争迷雾
    public Vector3 position;
    public float radius;
    public bool isVisible = false;

    //public CharacterModel() {
    //    Hp = maxHp;
    //    Mp = maxMp;
    //}
    public BindableProperty<int>.OnValueChangeHandler HpValueChangedHandler {
        get {
            return hp.OnValueChange;
        }
        set {
            hp.OnValueChange = value;
        }
    }
    public int Hp {
        get {
            return hp.Value;
        }

        set {
            // 在bindproperty上再加一层封装,避免hp溢出
            if (value > maxHp)
                hp.Value = maxHp;
            else if (value < 0)
                hp.Value = 0;
            else
                hp.Value = value;
        }
    }
    public BindableProperty<int>.OnValueChangeHandler MpValueChangedHandler {
        get {
            return mp.OnValueChange;
        }
        set {
            mp.OnValueChange = value;
        }
    }
    public int Mp {
        get {
            return mp.Value;
        }

        set {
            if (value > maxMp)
                mp.Value = maxMp;
            else if (value < 0)
                mp.Value = 0;
            else
                mp.Value = value;
        }
    }

    public Vector3 Position {
        get {
            return position;
        }
        set {
            position = value;
        }
    }

    public float Radius {
        get {
            return radius;
        }
        set {
            radius = value;
        }
    }


    public bool IsVisible {
        get {
            return isVisible;
        }
        set {
            isVisible = value;
        }
    }


    /// <summary>
    /// 受到伤害时执行的方法
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damaged(Damage damage) {
        // 进行一系列计算
        // pass

        // 统计伤害
        Hp -= damage.TotalDamage;
    }

    // 获得当前CharacterModel对象的深拷贝对象
    public virtual CharacterModel DeepCopy() {
        CharacterModel deepCopyModel = new CharacterModel() { };
        return deepCopyModel;
    }
}

