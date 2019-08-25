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
            new RangeSkillGroup(new SkillModel(new Tuple<string, object>[]{
                new Tuple<string, object>{
                    First = "ActiveSkills",
                    Second = new ActiveSkill[]{
                            new AdditionalStateSkill(new SkillModel(new Tuple<string, object>[]{
                                new Tuple<string, object>{
                                    First = "AdditionalState",
                                    Second = new PoisoningState{
                                        Description = "范围中毒技能",
                                        stateHolderEffect = targetEnemryEffect,
                                        Duration = 15,
                                        IconPath = "0041",
                                        Damage = new Damage{ PlusDamage = 100 },
                                        Name = "中毒",
                                        IsStackable = false,
                                        statePassiveSkills = new List<PassiveSkill>{
                                            new BaseAtributeChangeSkill(new SkillModel(new Tuple<string, object>[]{
                                                new Tuple<string, object>{ First = "Attribute",Second=CharacterAttribute.Attack },
                                                new Tuple<string, object>{ First = "Value",Second = 10 },
                                                new Tuple<string, object>{ First = "IsScale",Second = true}
                                            }))
                                    }
                                },
                                }
                            }){
                                SpellDistance = 10,
                                SkillTargetType = UnitType.Everything
                            })
                        }
                },
                // SkillDelayAttributes
                new Tuple<string, object>{
                    First = "SkillDelayAttributes",
                    Second = new SkillDelayAttribute[] {
                                new SkillDelayAttribute{
                                    isDelay = false,
                                    index = -1,
                                }
                            }
                }
            }){
                KeyCode = KeyCode.F,
                TargetEffect = targetPositionEffect,
                SpellDistance = 10f,
                SkillName = "F技能",
                IconPath = "00041",
                SkillInfluenceRadius = 6f,
                SkillTargetType = UnitType.Everything,
                Mp = 1000
            }){ SkillLevel = 1},
            new TransformSkill(new SkillModel(){
                KeyCode = KeyCode.W,
                SkillTargetType = UnitType.Everything,
                SkillName = "闪现",
                IconPath = "00046",
                Mp = 10,
                SpellDistance = 15f,
                Cooldown = 2f,
                LongDescription = "one skill Description",
                TargetEffect = targetEnemryEffect,
                SelfEffect = targetPositionEffect
            }){ SkillLevel = 1},
            new RangeSkillGroup(new SkillModel(new Tuple<string, object>[]{
                new Tuple<string, object>{
                    First = "ActiveSkills",
                    Second = new ActiveSkill[]{
                                new DisperseStateSkill(new SkillModel(new Tuple<string, object>[]{
                                    new Tuple<string, object>{ First="SpellDistance",Second=10 },
                                    new Tuple<string, object>{ First="BattleStateType",Second=BattleStateType.PoisoningState },
                                    new Tuple<string, object>{ First="SkillTargetType",Second=UnitType.Everything }
                                }))
                            }
                },
                new Tuple<string, object>{
                    First = "SkillDelayAttributes",
                    Second = new SkillDelayAttribute[] {
                            new SkillDelayAttribute{
                                isDelay = false,
                                index = -1,
                            }
                        }
                }
            }){
                KeyCode = KeyCode.E,
                TargetEffect = targetPositionEffect,
                SpellDistance = 10f,
                SkillName = "E技能",
                IconPath = "00041",
                SkillInfluenceRadius = 6f,
                SkillTargetType = UnitType.Everything,
            }){ SkillLevel = 1},
            new AdditionalActiveSkill(new SkillModel(new Tuple<string, object>[]{
                new Tuple<string, object>{
                    First ="AdditionalActiveSkill",
                    Second = new ChainSkill(new SkillModel(new Tuple<string, object>[]{
                                new Tuple<string, object>{ First="Count",Second=4 },
                                new Tuple<string, object>{ First="Damage",Second=new Damage { PlusDamage = 500 } }
                            }){
                                KeyCode = KeyCode.P,
                                Mp = 220,
                                SpellDistance = 4f,
                                Cooldown = 5f,
                                SkillName = "W技能",
                                IconPath = "00041",
                                LongDescription = "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化," +
                                    "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
                                    "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
                                    "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化",
                                TargetEffect = targetEnemryEffect,
                                SkillTargetType = UnitType.Everything,
                                SkillInfluenceRadius = 10
                            })
                    }
            }){
                Cooldown = 0.1f,
                IconPath = "0041",
                SkillName = "AdditionActiveSkil",
                SkillTargetType = UnitType.Everything,
            }) {
                TiggerType = PassiveSkillTriggerType.WhenAttack,
                SkillLevel = 1
            },
            new ChainSkill(new SkillModel(new Tuple<string, object>[]{
                new Tuple<string, object>{ First="Count",Second=4 },
                new Tuple<string, object>{ First="Damage",Second=new Damage { BaseDamage = -1000, PlusDamage = -1000 } }
            }){
                KeyCode = KeyCode.P,
                Mp = 220,
                SpellDistance = 4f,
                Cooldown = 5f,
                SkillName = "W技能",
                IconPath = "00041",
                LongDescription = "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化," +
                        "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
                        "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
                        "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化",
                TargetEffect = targetEnemryEffect,
                SkillTargetType = UnitType.Everything,
                SkillInfluenceRadius = 10
            }){ SkillLevel = 1},
            new ContinuousRangeSkillGroup(new SkillModel(new Tuple<string, object>[]{
                new Tuple<string, object>{
                    First = "ActiveSkills",
                    Second = new ActiveSkill[]{
                            new PointingSkill(new SkillModel(new Tuple<string, object>[]{
                                new Tuple<string, object>{ First="Damage",Second=new Damage(100,200) }
                            })),
                            new AdditionalStateSkill(new SkillModel(new Tuple<string, object>[]{
                                new Tuple<string, object>{
                                    First = "AdditionalState",
                                    Second = new PoisoningState{
                                                stateHolderEffect = targetEnemryEffect,
                                                Duration = 15,
                                                Damage = new Damage{ PlusDamage = 100 },
                                                IsStackable = false,
                                                statePassiveSkills = new List<PassiveSkill>{
                                                    new BaseAtributeChangeSkill(new SkillModel(new Tuple<string, object>[]{
                                                        new Tuple<string, object>{ First="Attribute",Second=CharacterAttribute.Attack },
                                                        new Tuple<string, object>{ First="Value",Second=10 },
                                                        new Tuple<string, object>{ First="IsScale",Second=true }
                                                    }))
                                                }
                                            }
                                }
                            }){
                                SpellDistance = 10,
                                SkillTargetType = UnitType.Everything
                            })
                        },
                },
                new Tuple<string, object>{
                    First = "SkillDelayAttributes",
                    Second = new SkillDelayAttribute[] {
                            new SkillDelayAttribute{
                                isDelay = false,
                                index = -1,
                            },
                           new SkillDelayAttribute{
                                isDelay = false,
                                index = -1,
                            }
                        }
                }
            }){
                KeyCode = KeyCode.Y,
                TargetEffect = targetPositionEffect,
                SpellDistance = 10f,
                SkillName = "E技能",
                IconPath = "00041",
                SkillInfluenceRadius = 6f,
                SkillTargetType = UnitType.Everything,
                SpellDuration = 3,
            }){ SkillLevel = 1}
        };

        items = new List<Item>() {
            new Item {
                name = "测试物品",
                itemActiveSkill = new PointingSkill(new SkillModel(){
                    SelfEffect = targetPositionEffect,
                    TargetEffect = targetPositionEffect,
                    SpellDistance = 10,
                    Cooldown = 3,
                    SkillTargetType = UnitType.Everything,
                }),
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
                MaxLevel = 10,
                maxHp = 10000,
                Hp = 200,
                maxMp = 1000,
                Mp = 200,
                Name = "sjm",
                attackDistance = 10f,
                Level = 0,
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

}
