using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSM {
    public abstract class FSMTransition {
        public abstract bool IsValid();
        public abstract void OnTransition();
        public abstract FSMState GetNextState();

        private FSMState nextState;         // 下一个状态
        private BlackBorad blackBorad;       // 黑板

        public BlackBorad BlackBorad {
            get {
                return blackBorad;
            }

            set {
                blackBorad = value;
            }
        }

        public FSMState NextState {
            get {
                return nextState;
            }

            set {
                nextState = value;
            }
        }
    }
}
