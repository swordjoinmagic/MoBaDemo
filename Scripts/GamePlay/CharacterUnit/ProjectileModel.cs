using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 投射物Model类
/// </summary>
public class ProjectileModel{
    // 该投射物影响范围的半径,为0时,不造成范围伤害
    public float spherInfluence = 0;
    // 对敌人造成伤害后,敌人身上产生的特效
    public GameObject tartgetEnemryEffect;
    // 当该投射物到达目的后,产生的特效
    public GameObject targetPositionEffect;
    // 移动速度
    public float movingSpeed;

}
