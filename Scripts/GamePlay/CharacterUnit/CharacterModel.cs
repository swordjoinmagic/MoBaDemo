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

    #region 单位的基本属性
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
    private BindableProperty<int> level = new BindableProperty<int>();
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
    // 物理抗性
    public float physicalResistance;
    // 魔法抗性
    public float magicalResistance;
    // 闪避率
    public float dodgeRate;


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

    public int Level {
        get {
            return level.Value;
        }

        set {
            level.Value = value;
        }
    }

    #endregion


    #region 单位用于战争迷雾的属性
    //=====================================
    // 用于战争迷雾
    private Vector3 position;
    private float radius;
    private bool isVisible = false;
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
    #endregion


    #region 属性与ViewModel双向绑定 用于向订阅属性改变事件的UI发送消息
    public BindableProperty<int>.OnValueChangeHandler HpValueChangedHandler {
        get {
            return hp.OnValueChange;
        }
        set {
            hp.OnValueChange = value;
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
    public BindableProperty<int>.OnValueChangeHandler LevelChangedHandler {
        get {
            return level.OnValueChange;
        }
        set {
            level.OnValueChange = value;
        }
    }
    #endregion


    #region 属性 Plus 设置
    //================================================================
    // ● Plus 设置处
    //===============================================================

    /// <summary>
    ///  单位附加攻击力,此处所指的附加攻击力指的是 
    ///  1. 被动技能
    ///  2. 装备 
    ///  等等
    ///  所带来的附加攻击力
    /// </summary>
    public int AttackPlus {
        get {
            int newAttackPlus = 0;
            int plus;
            // 根据 单位身上的 被动技能 和 装备 来计算攻击附加值

            foreach (var passiveSkill in passiveSkills) {
                if (passiveSkill.TiggerType == PassiveSkillTriggerType.GainAttribute) {
                    passiveSkill.Execute(this,out plus,CharacterAttribute.Attack,attack);
                    newAttackPlus += plus;
                }
            }

            foreach (var itemGrid in itemGrids) {
                if (itemGrid!=null && itemGrid.item != null) {
                    foreach (var passiveSkill in itemGrid.item.itemPassiveSkills) {
                        if (passiveSkill.TiggerType == PassiveSkillTriggerType.GainAttribute) {
                            passiveSkill.Execute(this, out plus, CharacterAttribute.Attack, attack);
                            newAttackPlus += plus;
                        }
                    }
                }
            }
            return newAttackPlus;
        }
    }
    #endregion


    #region 属性 Total 设置

    public int TotalAttack {
        get {
            return attack + AttackPlus;
        }
    }
    #endregion


    /// <summary>
    /// 受到伤害时执行的方法
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damaged(Damage damage) {
        // 进行一系列计算
        // toDo

        // 统计伤害
        Hp -= damage.TotalDamage;
    }

    public delegate void OnDamageHandler(Damage damage,CharacterMono attacker,int nowHp);
    public event OnDamageHandler OnDamaged;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attacker">袭击这个单位的人</param>
    public virtual void Damaged(Damage damage,CharacterMono attacker) {
        Damaged(damage);

        if (OnDamaged != null)
            OnDamaged(damage,attacker,Hp);
    }



    // 获得当前CharacterModel对象的深拷贝对象
    public virtual CharacterModel DeepCopy() {
        CharacterModel deepCopyModel = new CharacterModel() { };
        return deepCopyModel;
    }
}

