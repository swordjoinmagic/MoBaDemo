using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WayPointsUnit {
    /// <summary>
    /// 该单位寻路的类型，是上中下三条哪条路
    /// </summary>
    private WayPointEnum wayPointType;
    private UnitFaction unitFaction;

    /// <summary>
    /// 该单位拥有的路径点
    /// </summary>
    private List<Vector3> wayPoints;
    // 该单位当前所在的路径点的下标
    public int nowIndex = 0;

    /// <summary>
    /// 根据wayPointType初始化路径点
    /// </summary>
    public WayPointsUnit(WayPointEnum WayPointType, UnitFaction unitFaction) {
        switch (WayPointType) {
            case WayPointEnum.UpRoad:
                wayPoints = WayPointsManager.Instance.upRoadPoints;
                break;
            case WayPointEnum.MiddleRoad:
                wayPoints = WayPointsManager.Instance.middleRoadPoints;
                break;
            case WayPointEnum.DownRoad:
                wayPoints = WayPointsManager.Instance.downRoadPoints;
                break;
        }
        this.unitFaction = unitFaction;
        switch (unitFaction) {
            case UnitFaction.Blue:
                nowIndex = wayPoints.Count - 1;
                break;
            case UnitFaction.Red:
                nowIndex = 0;
                break;
        }
    }

    public Vector3 GetNextWayPoint() {
        if (unitFaction == UnitFaction.Red) {
            if (nowIndex == wayPoints.Count - 1) {
                return wayPoints[nowIndex];
            }
            // 在一开始先获得下标为0的路径点
            return wayPoints[nowIndex++];
        } else if (unitFaction == UnitFaction.Blue) {
            if (nowIndex == 0) {
                return wayPoints[nowIndex];
            }
            // 在一开始先获得下标为0的路径点
            return wayPoints[nowIndex--];
        }
        return Vector3.zero;
    }

    public Vector3 GetNowWayPoint() {
        return wayPoints[nowIndex];
    }

    /// <summary>
    /// 找到离单位当前位置最近的路径点，
    /// 并将当前路径点nowIndex设为这个找到的路径点
    /// </summary>
    /// <param name="position">单位当前位置</param>
    /// <returns></returns>
    public void GetNearestWayPoint(Vector3 position) {
        // 判断是否是终点，如果是，返回
        if (nowIndex==wayPoints.Count-1) return;

        // 从当前路径点往下找离自己最近的
        float distance = Vector3.Distance(position,wayPoints[nowIndex]);
        int nearestPointIndex = nowIndex;
        for (int i = 0; i < wayPoints.Count; i++) {
            float tempDistance = Vector3.Distance(position,wayPoints[i]);
            //Debug.Log("TempDistance:"+tempDistance+" i:"+i);
            if (tempDistance<distance) {
                distance = tempDistance;
                nearestPointIndex = i;
            }            
        }
        //Debug.Log("最近的点是："+nearestPointIndex+" 距离是："+distance);

        // 设置当前路径点
        nowIndex = nearestPointIndex + (unitFaction==UnitFaction.Red?1:-1);

        if (nowIndex >= wayPoints.Count())
            nowIndex = wayPoints.Count() - 1;
        else if (nowIndex < 0) {
            nowIndex = 0;
        }
    }
}

