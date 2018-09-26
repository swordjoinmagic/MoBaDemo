using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 基本单位,所有单位的基类
/// </summary>
[System.Serializable]
public class CharacterModel {
    // 生命值
    private int hp;
    // 魔力值
    private int mp;
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
    // 单位攻击力
    public int attack;
    // 单位攻击类型
    public AttackType attackType;
    // 单位防御力
    public int defense;
    // 单位防御类型
    public DefenseType defenseType;
    // 移动速度
    public int movingSpeed;
    // 转身速度
    public int turningSpeed;
    // 投射物
    public ProjectileModel projectile;
    // 等级
    public int level;
    // 回血速度
    public float restoreHpSpeed;
    // 回魔速度
    public float resotreMpSpeed;
    // 是否可被攻击(即是否无敌)
    public Boolean canBeAttacked;

    public int Hp {
        get {
            return hp;
        }

        set {
            if (value > maxHp)
                hp = maxHp;
            else if (value < 0)
                hp = 0;
            else
                hp = value;
        }
    }

    public int Mp {
        get {
            return mp;
        }

        set {
            if (value > maxMp)
                mp = maxMp;
            else if (value < 0)
                mp = 0;
            else
                mp = value;
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
        hp -= damage.TotalDamage;
    }
}

