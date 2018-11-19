using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleStatusListView : MonoBehaviour{
    public HeroMono heroMono;

    //=======================================
    // 此View管理的UI控件

    // 单个状态视图Prefab
    public BattleStatusView battleStatusPrefab;

    /// <summary>
    /// 当单位的状态增加时,使用的函数
    /// </summary>
    private void OnAddBattleStatus(BattleState battleState) {
        BattleStatusView statusView = GameObject.Instantiate<BattleStatusView>(battleStatusPrefab,this.transform);
        statusView.BattleState = battleState;
    }

    private void Bind() {
        heroMono.OnAddNewBattleStatus += OnAddBattleStatus;
    }

    private void Start() {
        Bind();
    }
}

