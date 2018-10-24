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
        CharacterMono target = BlackBorad.GetCharacterMono("Enemry");
        CharacterMono characterMono = BlackBorad.CharacterMono;
        if (target == null || characterMono == null) return false;
        if (Input.GetKeyDown(KeyCode.Escape) &&
            Vector2.Distance(
                new Vector2(target.transform.position.x, target.transform.position.z),
                new Vector2(characterMono.transform.position.x, characterMono.transform.position.z)
                ) > characterMono.prepareSkill.SpellDistance
            ) {
            Debug.Log("单位按下ESC键,并且两个单位相距距离超过技能释放距离");
            BlackBorad.GameObject.GetComponent<CharacterMono>().isPrepareUseSkill = false;
            return true;
        }
        return false;
    }

    public override void OnTransition() {
        
    }
}

