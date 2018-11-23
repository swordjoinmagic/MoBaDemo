using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// 将一个UI控件的宽度缓慢变化到另一个值（另一个UI控件的宽度）
    /// </summary>
    /// <param name="hpImageRectTransform">阈值，当dataTransform的宽度下降\增加到此值后，协程停止</param>
    /// <param name="dataRectTransform">宽度要下降的UI控件</param>
    /// <param name="maxImageHeight">UI控件的最大高度</param>
    /// <param name="maxImageWidth">UI控件的最大宽度</param>
    /// <param name="maxData">最大变化量</param>
    /// <returns></returns>
    public static IEnumerator SlowDown(RectTransform hpImageRectTransform, 
        RectTransform dataRectTransform, float maxImageHeight) {

        // 当dataRectTransform的宽度小于hpImageRectTransform时，协程停止
        while (Mathf.Abs(dataRectTransform.sizeDelta.x - hpImageRectTransform.sizeDelta.x)>0.1f) {

            //Debug.Log("扣血协程迭代中:"+ Mathf.Abs(dataRectTransform.sizeDelta.x - hpImageRectTransform.sizeDelta.x) );

            // 每次变化的量，每次按变化的2%来递减\增(最小变化量为0.1)
            float step = (hpImageRectTransform.sizeDelta.x - dataRectTransform.sizeDelta.x)*0.02f;

            // 血条变化
            dataRectTransform.sizeDelta = new Vector2(dataRectTransform.sizeDelta.x+step, maxImageHeight);

            yield return new WaitForFixedUpdate();
        }

        //Debug.Log("协程结束");
    }
}
