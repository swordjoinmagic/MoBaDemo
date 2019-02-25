using System.Collections.Generic;
using UnityEngine;
using uMVVM;
using System;

/// <summary>
/// 基本单位,所有单位的基类
/// </summary>
[Serializable]
public class CharacterModel : IFOVUnit,IAudioUnit{

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
    private string name;
    // 该单位的普通攻击距离
    public float attackDistance;

    #region 技能相关
    // 当前角色的所有技能
    public List<BaseSkill> baseSkills = new List<BaseSkill>();
    // 当前角色的所有主动技能
    public List<ActiveSkill> activeSkills = new List<ActiveSkill>();
    // 当前角色所有被动技能
    public List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
    // 单位可以拥有的最大技能数
    public int MaxSkillCount {
        get {
            return 5;
        }
    }
    #endregion

    // 单位身上拥有的所有物品
    public List<ItemGrid> itemGrids;
    // 单位攻击力
    public int attack;
    // 攻击浮动值,举例 当浮动值=5，攻击值=10，那么整体攻击值应该在5-15之间浮动。
    // 即[attack-float , attack+float]这样一个区间内浮动
    public int attackFloatingValue;
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
    public float movingSpeed;
    // 转身速度
    public float turningSpeed;
    // 投射物Model类属性
    public ProjectileModel projectileModel;
    // 投射物
    public GameObject projectile;
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
    // 单位守备范围（会自动攻击距离多少范围内的敌人）
    public float defenseRange = 100;

    #region 基础属性 Getter/Setter
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

    public float AttackSpeed {
        get {
            return attackSpeed;
        }

        set {
            attackSpeed = value;
        }
    }

    public int AttackFloatingValue {
        get {
            return attackFloatingValue;
        }

        set {
            attackFloatingValue = value;
        }
    }

    public int Attack {
        get {
            return attack;
        }

        set {
            attack = value;
        }
    }

    public int Defense {
        get {
            return defense;
        }

        set {
            defense = value;
        }
    }

    public float MovingSpeed {
        get {
            return movingSpeed;
        }

        set {
            movingSpeed = value;
        }
    }

    public float RestoreHpSpeed {
        get {
            return restoreHpSpeed;
        }

        set {
            restoreHpSpeed = value;
        }
    }

    public float ResotreMpSpeed {
        get {
            return resotreMpSpeed;
        }

        set {
            resotreMpSpeed = value;
        }
    }

    public float PhysicalResistance {
        get {
            return physicalResistance;
        }

        set {
            physicalResistance = value;
        }
    }

    public float MagicalResistance {
        get {
            return magicalResistance;
        }

        set {
            magicalResistance = value;
        }
    }

    public float DodgeRate {
        get {
            return dodgeRate;
        }

        set {
            dodgeRate = value;
        }
    }
    #endregion

    #endregion

    #region 单位音频属性
    private string attackAudioPath;
    private string moveAudioPath;
    public string AttackAudioPath {
        get {
            return attackAudioPath;
        }
        set {
            attackAudioPath = value;
        }
    }
    public string MoveAudioPath {
        get {
            return moveAudioPath;
        }
        set {
            moveAudioPath = value;
        }
    }
    #endregion

    #region 单位用于战争迷雾的属性
    //=====================================
    // 用于战争迷雾
    private Vector3 position;
    [SerializeField]
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

    // 附加攻击力
    private int attackPlus;
    public int AttackPlus {
        get {
            return attackPlus;
        }
        set {
            attackPlus = value;
        }
    }

    // 附加防御力
    private int defensePlus;
    public int DefensePlus {
        get {
            return defensePlus;
        }

        set {
            defensePlus = value;
        }
    }

    // 附加攻击距离
    private float attackDistancePlus;
    public float AttackDistancePlus {
        get {
            return attackDistancePlus;
        }

        set {
            attackDistancePlus = value;
        }
    }

    // 附加移动速度
    private int movingSpeedPlus;
    public int MovingSpeedPlus {
        get {
            return movingSpeedPlus;
        }

        set {
            movingSpeedPlus = value;
        }
    }

    // 附加回血速度
    private float restoreHpSpeedPlus;
    public float RestoreHpSpeedPlus {
        get {
            return restoreHpSpeedPlus;
        }

        set {
            restoreHpSpeedPlus = value;
        }
    }

    // 附加回魔速度
    private float resotreMpSpeedPlus;
    public float ResotreMpSpeedPlus {
        get {
            return resotreMpSpeedPlus;
        }

        set {
            resotreMpSpeedPlus = value;
        }
    }

    // 附加物理抗性
    private float physicalResistancePlus;
    public float PhysicalResistancePlus {
        get {
            return physicalResistancePlus;
        }

        set {
            physicalResistancePlus = value;
        }
    }

    // 附加魔法抗性
    private float magicalResistancePlus;
    public float MagicalResistancePlus {
        get {
            return magicalResistancePlus;
        }

        set {
            magicalResistancePlus = value;
        }
    }

    private float dodgeRatePlus;
    public float DodgeRatePlus {
        get {
            return dodgeRatePlus;
        }

        set {
            dodgeRatePlus = value;
        }
    }
    #endregion


    #region 属性 Total 设置

    public int TotalAttack {
        get {
            return Attack + AttackPlus;
        }
    }

    public int TotalDefense {
        get {
            return Defense+DefensePlus;
        }
    }

    public float TotalMovingSpeed {
        get {
            return MovingSpeed+movingSpeedPlus;
        }
    }

    public float TurningSpeed {
        get {
            return turningSpeed;
        }

        set {
            turningSpeed = value;
        }
    }

    public float TotalRestoreHpSpeed {
        get {
            return RestoreHpSpeed+resotreMpSpeedPlus;
        }
    }

    public float TotalResotreMpSpeed {
        get {
            return ResotreMpSpeed+resotreMpSpeedPlus;
        }
    }

    public float TotalPhysicalResistance {
        get {
            return PhysicalResistance + physicalResistancePlus;
        }
    }

    public float TotalMagicalResistance {
        get {
            return MagicalResistance+magicalResistancePlus;
        }
    }

    public float TotalDodgeRate {
        get {
            return DodgeRate+dodgeRatePlus;
        }
    }

    public string Name {
        get {
            return name;
        }

        set {
            name = value;
        }
    }

    public List<BaseSkill> BaseSkills {
        get {
            return baseSkills;
        }

        set {
            baseSkills = value;
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

    public delegate void OnDamageHandler(CharacterMono victim, Damage damage,CharacterMono attacker,int nowHp);
    public event OnDamageHandler OnDamaged;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attacker">袭击这个单位的人</param>
    public virtual void Damaged(CharacterMono victim, Damage damage,CharacterMono attacker) {
        Damaged(damage);

        if (OnDamaged != null) {
            OnDamaged(victim, damage, attacker, Hp);
        }
    }

    /// <summary>
    /// 计算单位能对目标造成的一次普通伤害
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Damage GetDamage(CharacterModel target) {
        return new Damage { BaseDamage= UnityEngine.Random.Range(TotalAttack - AttackFloatingValue, TotalAttack + AttackFloatingValue) };
    }

    // 获得当前CharacterModel对象的深拷贝对象
    public virtual CharacterModel DeepCopy() {
        CharacterModel deepCopyModel = new CharacterModel() {
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
            Radius = Radius,
        };
        return deepCopyModel;
    }
}

