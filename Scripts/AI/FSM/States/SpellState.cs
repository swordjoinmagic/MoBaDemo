using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 施法状态,
///     1.当释放的是原地释放型技能时,播放动画(首先重置动画状态),
///       直接释放该技能.
///     2.当释放的是指向型技能时,
///     
/// 
/// </summary>
public class SpellState : FSMState {

    public CharacterMono enemryMono;    
    public Transform enermyTransform;
    public CharacterMono spllerMono;       // 施放技能者
    public CharacterModel speller;

    public override void OnEnter() {
        enemryMono = BlackBorad.GetGameObject("Enemry").GetComponent<CharacterMono>();
        enermyTransform = BlackBorad.GetTransform("EnemryTransform");
        spllerMono = BlackBorad.GameObject.GetComponent<CharacterMono>();

        // 重置攻击状态
        spllerMono.ResetAttackStateAnimator();
    }

    public override void OnExit() {
        spllerMono.isPrepareUseSkill = false;
        //spllerMono.prepareSkill = null;

    }

    public override void OnUpdate() {
        // 如果施放技能状态结束,就自动回到Idle状态,为黑板设置变量
        if (enemryMono != null && spllerMono.Spell(enemryMono, enermyTransform)) {
            BlackBorad.SetBool("IsUseSkillFinish", true);
            BlackBorad.SetBool("isPrePareUseSkill", false);
        }
        // 如果目标单位不能被攻击,回到Idle状态
        if (!enemryMono.IsCanBeAttack()) {
            BlackBorad.SetBool("IsUseSkillFinish", true);
            BlackBorad.SetBool("isPrePareUseSkill", false);
        }
    }
}

