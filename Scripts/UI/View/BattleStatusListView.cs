using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleStatusListView : MonoBehaviour{
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

    /// <summary>
    /// 当单位的状态增加时,使用的函数
    /// </summary>
    private void OnAddBattleStatus(BattleState battleState) {

        // 因为战斗状态视图的生命周期由它所表示的状态决定(也就是说状态出现,视图出现,状态消失,视图销毁)
        // 所以这里不将状态视图添加进列表进行管理
        BattleStatusView statusView = GameObject.Instantiate<BattleStatusView>(battleStatusPrefab,this.transform);
        statusView.BattleState = battleState;

        // 鼠标进入事件
        EventTrigger.Entry onMouseEnter = new EventTrigger.Entry();
        onMouseEnter.eventID = EventTriggerType.PointerEnter;
        onMouseEnter.callback.AddListener((eventData)=> {
            // 显示状态视图
            if (battleStatusTipsView==null) {
                battleStatusTipsView = GameObject.Instantiate<BattleStatusTipsView>(battleStatusTipsViewPrefab,canvas.transform);
            }
            Vector3 vector3 = UICamera.ScreenToWorldPoint(Input.mousePosition);
            vector3.z = 100;
            // 设置提示窗口出现位置
            battleStatusTipsView.transform.position = vector3;

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

    private void Bind() {
        heroMono.OnAddNewBattleStatus += OnAddBattleStatus;
    }

    private void Start() {
        canvas = GameObject.FindObjectOfType<Canvas>();
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        Bind();
    }
}

