using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 状态切换型技能:
///     点击此技能会给目标单位附加一个状态,再次点击时,会将该状态从目标单位身上消除,即典型的状态切换型技能
/// </summary>
public class SwitchBattleStateSkill : ActiveSkill{
    // 表示此技能是否开启
    private bool isOpenState;
    public override bool IsMustDesignation {
        get {
            return false;
        }
    }

    //========================================================
    // 此技能开放的接口
    private BattleState additionalState;    // 开启此技能时附加的状态

    public BattleState AdditionalState {
        get {
            return additionalState;
        }

        set {
            additionalState = value;
        }
    }

    public override void Execute(CharacterMono speller, Vector3 position) {
        base.Execute(speller, speller.transform.position);

        if (!isOpenState) {
            additionalState.isFirstEnterState = true;
            speller.AddBattleState(AdditionalState);

            // 设置状态为开启状态
            isOpenState = true;
        } else {

            speller.RemoveBattleState(additionalState.Name);

            // 设置状态为关闭状态
            isOpenState = false;
        }
    }
}

