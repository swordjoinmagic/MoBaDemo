using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 负责显示单位身上状态的视图
/// </summary>
public class BattleStatusView : MonoBehaviour{
    BattleState battleState;

    //=======================================
    // 此View管理的UI空间
    public Image icon;          // 状态图标
    public Image coolDownBar;   // 状态

    public BattleState BattleState {
        get {
            return battleState;
        }

        set {
            battleState = value;
            icon.sprite = Resources.Load("UIImage/" + battleState.IconPath, typeof(Sprite)) as Sprite;
        }
    }

    private void Update() {
        // -1表示此状态的持续时间为永久
        if(battleState != null && battleState.Duration != -1)
            coolDownBar.fillAmount = 1f - ((Time.time - battleState.FirsstEnterTime) / battleState.Duration);
    }

}

