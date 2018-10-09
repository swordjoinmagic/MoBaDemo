using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 单位的阵营,在这里简单把阵营分为红蓝两个阵营
/// </summary>
public enum UnitFaction {
    Red = 0,        // 红方阵营
    Blue = 1,        // 蓝方阵营
    Neutral = 2,     // 中立阵营,即所有玩家的盟友
    Hostility = 3    // 敌对阵营,即所有玩家的敌人
}

