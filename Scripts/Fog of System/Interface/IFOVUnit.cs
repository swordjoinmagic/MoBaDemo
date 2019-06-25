using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 视野体单位接口
/// </summary>
public interface IFOVUnit {
    /// <summary>
    /// 单位当前位置,用于提供给战争迷雾逻辑层判断当前单位是否可见
    /// </summary>
    Vector3 Position { get; set; }

    /// <summary>
    /// 单位当前旋转角度，用于fov角度的视野
    /// </summary>
    Quaternion Rotation { get; set; }

    /// <summary>
    /// 单位的视野属性,用于战争迷雾表现层
    /// </summary>
    float Radius { get; set; }
    /// <summary>
    /// 单位当前是否可见,用于战争迷雾逻辑层
    /// </summary>
    bool IsVisible { get; set; }
}

