using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Reflection;

public class TestDatabase : MonoBehaviour {

    public CharacterMono characterMono;

    private void Awake() {
        DataManager.Instance.LoadData();

        //Debug.Log((DataManager.Instance.baseSkillDataSet[0] as ActiveSkill).skillName);

        characterMono.characterModel.activeSkills.Add(
            DataManager.Instance.baseSkillDataSet[0] as ActiveSkill
        );
        characterMono.characterModel.passiveSkills.Add(
            DataManager.Instance.baseSkillDataSet[1] as PassiveSkill    
        );
    }

}
