using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class BaseSkill {

    // 技能在技能列表的ID号
    private int skillID;

    private string skillName;

    // 图标地址，用地址保存，当要使用时进行加载
    private string iconPath;

    // 技能长描述
    private string longDescription;
    // 技能短描述
    private string shortDescription;
    // 技能背景描述
    private string backgroundDescription;

    // 技能等级
    private int skillLevel;

    // 对于英雄来说的,该技能下一级需要的英雄等级
    private int nextLevelNeedHeroLevel;

    private string skillType;

    public string SkillType {
        get {
            return skillType;
        }

        set {
            skillType = value;
        }
    }

    public virtual int SkillLevel {
        get {
            return skillLevel;
        }

        set {
            skillLevel = value;
        }
    }

    public string LongDescription {
        get {
            return longDescription;
        }

        set {
            longDescription = value;
        }
    }

    public string IconPath {
        get {
            return iconPath;
        }

        set {
            iconPath = value;
        }
    }

    public string SkillName {
        get {
            return skillName;
        }

        set {
            skillName = value;
        }
    }

    public string ShortDescription {
        get {
            return shortDescription;
        }

        set {
            shortDescription = value;
        }
    }

    public string BackgroundDescription {
        get {
            return backgroundDescription;
        }

        set {
            backgroundDescription = value;
        }
    }

    /// <summary>
    /// 对技能的目标,附加的状态,造成的伤害等进行描述,由子类进行重写
    /// </summary>
    public virtual string TargetDescription {
        get {
            return "";
        }
    }

    public int NextLevelNeedHeroLevel {
        get {
            return nextLevelNeedHeroLevel;
        }

        set {
            nextLevelNeedHeroLevel = value;
        }
    }

    public UnitType SkillTargetType {
        get {
            return skillTargetType;
        }

        set {
            skillTargetType = value;
        }
    }

    public int SkillID {
        get {
            return skillID;
        }

        set {
            skillID = value;
        }
    }

    // 此技能允许释放的目标，是一个多重枚举
    protected UnitType skillTargetType;

    /// <summary>
    /// 判断此技能的指向目标是否包含某个目标（即targetType）,
    /// 举个例子，此技能的指向目标是 Enermy | Firend | Tree,此时询问此技能指向目标是否包含Enermy，
    /// 返回True
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public bool ContainsTarget(UnitType targetType) {
        return (SkillTargetType & targetType) == targetType;
    }

    /// <summary>
    /// 判断某个单位可以被准备释放的技能攻击
    /// </summary>
    /// <returns></returns>
    protected bool CanBeExecuteToTarget(CharacterMono speller, CharacterMono target) {
        // 1. 施法者和目标阵营不一样，且目标阵营不属于中立阵营，则目标属于敌人
        if (!speller.CompareOwner(target)) {
            if (ContainsTarget(UnitType.Enermy))
                return true;
        } else {
            // 2. 施法者和目标阵营一样，则目标属于朋友单位
            if (ContainsTarget(UnitType.Friend)) {
                return true;
            }
        }

        // 3. 目标是英雄单位
        if (target is HeroMono) {
            if (ContainsTarget(UnitType.Hero)) {
                return true;
            }
        }

        // 4. 目标是建筑物
        if ((target.characterModel.unitType & UnitType.Building) == UnitType.Building)
            if (ContainsTarget(UnitType.Building))
                return true;

        // 5. 目标是守卫
        if ((target.characterModel.unitType & UnitType.Guard) == UnitType.Guard) {
            if (ContainsTarget(UnitType.Guard))
                return true;
        }

        // default:
        return false;
    }

}
