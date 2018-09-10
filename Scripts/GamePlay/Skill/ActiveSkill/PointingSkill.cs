using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 所有指向型技能的基类,包含了指向型技能的基本特性:
///     1.target:指向的单位
///     2.self:自身
///     3.selfEffect:释放技能时自身的特效/动画
///     4.targetEffect:释放技能时敌人的特效/动画
/// </summary>
public class PointingSkill : ActiveSkill{
    public GameObject target;
    public GameObject self;
    public GameObject selfEffect;
    public GameObject targetEffect;

    public override Damage Execute() {
        GameObject tempSelfEffect = null;
        GameObject tempTargetEffect = null;
        if (selfEffect!=null)
            tempSelfEffect = GameObject.Instantiate(selfEffect,self.transform);
        if(targetEffect!=null)
            tempTargetEffect = GameObject.Instantiate(targetEffect, target.transform);
        return base.Execute();
    }
}

