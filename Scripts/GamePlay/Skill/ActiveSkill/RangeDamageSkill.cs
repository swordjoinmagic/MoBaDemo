using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RangeDamageSkill : ActiveSkill{

    public Damage damage = Damage.Zero;


    public override void Execute(CharacterMono speller, Vector3 position) {
        Debug.Log("(0.03f*(skillInfluenceRadius/0.5f)):"+ (SkillInfluenceRadius - (0.12f * (skillInfluenceRadius / 0.5f))));
        Collider[] targetList = Physics.OverlapSphere(position,SkillInfluenceRadius - (0.12f*(skillInfluenceRadius/0.5f)));
        foreach (var collider in targetList) {
            CharacterMono characterMono = collider.GetComponent<CharacterMono>();
            if (characterMono != null) {
                characterMono.characterModel.Damaged(damage);
            }
        }
    }
}
