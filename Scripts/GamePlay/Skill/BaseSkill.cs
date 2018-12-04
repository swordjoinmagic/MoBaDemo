using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class BaseSkill {

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

    // 执行伤害效果
    public virtual Damage CalculateDamage() {
        return Damage.Zero;
    }
}
