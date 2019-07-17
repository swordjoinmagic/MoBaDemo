using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 路径点管理器，用来管理单位上中下三条路径
/// </summary>
class WayPointsManager : MonoBehaviour{
    public static WayPointsManager wayPointsManager;
    public static WayPointsManager Instance {
        get {
            if (wayPointsManager==null) {
                wayPointsManager = GameObject.FindObjectOfType<WayPointsManager>();
            }
            return wayPointsManager;
        }
    }

    // 上路的路径点
    public List<Vector3> upRoadPoints;
    // 中路的路径点
    public List<Vector3> middleRoadPoints;
    // 下路的路径点
    public List<Vector3> downRoadPoints;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (var position in upRoadPoints) {
            Gizmos.DrawSphere(position, 1f);
        }
        Gizmos.color = Color.green;
        foreach (var position in middleRoadPoints) {
            Gizmos.DrawSphere(position, 1f);
        }
        Gizmos.color = Color.blue;
        foreach (var position in downRoadPoints) {
            Gizmos.DrawSphere(position, 1f);
        }
    }
}

