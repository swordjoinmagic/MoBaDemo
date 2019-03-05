using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 投射物Model类
/// </summary>
[Serializable]
public class ProjectileModel{
    // 该投射物影响范围的半径,为0时,不造成范围伤害
    public float spherInfluence = 0;
    // 对敌人造成伤害后,敌人身上产生的特效
    public GameObject tartgetEnemryEffect;
    // 当该投射物到达目的后,产生的特效
    public GameObject targetPositionEffect;
    // 移动速度
    public float movingSpeed = 1;
    // 上升高度
    public float riseHeight = 5;
    // 开始的角度(基于水平面计算)
    public float angle = 30;
    // 结束时的角度(基于水平面)
    public float endAngle = 130;
    // 表示此次运动是否为弧线运动
    public bool isArcMotion = true;
    // 模板对象
    GameObject templateObject = null;
}
