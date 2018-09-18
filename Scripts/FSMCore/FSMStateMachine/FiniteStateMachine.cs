using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FSM {
    public class FiniteStateMachine : MonoBehaviour{

        // 所有状态列表
        public List<FSMState> states = new List<FSMState>();

        // 初始状态
        public FSMState initialState;

        // 当前活动状态
        public FSMState activeState;

        // 任何时候都会进入的状态
        public FSMState anyState;

        // 黑板
        public BlackBorad blackBorad = new BlackBorad();

        // 是否开启Debug模式
        public bool debug = false;
        

        // 开始时进行的操作
        public virtual void OnStart() {

        }

        public virtual void OnUpdate() {

            if (debug) DebugWithFSM();

            //===================================
            // 更新Transition

            // 更新anyState的Transition
            if (anyState != null)
                foreach (var transition in anyState.transitions) {
                    if (transition.IsValid()) {
                        activeState.OnExit();
                        activeState = transition.GetNextState();
                        activeState.OnEnter();
                        return;
                    }
                }

            // 遍历当前活动状态的所有转换条件,进行状态转换
            foreach (var transition in activeState.transitions) {
                if (transition.IsValid()) {
                    activeState.OnExit();
                    activeState = transition.GetNextState();
                    activeState.OnEnter();
                    return;
                }
            }


            //===================================
            // 更新State

            // 更新anyState
            if (anyState != null)
                anyState.OnUpdate();

            // 没有状态可以转换的时候,执行当前状态要进行的事件
            activeState.OnUpdate();

        }


        public void DebugWithFSM() {
            Debug.Log("当前State是:"+this.activeState);
        }
    }
}
