using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 单位类型,用于区分不同的单位
/// </summary>
[GameType]
public enum UnitType {
    Everything = ~0,
    Enermy = 1,      // 敌人
    Friend = 2,      // 盟友单位
    Hero = 4,        // 英雄单位
    Guard = 8,      // 守卫
    Building = 16,  // 建筑物
    None = 0,
}

