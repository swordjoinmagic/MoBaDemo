using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Reflection;

public class TestDatabase : MonoBehaviour {

    public List<BaseSkill> baseSkills;
    public GameObject targetPositionEffect;
    public GameObject targetEnemryEffect;

    private void Awake() {
        baseSkills = new List<BaseSkill>() {
            new SwitchBattleStateSkill {
                SkillID = 0,
                BaseDamage = 1000,
                KeyCode = KeyCode.T,
                Mp = 220,
                PlusDamage = 200,
                CD = 5f,
                SpellDistance = 0,
                SkillName = "T技能",
                IconPath = "00041",
                SkillLevel = 6,
                TargetEffect = targetPositionEffect,
                SkillTargetType = UnitType.Everything,
                AdditionalState = new BattleState {
                    Description = "anohter",
                    stateHolderEffect = targetEnemryEffect,
                    Duration = -1,
                    IconPath = "0041",
                    Name = "anohter",
                    IsStackable = false,
                    statePassiveSkills = new List<PassiveSkill>{
                                new BaseAtributeChangeSkill{
                                    attribute = CharacterAttribute.Attack,
                                    value = 500,
                                }
                            }
                }
            },
            new RangeSkillGroup {
                SkillID = 1,
                KeyCode = KeyCode.F,
                PlusDamage = 200,
                TargetEffect = targetPositionEffect,
                SpellDistance = 10f,
                SkillName = "F技能",
                IconPath = "00041",
                SkillLevel = 6,
                SkillInfluenceRadius = 6f,
                activeSkills = new ActiveSkill[]{
                            new AdditionalStateSkill{
                                SpellDistance = 10,
                                AdditionalState = new PoisoningState{
                                    Description = "范围中毒技能",
                                    stateHolderEffect = targetEnemryEffect,
                                    Duration = 15,
                                    IconPath = "0041",
                                    Damage = new Damage{ PlusDamage = 100 },
                                    Name = "中毒",
                                    IsStackable = false,
                                    statePassiveSkills = new List<PassiveSkill>{
                                        new BaseAtributeChangeSkill{
                                            attribute = CharacterAttribute.Attack,
                                            value = 10,
                                            isScale = true
                                        }
                                    }
                                },
                                SkillTargetType = UnitType.Everything
                            }
                        },
                skillDelayAttributes = new SkillDelayAttribute[] {
                            new SkillDelayAttribute{
                                isDelay = false,
                                index = -1,
                            }
                        },
                SkillTargetType = UnitType.Everything,
            },
            new TransformSkill {
                SkillID = 2,
                KeyCode = KeyCode.W,
                SkillLevel = 1,
                SkillTargetType = UnitType.Everything,
                SkillName = "闪现",
                IconPath = "00046",
                Mp = 10,
                PlusDamage = 200,
                SpellDistance = 15f,
                CD = 2f,
                LongDescription = "one skill Description",
                TargetEffect = targetEnemryEffect,
                SelfEffect = targetPositionEffect
            },
            new RangeSkillGroup {
                SkillID = 3,
                KeyCode = KeyCode.E,
                PlusDamage = 200,
                TargetEffect = targetPositionEffect,
                SpellDistance = 10f,
                SkillName = "E技能",
                IconPath = "00041",
                SkillLevel = 6,
                SkillInfluenceRadius = 6f,
                activeSkills = new ActiveSkill[]{
                            new DisperseStateSkill{
                                SpellDistance = 10,
                                BattleStateType = BattleStateType.PoisoningState,
                                SkillTargetType = UnitType.Everything
                            }
                        },
                skillDelayAttributes = new SkillDelayAttribute[] {
                            new SkillDelayAttribute{
                                isDelay = false,
                                index = -1,
                            }
                        },
                SkillTargetType = UnitType.Everything,
            },
            new AdditionalActiveSkill {
                SkillID = 4,
                TiggerType = PassiveSkillTriggerType.WhenAttack,
                CD = 0.1f,
                IconPath = "0041",
                SkillName = "AdditionActiveSkil",
                SkillTargetType = UnitType.Everything,
                SkillLevel = 1,
                additionalActiveSkill = new ChainSkill {
                    BaseDamage = 1000,
                    KeyCode = KeyCode.P,
                    Mp = 220,
                    PlusDamage = 200,
                    SpellDistance = 4f,
                    CD = 5f,
                    Count = 4,
                    Damage = new Damage { PlusDamage = 500 },
                    SkillName = "W技能",
                    IconPath = "00041",
                    LongDescription = "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化," +
                        "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
                        "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
                        "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化",
                    SkillLevel = 6,
                    TargetEffect = targetEnemryEffect,
                    SkillTargetType = UnitType.Everything,
                    SkillInfluenceRadius = 10
                }
            }
        };
    }

    private static TestDatabase instance;
    public static TestDatabase Instance{
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<TestDatabase>();
            }
            return instance;
        }
    }
    
}
