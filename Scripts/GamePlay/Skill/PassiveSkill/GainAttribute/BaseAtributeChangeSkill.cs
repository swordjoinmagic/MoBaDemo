using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseAtributeChangeSkill : PassiveSkill{

    //==========================================
    // 开放接口
    public bool isScale = false;
    public int value = 0;
    public CharacterAttribute attribute = CharacterAttribute.Attack;

    // 技能描述
    public override string TargetDescription {
        get {
            return base.TargetDescription;
        }
    }
    // 设置为属性增益型技能
    public override PassiveSkillTriggerType TiggerType {
        get {
            return PassiveSkillTriggerType.GainAttribute;
        }
    }

    public override void Execute(CharacterModel speller, out int result,CharacterAttribute characterAttribute,int attributeValue) {
        if (attribute == characterAttribute) {
            if (isScale)
                result = attributeValue * value;
            else
                result = value;
        } else
            result = 0;
        Debug.Log("此被动技能增加的值是："+result);
    }

}
