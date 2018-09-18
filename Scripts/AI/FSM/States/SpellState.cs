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

    public Animator animator;
    public CharacterMono enemryMono;    
    public CharacterModel enemryModel;
    public NavMeshAgent agent;
    public Transform enermyTransform;
    public bool isImmediatelySpell;
    public CharacterMono spellerMono;       // 施放技能者
    public CharacterModel speller;
    public bool isStartSpell = false;       // 是否准备释放法术


    public override void OnEnter() {
        animator = BlackBorad.Animator;
        enemryMono = BlackBorad.GetGameObject("Enemry").GetComponent<CharacterMono>();
        enemryModel = enemryMono.characterModel;
        enermyTransform = BlackBorad.GetTransform("EnemryTransform");
        agent = BlackBorad.Agent;
        spellerMono = BlackBorad.GameObject.GetComponent<CharacterMono>();
        speller = spellerMono.characterModel;

        // 是否是原地释放技能
        isImmediatelySpell = BlackBorad.GetBool("isImmediatelySpell");

        // 如果是指向型技能,那么预先移动过去
        if (!isImmediatelySpell) {
            agent.SetDestination(enermyTransform.position);
        }

        // 重置状态
        animator.ResetTrigger("attack");
    }

    public override void OnExit() {
        
    }

    public override void OnUpdate() {

        // 获得当前动画和下一个动画状态
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        if (isImmediatelySpell) {
            // 原地释放技能,此时直接释放技能

            // 播放释放技能的动画
            if(!currentAnimatorStateInfo.IsName("Spell"))
                animator.SetTrigger("spell");

            // 如果技能释放结束,那么产生特效,计算伤害
            if (currentAnimatorStateInfo.IsName("Spell") && 
                nextAnimatorStateInfo.IsName("Idle")) {

                // 施放技能状态结束,自动回到Idle状态,为黑板设置变量
                // IsUseSkillFinish为true
                BlackBorad.SetBool("IsUseSkillFinish",true);
                BlackBorad.SetBool("isPrePareUseSkill", false);
                spellerMono.isPrepareUseSkill = false;
            }
        } else {
            // 指向型技能

            PointingSkill pointingSkill = spellerMono.prepareSkill as PointingSkill;

            // 当前距离敌人 > 施法距离,进行移动
            if (!agent.pathPending && agent.remainingDistance < spellerMono.prepareSkill.SpellDistance) {
                //Debug.Log("运动就结束，设置animator为false");
                animator.SetBool("isRun", false);
                agent.isStopped = true;
                isStartSpell = true;
            } else {
                isStartSpell = false;
                animator.SetBool("isRun", true);
                agent.isStopped = false;
                agent.SetDestination(enermyTransform.position);
            }

            //======================================
            // 播放施法动画
            // 如果准备开始施法,那么播放动画
            if (isStartSpell && !currentAnimatorStateInfo.IsName("Spell")) {
                animator.SetTrigger("spell");
                isStartSpell = false;
            }

            // 如果技能释放结束,那么产生特效,计算伤害
            if (currentAnimatorStateInfo.IsName("Spell") &&
                nextAnimatorStateInfo.IsName("Idle")) {

                pointingSkill.target = enemryMono.gameObject;
                pointingSkill.targetEffect = BlackBorad.GetGameObject("targetEffect");
                
                Debug.Log("targetEffect:"+pointingSkill.targetEffect);
                Damage damage = pointingSkill.Execute();
                enemryModel.Hp -= damage.TotalDamage;
                enemryMono.SimpleCharacterViewModel.Modify(enemryModel);

                Debug.Log("释放技能");

                // 施放技能状态结束,自动回到Idle状态,为黑板设置变量
                // IsUseSkillFinish为true
                BlackBorad.SetBool("IsUseSkillFinish", true);
                BlackBorad.SetBool("isPrePareUseSkill", false);
                spellerMono.isPrepareUseSkill = false;
            }
        }

    }
}

