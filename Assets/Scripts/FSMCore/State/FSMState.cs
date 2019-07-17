using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSM {
    public abstract class FSMState {
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();

        public void SetTransitionsBlackBorad() {
            // 为每一个transition都设置黑板
            foreach (var transition in transitions) {
                transition.BlackBorad = this.blackBorad;
            }
        }

        public List<FSMTransition> transitions;
        private BlackBorad blackBorad;       // 黑板

        public BlackBorad BlackBorad {
            get {
                return blackBorad;
            }

            set {
                blackBorad = value;
            }
        }
    }
}
