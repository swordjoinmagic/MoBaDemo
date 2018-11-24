using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleStatusListView : MonoBehaviour {
    public HeroMono heroMono;

    //=======================================
    // 此View管理的UI控件

    // 单个状态视图Prefab
    public BattleStatusView battleStatusPrefab;
    // 状态提示视图的Prefab
    public BattleStatusTipsView battleStatusTipsViewPrefab;

    private BattleStatusTipsView battleStatusTipsView = null;
    private Camera UICamera;
    private Canvas canvas;
    private Dictionary<BattleState, BattleStatusView> battleStatusViewDict = new Dictionary<BattleState, BattleStatusView>();

    /// <summary>
    /// 当单位的状态增加时,使用的函数
    /// </summary>
    private void OnAddBattleStatus(BattleState battleState) {
        BattleStatusView statusView = GameObject.Instantiate<BattleStatusView>(battleStatusPrefab, this.transform);
        battleStatusViewDict[battleState] = statusView;
        statusView.BattleState = battleState;

        // 鼠标进入事件
        EventTrigger.Entry onMouseEnter = new EventTrigger.Entry();
        onMouseEnter.eventID = EventTriggerType.PointerEnter;
        onMouseEnter.callback.AddListener((eventData) => {
            // 显示状态视图
            if (battleStatusTipsView == null) {
                battleStatusTipsView = GameObject.Instantiate<BattleStatusTipsView>(battleStatusTipsViewPrefab, canvas.transform);
            }

            // 设置提示窗口出现位置
            battleStatusTipsView.transform.SetParent(statusView.transform);
            (battleStatusTipsView.transform as RectTransform).anchoredPosition = new Vector2((statusView.transform as RectTransform).sizeDelta.x / 2, (statusView.transform as RectTransform).sizeDelta.y / 2);
            battleStatusTipsView.transform.SetParent(canvas.transform);

            battleStatusTipsView.Reveal(battleState);
        });
        // 鼠标离开事件
        EventTrigger.Entry onMouseExit = new EventTrigger.Entry();
        onMouseExit.eventID = EventTriggerType.PointerExit;
        onMouseExit.callback.AddListener((eventData) => {
            // 显示状态视图
            if (battleStatusTipsView == null) {
                return;
            }
            battleStatusTipsView.Hide();
        });

        EventTrigger eventTrigger = statusView.GetComponent<EventTrigger>();
        eventTrigger.triggers.Add(onMouseEnter);
        eventTrigger.triggers.Add(onMouseExit);
    }

    /// <summary>
    /// 移除某个状态时，触发的函数
    /// </summary>
    /// <param name="battleState"></param>
    private void OnRemoveBattleStatus(BattleState battleState) {
        // 第一步，判断状态提示视图是否不为空，且目前显示的状态是否是要移除的状态
        if (battleStatusTipsView != null) {
            if (battleState == battleStatusTipsView.BattleState) {
                // 将状态提示视图隐藏
                battleStatusTipsView.Hide();
            }
        }

        // 第二步，找到要被移除的状态的状态视图，从Dictionary中移除他，并Destroy他
        BattleStatusView view;
        battleStatusViewDict.TryGetValue(battleState,out view);
        if (view != null) {
            // 从dict中移除他
            battleStatusViewDict.Remove(battleState);
            // 销毁视图
            Destroy(view.gameObject);
        }
    }

    private void Bind() {
        heroMono.OnAddNewBattleStatus += OnAddBattleStatus;
        heroMono.OnRemoveBattleStatus += OnRemoveBattleStatus;
    }

    private void Start() {
        canvas = GameObject.FindObjectOfType<Canvas>();
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        Bind();
    }
}

