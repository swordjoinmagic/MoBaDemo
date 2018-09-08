using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Util {
    /// <summary>
    /// 进行缓慢扣血的协程
    /// </summary>
    /// <param name="different">本次一共扣多少血</param>
    /// <param name="different">每次扣血的间隔时间</param>
    /// <returns></returns>
    public static IEnumerator SlowDown(int different, int nowData, int newData,
                                int maxData, RectTransform dataRectTransform, int maxImageHeight,
                                int maxImageWidth) {
        int tempData = nowData;
        int tempDifference = 0;
        // 判断是加还是减
        bool isIncrease = newData > nowData ? true : false;

        // 每次变化的量，每次按最大值血量的1%来递减
        int step = isIncrease ? (int)(maxData * 0.01) : (int)(-maxData * 0.01);

        while (tempDifference < different) {
            tempDifference += Math.Abs(step);

            // 血量变化
            tempData += step;

            // 计算此时血量与最大血量的比值
            float rate = (float)tempData / maxData;

            // 计算此时血条的长度
            float width = rate * maxImageWidth;

            // 血条变化
            dataRectTransform.sizeDelta = new Vector2(width, maxImageHeight);

            yield return new WaitForFixedUpdate();
        }
    }
}
