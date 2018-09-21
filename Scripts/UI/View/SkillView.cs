using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class SkillView : MonoBehaviour{
    public CharacterMono characterMono;
    private CharacterModel character;
    private List<ActiveSkill> activeSkills;

    //===============================
    // UI控件
    public List<Image> images;

    private void Update() {

        character = characterMono.characterModel;
        activeSkills = character.activeSkills;

        for (int i=0;i<activeSkills.Count;i++) {

            ActiveSkill activeSkill = activeSkills[i];
            print(activeSkill);
            float coolDown = activeSkill.CD;
            float finalSpellTime = activeSkill.FinalSpellTime;

            float different = Time.time - finalSpellTime;
            //print("Skill Different:"+different);
            float rate = 1 - Mathf.Clamp01(different/coolDown);
            images[i].fillAmount = rate;
        }
    }
}

