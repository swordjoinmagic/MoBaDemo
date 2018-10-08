using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;

class IsMoveOverTransition : FSMTransition {
    public override FSMState GetNextState() {
        return NextState;
    }

    public override bool IsValid() {
        if (BlackBorad.GetBool("IsMoveOver")) {
            return true;
        } else {
            return false;
        }
    }

    public override void OnTransition() {
        
    }
}
