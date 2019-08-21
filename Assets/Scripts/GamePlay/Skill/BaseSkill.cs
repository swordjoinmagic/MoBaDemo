using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class BaseSkill<T> where T:BaseSkillModel{

    // 享元基类,保存技能对象不变的属性
    protected T skillModel;

    public BaseSkill(T skillModel) {
        this.skillModel = skillModel;
    }

    // 技能等级
    private int skillLevel;
    public int SkillLevel {
        get {
            return skillLevel;
        }

        set {
            skillLevel = value;
        }
    }

    // 对于英雄来说,该技能下一级需要的英雄等级
    private int nextLevelNeedHeroLevel;
    public int NextLevelNeedHeroLevel {
        get {
            return nextLevelNeedHeroLevel;
        }

        set {
            nextLevelNeedHeroLevel = value;
        }
    }

    // 最后一次释放技能的时间
    protected float finalSpellTime;

    #region 技能基类属性
    public string SkillType {
        get {
            return skillModel.SkillType;
        }
    }

    public string LongDescription {
        get {
            return skillModel.LongDescription;
        }
    }

    public string IconPath {
        get {
            return skillModel.IconPath;
        }
    }

    public string SkillName {
        get {
            return skillModel.SkillName;
        }
    }

    public string ShortDescription {
        get {
            return skillModel.ShortDescription;
        }
    }

    public string BackgroundDescription {
        get {
            return skillModel.BackgroundDescription;
        }
    }

    public UnitType SkillTargetType {
        get {
            return skillModel.SkillTargetType;
        }
    }

    #endregion

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
