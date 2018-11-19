using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 所有战斗状态的基类，这里的状态指的不是FSM中的状态。
/// <para>这里的状态指的是战斗中被敌人施加的，持续性影响某一单位的状态，如：中毒状态</para>
/// </summary>
public class BattleState {
    public string name;
    public string description;
    public string iconPath;
    public float duration;
    public List<PassiveSkill> statePassiveSkills;
    public GameObject stateHolderEffect;
    public bool isStackable = false;      // 状态是否可叠加

    // 是否是第一次进入该状态
    private bool isFirstEnterState = true;

    // 第一次进入该状态时的时间
    private float firsstEnterTime;

    // 当前状态是否已经消失
    private bool isStateDying = false;

    public bool IsStateDying {
        get {
            return isStateDying;
        }
    }

    public float FirsstEnterTime {
        get {
            return firsstEnterTime;
        }
    }

    /// <summary>
    /// 战斗状态类总更新方法，自动更新状态的OnEnter、OnUpdate、OnExit状态，
    /// 自动处理状态的一系列流程
    /// </summary>
    /// <param name="stateHolder">状态持有者</param>
    public void Update(CharacterMono stateHolder) {

        // 处理进入状态的流程
        if (isFirstEnterState) {
            OnEnter(stateHolder);
            isFirstEnterState = false;
            return;
        }

        OnUpdate(stateHolder);

        // 处理状态消失流程
        if (Time.time - FirsstEnterTime >= duration) {
            OnExit(stateHolder);
        }
    }

    protected virtual void OnEnter(CharacterMono stateHolder) {
        firsstEnterTime = Time.time;

        if(stateHolderEffect!=null)
            GameObject.Instantiate(stateHolderEffect,stateHolder.transform.position,Quaternion.identity);
    }
    protected virtual void OnUpdate(CharacterMono stateHolder) {

    }
    protected virtual void OnExit(CharacterMono stateHolder) {
        isStateDying = true;
        // 状态消失时，自动将当前状态从单位的状态列表中去除
        stateHolder.RemoveBattleState(this);
    }
}

