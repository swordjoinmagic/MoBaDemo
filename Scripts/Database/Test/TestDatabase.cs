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
        //characterMono.characterModel.passiveSkills.Add(
        //    DataManager.Instance.baseSkillDataSet[1] as PassiveSkill
        //);
        characterMono.characterModel.passiveSkills.Add(
            new BaseAtributeChangeSkill {
                attribute = CharacterAttribute.Attack,
                value = 5,
                triggerType = PassiveSkillTriggerType.GainAttribute,
                isScale = true,
                SkillName = "属性增益型技能"
            }    
        );
    }

}
