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

    public override void Execute(CharacterModel speller, out int result,out CharacterAttribute characterAttribute) {

        int attributeValue = 0;
        characterAttribute = attribute;

        if (isScale) {
            switch (attribute) {
                case CharacterAttribute.Attack:
                    attributeValue = speller.Attack;
                    break;
                case CharacterAttribute.Defense:
                    attributeValue = speller.Defense;
                    break;
                case CharacterAttribute.MaxHP:
                    attributeValue = speller.maxHp;
                    break;
                case CharacterAttribute.MaxMp:
                    attributeValue = speller.maxMp;
                    break;
            }
            result = attributeValue * value;
        } else {
            result = value;
        }
    }

    public override void Execute(CharacterModel speller, out float result,out CharacterAttribute characterAttribute) {
        
        float attributeValue = 0;
        characterAttribute = attribute;

        if (isScale) {
            switch (attribute) {
                case CharacterAttribute.AttackDistance:
                    attributeValue = speller.attackDistance;
                    break;
                case CharacterAttribute.AttackSpeed:
                    attributeValue = speller.AttackSpeed;
                    break;
                case CharacterAttribute.DodgeRate:
                    attributeValue = speller.DodgeRate;
                    break;
                case CharacterAttribute.MagicalResistance:
                    attributeValue = speller.MagicalResistance;
                    break;
                case CharacterAttribute.PhysicalResistance:
                    attributeValue = speller.PhysicalResistance;
                    break;
                case CharacterAttribute.movingSpeed:
                    attributeValue = speller.MovingSpeed;
                    break;
            }
            result = attributeValue * value;
        } else {
            result = value;
        }
    }
}
