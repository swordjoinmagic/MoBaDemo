using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class BaseSkill {

    public string skillName;

    // 图标地址，用地址保存，当要使用时进行加载
    public string iconPath;

    public string description;

    // 技能等级
    public int skillLevel;

    public SkillType skillType;

    public SkillType SkillType {
        get {
            return skillType;
        }

        set {
            skillType = value;
        }
    }

    public int SkillLevel {
        get {
            return skillLevel;
        }

        set {
            skillLevel = value;
        }
    }

    public string Description {
        get {
            return description;
        }

        set {
            description = value;
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

    // 执行伤害效果
    public virtual Damage CalculateDamage() {
        return Damage.Zero;
    }
}
