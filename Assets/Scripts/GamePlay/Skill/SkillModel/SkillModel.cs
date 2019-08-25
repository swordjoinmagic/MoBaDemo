using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 基于享元模式的技能对象基类,
/// 保存技能对象中不变的属性,
/// 也就是在数据编辑器中编辑好的属性,
/// 在整场游戏中都不会发生太大的变化
/// </summary>
public class SkillModel {
    private string skillName;
    // 图标地址，用地址保存，当要使用时进行加载
    private string iconPath;
    // 技能长描述
    private string longDescription;
    // 技能短描述
    private string shortDescription;
    // 技能背景描述
    private string backgroundDescription;
    private string skillType;
    // 对技能的目标,附加的状态,造成的伤害等进行描述,由子类进行重写
    private string targetDescription;
    // 此技能允许释放的目标，是一个多重枚举
    private UnitType skillTargetType;
    // 消耗mp
    private int mp;
    // 热键,如果为被动技能,那么为None
    private KeyCode keyCode;
    // 释放该技能需要的最近距离,
    // 对于主动技能来说,释放技能时必须要与目标距离小于这个距离才能释放,
    // 对于被动技能来说,触发技能时必须要与目标距离小于这个距离才能触发
    // 为负数时表示可以无视距离进行释放/触发
    private float spellDistance;
    // 技能影响范围，是一个以r为半径的圆
    private float skillInfluenceRadius;
    // 技能CD时间
    private float cooldown;
    // 施法持续时间,为0时表示该技能不会持续释放
    private float spellDuration;
    // 表示该技能是否可以被打断,默认所有技能都不能被打断,
    // 部分持续施法技能可以被单位的操作/其他技能打断
    private float canBeInterrupt;
    // 施法时，自身产生的特效
    private GameObject selfEffect;
    // 施法时，目标产生的特效
    private GameObject targetEffect;
    private int skillID;
    // 额外属性(子类技能/操作中额外需要的属性,如闪电链技能需要链接次数等等)
    private Dictionary<string, object> extraAttributes = new Dictionary<string, object>();
    

    public SkillModel() { }

    // 实现对extraAttributes的快速赋值
    public SkillModel(params Tuple<string,object>[] tuples) {
        foreach (var o in tuples) {
            extraAttributes[o.First] = o.Second;
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

    public string IconPath {
        get {
            return iconPath;
        }

        set {
            iconPath = value;
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

    public string SkillType {
        get {
            return skillType;
        }

        set {
            skillType = value;
        }
    }

    public string TargetDescription {
        get {
            return targetDescription;
        }

        set {
            targetDescription = value;
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

    public int Mp {
        get {
            return mp;
        }

        set {
            mp = value;
        }
    }

    public KeyCode KeyCode {
        get {
            return keyCode;
        }

        set {
            keyCode = value;
        }
    }

    public float SpellDistance {
        get {
            return spellDistance;
        }

        set {
            spellDistance = value;
        }
    }

    public float SkillInfluenceRadius {
        get {
            return skillInfluenceRadius;
        }

        set {
            skillInfluenceRadius = value;
        }
    }

    public float Cooldown {
        get {
            return cooldown;
        }

        set {
            cooldown = value;
        }
    }

    public float SpellDuration {
        get {
            return spellDuration;
        }

        set {
            spellDuration = value;
        }
    }

    public float CanBeInterrupt {
        get {
            return canBeInterrupt;
        }

        set {
            canBeInterrupt = value;
        }
    }

    public GameObject SelfEffect {
        get {
            return selfEffect;
        }

        set {
            selfEffect = value;
        }
    }

    public GameObject TargetEffect {
        get {
            return targetEffect;
        }

        set {
            targetEffect = value;
        }
    }

    public Dictionary<string, object> ExtraAttributes {
        get {
            return extraAttributes;
        }

        set {
            extraAttributes = value;
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
}

