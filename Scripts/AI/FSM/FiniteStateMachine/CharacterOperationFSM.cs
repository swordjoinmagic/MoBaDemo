using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 进行人物操作的FSM
/// </summary>
class CharacterOperationFSM : FiniteStateMachine{

    public Animator animator;
    public NavMeshAgent agent;

    // 移动特效
    public GameObject MoveEffect;
    public GameObject ExplosionEffect;

    public void InitBlackBoard() {
        blackBorad.Animator = animator;
        blackBorad.Agent = agent;
        blackBorad.GameObject = gameObject;
        blackBorad.SetGameObject("MoveEffect",MoveEffect);
        blackBorad.SetBool("isPrePareUseSkill", false);
        blackBorad.SetBool("isImmediatelySpell", false);
        blackBorad.SetBool("IsUseSkillFinish",false);
        blackBorad.SetBool("IsMoveOver", true);
        blackBorad.SetGameObject("targetEffect", ExplosionEffect);

        Debug.Log("初始化黑板:"+" 黑板的Aniamtor:"+blackBorad.Animator);
    }

    /// <summary>
    /// 新建状态并设置Transition
    /// </summary>
    public void SetState() {

        //=================================================================================
        // 设置Transition
        IsClickedEnermyTransition isClickedEnermyTransition = new IsClickedEnermyTransition();
        IsClickedMoveTransition isClickedMoveTransition = new IsClickedMoveTransition();
        IsSpellFinishTransition isSpellFinishTransition = new IsSpellFinishTransition();
        IsMoveOverTransition isMoveOverTransition = new IsMoveOverTransition();
        CharacterOperationStateToSpellTransition characterOperationStateToSpellTransition = new CharacterOperationStateToSpellTransition();
        AttackToIdleTransition attackToIdleTransition = new AttackToIdleTransition();
        SpellToIdleTransition spellToIdleTransition = new SpellToIdleTransition();
        IdleToAttackTransition idleToAttackTransition = new IdleToAttackTransition();

        //=================================================================================
        // 设置State
        AttackState attackState = new AttackState() {
            transitions = new List<FSMTransition>() {
                isClickedEnermyTransition,
                isClickedMoveTransition,
                attackToIdleTransition
            }
        };
        IdleState idleState = new IdleState {
            transitions = new List<FSMTransition> {
                isClickedEnermyTransition,
                isClickedMoveTransition,
                idleToAttackTransition
            }
        };
        MoveState moveState = new MoveState() {
            transitions = new List<FSMTransition>() {
                isClickedEnermyTransition,
                isClickedMoveTransition,
                isMoveOverTransition
            }
        };
        SpellState spellState = new SpellState() {
            transitions = new List<FSMTransition>() {
                isSpellFinishTransition,
                spellToIdleTransition,
            }
        };
        CharacterOperationExtraState anyExtraState = new CharacterOperationExtraState() {
            transitions = new List<FSMTransition>() {
                characterOperationStateToSpellTransition
            }
        };

        //===================================
        // 设置Transition的nextState
        isClickedMoveTransition.NextState = moveState;
        isClickedEnermyTransition.NextState = attackState;
        characterOperationStateToSpellTransition.NextState = spellState;
        isSpellFinishTransition.NextState = idleState;
        isMoveOverTransition.NextState = idleState;
        attackToIdleTransition.NextState = idleState;
        spellToIdleTransition.NextState = idleState;
        idleToAttackTransition.NextState = attackState;

        //=======================================
        // 设置初始状态
        initialState = idleState;
        activeState = initialState;

        // 设置状态列表
        states = new List<FSMState> {
            idleState,moveState,attackState,spellState,anyExtraState
        };

        //===========================
        // 为所有状态及Transition设置黑板
        foreach (var state in states) {
            state.BlackBorad = blackBorad;
            foreach (var transition in state.transitions) {
                transition.BlackBorad = blackBorad;
            }
        }

        // 设置anyState
        anyState = anyExtraState;
    }


    public override void OnStart() {

        debug = true;

        // 初始化黑板
        InitBlackBoard();

        // 对所有状态进行设置
        SetState();
    }

    public void Start() {

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        OnStart();
    }

    public void Update() {
        OnUpdate();
    }
}

