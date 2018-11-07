using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

class HPView : UnityGuiView<HPViewModel>{

    //====================================
    // 此View管理的UI控件
    public Text hpText;
    public Text mpText;
    public RectTransform hpImage;
    public RectTransform mpImage;
    public int hpImageMaxWidth;
    public int mpImageMaxWidth;
    public int hpImageHeight;
    public int mpImageHeight;

    protected override void OnInitialize() {
        base.OnInitialize();

        binder.Add<int>("Hp", OnHpChanged);
        
    }

    /// <summary>
    /// 进行缓慢扣血的协程
    /// </summary>
    /// <param name="different">本次一共扣多少血</param>
    /// <param name="different">每次扣血的间隔时间</param>
    /// <returns></returns>
    public IEnumerator SlowDown(int different,int nowData,int newData,
                                int maxData,RectTransform dataRectTransform,int maxImageHeight,
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
            dataRectTransform.sizeDelta = new Vector2(width,maxImageHeight);

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnHpChanged(int oldValue,int newValue) {
        hpText.text = string.Format("{0}/{1}",newValue,BindingContext.maxHp.Value);
        //Debug.Log("新的HP是:"+newValue);
        // 计算当前血量和新血量的差值
        int different = Math.Abs(oldValue-newValue);

        // 播放扣血动画
        StartCoroutine(
            SlowDown(different:different,
                     nowData:oldValue,
                     newData:newValue,
                     maxData:BindingContext.maxHp.Value,
                     dataRectTransform:hpImage,
                     maxImageHeight:hpImageHeight,
                     maxImageWidth:hpImageMaxWidth)
       );
    }

    public void OnMpValueChanged(int oldValue,int newValue) {
        mpText.text = string.Format("{0}/{1}",newValue,BindingContext.maxMp.Value);

        // 计算当前Mp和新MP的差值
        int different = Math.Abs(oldValue - newValue);

        // 播放扣血动画
        StartCoroutine(
            SlowDown(different: different,
                     nowData: oldValue,
                     newData: newValue,
                     maxData: BindingContext.maxHp.Value,
                     dataRectTransform: hpImage,
                     maxImageHeight: hpImageHeight,
                     maxImageWidth: hpImageMaxWidth)
       );
    }
}

