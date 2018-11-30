using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 用于技能组的技能延迟属性
/// </summary>
public struct SkillDelayAttribute {
    // 表示此技能是否延迟
    public bool isDelay;
    // 表示此技能如果延迟,将在第几个技能释放完毕之后再进行释放
    public int index;
}

