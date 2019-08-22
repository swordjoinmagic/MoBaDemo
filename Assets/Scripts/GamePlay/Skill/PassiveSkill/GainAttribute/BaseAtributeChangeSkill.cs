using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseAtributeChangeSkill : PassiveSkill{
    // 设置为属性增益型技能
    public override PassiveSkillTriggerType TiggerType {
        get {
            return PassiveSkillTriggerType.GainAttribute;
        }
    }

    public BaseAtributeChangeSkill(SkillModel skillModel) : base(skillModel) { }

    //==========================================
    // 开放接口
    public bool IsScale {
        get {
            return (bool)skillModel.ExtraAttributes["IsScale"];
        }
    }
    public int Value {
        get {
            return (int)skillModel.ExtraAttributes["Value"];
        }
    }
    public CharacterAttribute Attribute {
        get {
            return (CharacterAttribute)skillModel.ExtraAttributes["Attribute"];
        }
    }

    public override void Execute(CharacterModel speller, out int result,out CharacterAttribute characterAttribute) {

        int attributeValue = 0;
        characterAttribute = Attribute;

        if (IsScale) {
            switch (Attribute) {
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
            result = attributeValue * Value;
        } else {
            result = Value;
        }
    }

    public override void Execute(CharacterModel speller, out float result,out CharacterAttribute characterAttribute) {
        
        float attributeValue = 0;
        characterAttribute = Attribute;

        if (IsScale) {
            switch (Attribute) {
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
            result = attributeValue * Value;
        } else {
            result = Value;
        }
    }
}
