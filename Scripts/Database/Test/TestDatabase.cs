using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Reflection;

public class TestDatabase : MonoBehaviour {

    public List<BaseSkill> baseSkills;
    public List<Item> items;
    public List<CharacterModel> characterModels;
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
                mp = 1000
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
            },
            new ChainSkill {
                BaseDamage = 1000,
                KeyCode = KeyCode.P,
                Mp = 220,
                PlusDamage = 200,
                SpellDistance = 4f,
                CD = 5f,
                Count = 4,
                Damage = new Damage { BaseDamage = -1000, PlusDamage = -1000 },
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
            },
            new ContinuousRangeSkillGroup {
                SkillID = 3,
                KeyCode = KeyCode.Y,
                PlusDamage = 200,
                TargetEffect = targetPositionEffect,
                SpellDistance = 10f,
                SkillName = "E技能",
                IconPath = "00041",
                SkillLevel = 6,
                SkillInfluenceRadius = 6f,
                activeSkills = new ActiveSkill[]{
                            new PointingSkill{
                                PlusDamage = 100
                            },
                            new AdditionalStateSkill{
                                SpellDistance = 10,
                                AdditionalState = new PoisoningState{
                                    stateHolderEffect = targetEnemryEffect,
                                    Duration = 15,
                                    Damage = new Damage{ PlusDamage = 100 },
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
                            },
                           new SkillDelayAttribute{
                                isDelay = false,
                                index = -1,
                            }
                        },
                SkillTargetType = UnitType.Everything,
                SpellDuration = 3,
            }
        };
        items = new List<Item>() {
            new Item {
                name = "测试物品",
                itemActiveSkill = new PointingSkill {
                    BaseDamage = 1000,
                    SelfEffect = targetPositionEffect,
                    TargetEffect = targetPositionEffect,
                    SpellDistance = 10,
                    CD = 3,
                    SkillTargetType = UnitType.Everything,
                    SkillLevel = 1,
                },
                itemType = ItemType.Consumed,
                maxCount = 10,
                iconPath = "00046",
                useMethodDescription = "使用：点目标",
                activeDescription = "对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害",
                passiveDescription = "+100攻击力\n+100防御力\n+10力量",
                backgroundDescription = "一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品"
            }
        };
        characterModels = new List<CharacterModel> {
            new HeroModel {
                //projectileModel = new ProjectileModel {
                //    spherInfluence = 5,
                //    targetPositionEffect = targetPositionEffect,
                //    movingSpeed = 5
                //},
                maxHp = 10000,
                Hp = 200,
                maxMp = 1000,
                Mp = 1000,
                Name = "sjm",
                attackDistance = 10f,
                Level = 0,
                MaxLevel = 10,
                ForcePower = 100,
                NeedExp = 1000,
                Attack = 100,
                AttackFloatingValue = 99,
                Exp = 1,
                Expfactor = 2,
                AvatarImagePath = "PlayerAvatarImage",
                AgilePower = 20,
                IntelligencePower = 10,
                MainAttribute = HeroMainAttribute.AGI,
                SkillPointGrowthPoint = 1,
                TurningSpeed = 50,
                AttackAudioPath = "attackAudio",
                Radius = 20,
                MovingSpeed = 4,
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

    private void Start() {
    }
}
