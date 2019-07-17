using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 表示粒子特效生命周期的种类
/// </summary>
public enum EffectConditonalType {
    During,        // 该特效将会存在一段时间
    Auto,          // 播放完粒子特效自动消失
    BattleState,   // 状态型持久特效
}

