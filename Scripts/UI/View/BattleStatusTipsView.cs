using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 状态提示的小窗口
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class BattleStatusTipsView : MonoBehaviour{
    //==========================
    // 此View管理的UI控件
    public Text statusNameText;
    public Text statusDescriptionText;
    private CanvasGroup canvasGroup;
    private EventTrigger eventTrigger;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        eventTrigger = GetComponent<EventTrigger>();
    }

    /// <summary>
    /// 显示该窗口的方法
    /// </summary>
    public void Reveal(BattleState battleState) {
        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1;

        Init(battleState);
    }

    public void Hide() {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 根据指定状态来改变状态窗口的UI信息
    /// </summary>
    private void Init(BattleState battleState) {
        statusNameText.text = battleState.name;
        statusDescriptionText.text = battleState.description;
    }
}

