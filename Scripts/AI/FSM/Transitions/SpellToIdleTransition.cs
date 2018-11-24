using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;

class SpellToIdleTransition : FSMTransition {

    CharacterMono target;
    Vector3 targetPosition;

    public override FSMState GetNextState() {
        return NextState;
    }

    public override bool IsValid() {
        CharacterMono characterMono = BlackBorad.CharacterMono;
        if (characterMono.prepareSkill.IsMustDesignation)
            target = BlackBorad.GetCharacterMono("Enemry");
        else
            targetPosition = BlackBorad.GetVector3("EnemryPosition");

        if ((characterMono.prepareSkill.IsMustDesignation&&target == null) || characterMono == null) return false;

        Vector3 position = target == null ? targetPosition : target.transform.position;

        if ((Input.GetKeyDown(KeyCode.Escape)||Input.GetMouseButtonDown(1)) &&
            Vector2.Distance(
                new Vector2(position.x,position.z),
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

