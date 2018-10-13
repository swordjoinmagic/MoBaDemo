using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

class SpellToIdleTransition : FSMTransition {
    public override FSMState GetNextState() {
        return NextState;
    }

    public override bool IsValid() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            BlackBorad.SetBool("IsUseSkillFinish", true);
            BlackBorad.SetBool("isPrePareUseSkill", false);
            BlackBorad.GameObject.GetComponent<CharacterMono>().isPrepareUseSkill = false;
            return true;
        }
        return false;
    }

    public override void OnTransition() {
        
    }
}

