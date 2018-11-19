using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 中毒状态
/// </summary>
public class PoisoningState : BattleState{

    //============================================
    // 提供给外部调用的接口
    public Damage damage = Damage.Zero;     // 中毒时每间隔1秒受到的伤害

    private Damage nowDamage = Damage.Zero;

    protected override void OnUpdate(CharacterMono stateHolder) {
        base.OnUpdate(stateHolder);

        nowDamage += damage * Time.smoothDeltaTime;
        if (nowDamage.TotalDamage >= 1) {
            stateHolder.characterModel.Damaged(nowDamage);
            nowDamage = Damage.Zero;
        }
    }
}

