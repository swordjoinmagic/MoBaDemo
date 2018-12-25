using BehaviorDesigner.Runtime;
using DigitalRuby.LightningBolt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 用于管理一个单位在游戏中的逻辑,如播放动画,播放音效,进行攻击等等操作.
/// </summary>
[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class CharacterMono : MonoBehaviour {
    #region 单位的声音模块
    private AudioSource audioSource;     // 声音模块的发声者
    CharacterAudio characterAudio = null;
    #endregion

    #region 小兵的WayPointUnit属性
    public WayPointsUnit wayPointsUnit = null;
    #endregion

    #region 单位身上的事件及隐藏的委托

    //======================================
    // 委托集合

    /// <summary>
    /// 当单位身上的状态改变时，触发的事件
    /// </summary>
    /// <param name="battleState">新状态</param>
    public delegate void BattleStatusChangedHandler(BattleState battleState);

    /// <summary>
    /// 当单位进行攻击时，或者遭受伤害时，触发的事件
    /// </summary>
    /// <param name="Attacker">攻击者</param>
    /// <param name="Suffered">遭受伤害者</param>
    /// <param name="Damage">此次攻击造成的伤害</param>
    public delegate void AttackHandler(CharacterMono Attacker, CharacterMono Suffered, Damage damage);

    /// <summary>
    /// 当单位进行施法时，或者遭受某个施法的单位指向时，触发的事件
    /// </summary>
    /// <param name="Spller">施法者</param>
    /// <param name="Target">法术指定目标</param>
    /// <param name="damage">此次造成的伤害（为负则为治疗）</param>
    /// <param name="activeSkill">此次施法释放的主动技能（被动技能不算把？）</param>
    /// <param name="position">施法的目标位置</param>
    public delegate void SpellHandler(CharacterMono Spller, CharacterMono Target,Vector3 position, ActiveSkill activeSkill);

    /// <summary>
    /// 当单位死亡时，进行调用
    /// </summary>
    /// <param name="dead">死者</param>
    public delegate void DiedHandler(CharacterMono dead);

    /// <summary>
    /// 当单位学习/遗忘技能时，触发的回调函数/事件
    /// </summary>
    /// <param name="learner"></param>
    /// <param name="skill"></param>
    public delegate void SkillLerningHandler(CharacterMono learner, BaseSkill skill);

    /// <summary>
    /// 当单位获取/遗失物品时，触发的回调函数/事件
    /// </summary>
    /// <param name="characterMono"></param>
    public delegate void ItemHandler(CharacterMono characterMono, ItemGrid itemGrid);

    /// <summary>
    /// 当单位移动时,触发的事件
    /// </summary>
    /// <param name="characterMono">正在移动的单位</param>
    /// <param name="pos">单位要移动到的目标位置</param>
    public delegate void MoveHandler(CharacterMono characterMono,Vector3 pos);

    /// <summary>
    /// 当单位播放动画时触发的事件
    /// </summary>
    /// <param name="characterMono"></param>
    /// <param name="operation">要播放的动画的字符串标识符</param>
    public delegate void AnimationHandler(CharacterMono characterMono,string operation);
    //=====================================
    // 事件集合

    // 当单位身上增加新的状态(如中毒状态)时,触发的事件
    public event BattleStatusChangedHandler OnAddNewBattleStatus;
    public event BattleStatusChangedHandler OnRemoveBattleStatus;

    // 当单位移动时,触发的事件
    public event MoveHandler OnMove;

    // 当单位攻击、遭受伤害时，触发的事件
    public event AttackHandler OnAttack;        // 当攻击时
    public event AttackHandler OnSuffered;      // 当单位遭受攻击时

    // 当单位施法时，触发的事件
    public event SpellHandler OnSpell;

    // 当单位死亡时，触发的事件
    public event DiedHandler OnUnitDied;

    // 当单位学习/遗忘技能时
    public event SkillLerningHandler OnLearnSkill;
    public event SkillLerningHandler OnForgetSkill;

    // 当单位获得/遗失物品时
    public event ItemHandler OnGetItem;
    public event ItemHandler OnLostItem;
    public event ItemHandler OnDelteItem;   // 当一个物品从物品栏删除时

    // 当单位播放动画时
    public event AnimationHandler OnPlayAnimation;
    #endregion

    // 当前人物的动画组件以及寻路组件
    private Animator animator;
    private NavMeshAgent agent;

    #region 单位的网络ID
    private string netWorkPlayerID = "";
    public string NetWorkPlayerID {
        get { return netWorkPlayerID; }
        set { netWorkPlayerID = value; }
    }
    #endregion

    /// <summary>
    /// 表示当前单位的一些基本属性,如:hp,mp,攻击力等等
    /// </summary>
    public CharacterModel characterModel;
    // 此单位的所有者
    public Player Owner;

    #region GamePlay相关 包含一些用于战斗时的变量

    // 表示当前单位是否垂死
    public bool isDying = false;

    // 当前准备释放的技能
    public ActiveSkill prepareSkill = null;

    // 表示是否准备释放法术
    public bool isPrepareUseSkill = false;

    // 当前准备释放的技能是否是物品技能
    public bool isPrepareUseItemSkill = false;
    // 当前准备释放的物品技能的物品格子类
    public ItemGrid prepareItemSkillItemGrid;

    // 周围的敌人
    public List<CharacterMono> arroundEnemies;
    // 周围的友军
    public List<CharacterMono> arroundFriends;

    // 当前角色拥有的所有状态
    private List<BattleState> battleStates = new List<BattleState>();

    #endregion

    #region 用于处理单位身上的状态集合

    /// <summary>
    /// 为单位增加新状态
    /// </summary>
    /// <param name="newBattleState"></param>
    public void AddBattleState(BattleState newBattleState) {
        // 判断单位身上已经是否有这个状态了,并且判断状态是否可以叠加
        var state = battleStates.Find((battleState) => { return battleState.Name == newBattleState.Name; });
        if (newBattleState.IsStackable || state==null) {
            battleStates.Add(newBattleState);

            // 触发单位状态附加事件,向所有订阅该事件的观察者发送消息
            if (OnAddNewBattleStatus != null)
                OnAddNewBattleStatus(newBattleState);
        }

        // 如果状态已经存在，且状态不可叠加，那么重置状态存在时间
        // 不触发单位状态附加事件
        if (state != null && !state.IsStackable) {
            state.ResetDuration();
        }

    }

    /// <summary>
    /// 根据状态对象去除单位身上某一个状态,重载方法的最底层重载
    /// </summary>
    /// <param name="battleState"></param>
    public void RemoveBattleState(BattleState battleState) {

        if (battleStates.Remove(battleState)) {

            if (OnRemoveBattleStatus != null)
                OnRemoveBattleStatus(battleState);
        }
    }
    /// <summary>
    /// 根据状态名去除单位身上某一个状态
    /// </summary>
    /// <param name="battleState"></param>
    public void RemoveBattleState(string battleState) {
        for (int i=0;i<battleStates.Count;) {
            var state = battleStates[i];
            if (state != null && state.Name == battleState) {
                RemoveBattleState(state);
                break;
            } else {
                i++;
            }
        }
    }
    public void RemoveBattleState(int index) {
        if(index<battleStates.Count)
            RemoveBattleState(battleStates[index]);
    }
    public void RemoveBattleState(BattleStateType battleStateType) {
        for (int i = 0; i < battleStates.Count;) {
            var state = battleStates[i];
            if (state != null && state.GetType().ToString() == battleStateType.ToString()) {
                RemoveBattleState(state);
                break;
            } else {
                i++;
            }
        }
    }
    public void RemoveAllBattleState() {
        while (battleStates.Count>0) {
            RemoveBattleState(0);
        }
    }

    #endregion

    #region 测试
    //================================================
    // ●测试用
    public GameObject targetPositionEffect;
    public GameObject targetEnemryEffect;
    public GameObject projectile;
    public GameObject stateHolderEffect;
    public LightningBoltScript lightningBoltScriptPrefab;     // 用于控制闪电链的LineRender对象
    public void Install() {
        //characterModel = new HeroModel {
        //projectileModel = new ProjectileModel {
        //    spherInfluence = 5,
        //    targetPositionEffect = targetPositionEffect,
        //    movingSpeed = 5
        //},
        //characterModel.maxHp = 10000,
        //    characterModel.Hp = 200,
        //    characterModel.maxMp = 1000,
        //    characterModel.Mp = 1000,
        //    characterModel.Name = "sjm",
        //    attackDistance = 10f,
        //    Level = 0,
        //    forcePower = 100,
        //    needExp = 1000,
        //    Attack = 100,
        //    AttackFloatingValue = 99,
        //    Exp = 0,
        //    expfactor = 2,
        //    AvatarImagePath = "PlayerAvatarImage",
        //    agilePower = 20,
        //    intelligencePower = 10,
        //    mainAttribute = HeroMainAttribute.AGI,
        //    skillPointGrowthPoint = 1,
        //    TurningSpeed = 120,
        //    AttackAudioPath = "attackAudio",
        //    Radius = 10,
        //    MovingSpeed = 4,
        //};
        Owner = new Player() {
            Money = 1000
        };
        //this.LearnSkill(new HaloSkill {
        //    SkillLevel = 3,
        //    SkillName = "aaaaaaaaaaaaaaaa",
        //    IconPath = "00046",
        //    CD = 2f,
        //    LongDescription = "one skill Description",
        //    HaloEffect = targetEnemryEffect,
        //    SkillTargetType = UnitType.Everything,
        //    TiggerType = PassiveSkillTriggerType.Halo,
        //    inflenceRadius = 10f,
        //    additiveState = new PoisoningState {
        //        Description = "anohter",
        //        stateHolderEffect = targetEnemryEffect,
        //        Duration = -1,
        //        IconPath = "0041",
        //        Damage = new Damage { PlusDamage = 100 },
        //        Name = "anohter",
        //        IsStackable = false,
        //    }
        //});
        //LearnSkill(new SwitchBattleStateSkill {
        //    BaseDamage = 1000,
        //    KeyCode = KeyCode.T,
        //    Mp = 220,
        //    PlusDamage = 200,
        //    CD = 5f,
        //    SpellDistance = 0,
        //    SkillName = "T技能",
        //    IconPath = "00041",
        //    SkillLevel = 6,
        //    TargetEffect = targetPositionEffect,
        //    SkillTargetType = UnitType.Everything,
        //    AdditionalState = new BattleState {
        //        Description = "anohter",
        //        stateHolderEffect = targetEnemryEffect,
        //        Duration = -1,
        //        IconPath = "0041",
        //        Name = "anohter",
        //        IsStackable = false,
        //        statePassiveSkills = new List<PassiveSkill>{
        //                    new BaseAtributeChangeSkill{
        //                        attribute = CharacterAttribute.Attack,
        //                        value = 500,
        //                    }
        //                }
        //    }
        //});
        //LearnSkill(new ChainSkill {
        //    BaseDamage = 1000,
        //    KeyCode = KeyCode.P,
        //    Mp = 220,
        //    PlusDamage = 200,
        //    SpellDistance = 4f,
        //    CD = 5f,
        //    Count = 4,
        //    Damage = new Damage { BaseDamage = -1000, PlusDamage = -1000 },
        //    SkillName = "W技能",
        //    IconPath = "00041",
        //    LongDescription = "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化," +
        //            "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
        //            "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
        //            "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化",
        //    SkillLevel = 6,
        //    TargetEffect = targetEnemryEffect,
        //    SkillTargetType = UnitType.Everything,
        //    SkillInfluenceRadius = 10
        //});
        //LearnSkill(new RangeSkillGroup {
        //    KeyCode = KeyCode.F,
        //    PlusDamage = 200,
        //    TargetEffect = targetPositionEffect,
        //    SpellDistance = 10f,
        //    SkillName = "F技能",
        //    IconPath = "00041",
        //    SkillLevel = 6,
        //    SkillInfluenceRadius = 6f,
        //    activeSkills = new ActiveSkill[]{
        //                new AdditionalStateSkill{
        //                    SpellDistance = 10,
        //                    AdditionalState = new PoisoningState{
        //                        Description = "范围中毒技能",
        //                        stateHolderEffect = targetEnemryEffect,
        //                        Duration = 15,
        //                        IconPath = "0041",
        //                        Damage = new Damage{ PlusDamage = 100 },
        //                        Name = "中毒",
        //                        IsStackable = false,
        //                        statePassiveSkills = new List<PassiveSkill>{
        //                            new BaseAtributeChangeSkill{
        //                                attribute = CharacterAttribute.Attack,
        //                                value = 10,
        //                                isScale = true
        //                            }
        //                        }
        //                    },
        //                    SkillTargetType = UnitType.Everything
        //                }
        //            },
        //    skillDelayAttributes = new SkillDelayAttribute[] {
        //                new SkillDelayAttribute{
        //                    isDelay = false,
        //                    index = -1,
        //                }
        //            },
        //    SkillTargetType = UnitType.Everything,
        //});
        //LearnSkill(new TransformSkill {
        //    KeyCode = KeyCode.W,
        //    SkillLevel = 1,
        //    SkillTargetType = UnitType.Everything,
        //    SkillName = "闪现",
        //    IconPath = "00046",
        //    Mp = 10,
        //    PlusDamage = 200,
        //    SpellDistance = 15f,
        //    CD = 2f,
        //    LongDescription = "one skill Description",
        //    TargetEffect = targetEnemryEffect,
        //    SelfEffect = targetPositionEffect
        //});
        //LearnSkill(new RangeSkillGroup {
        //    KeyCode = KeyCode.E,
        //    PlusDamage = 200,
        //    TargetEffect = targetPositionEffect,
        //    SpellDistance = 10f,
        //    SkillName = "E技能",
        //    IconPath = "00041",
        //    SkillLevel = 6,
        //    SkillInfluenceRadius = 6f,
        //    activeSkills = new ActiveSkill[]{
        //                new DisperseStateSkill{
        //                    SpellDistance = 10,
        //                    BattleStateType = BattleStateType.PoisoningState,
        //                    SkillTargetType = UnitType.Everything
        //                }
        //            },
        //    skillDelayAttributes = new SkillDelayAttribute[] {
        //                new SkillDelayAttribute{
        //                    isDelay = false,
        //                    index = -1,
        //                }
        //            },
        //    SkillTargetType = UnitType.Everything,
        //});
        //LearnSkill(new AdditionalActiveSkill {
        //    TiggerType = PassiveSkillTriggerType.WhenAttack,
        //    CD = 0.1f,
        //    IconPath = "0041",
        //    SkillName = "AdditionActiveSkil",
        //    SkillTargetType = UnitType.Everything,
        //    SkillLevel = 1,
        //    additionalActiveSkill = new ChainSkill {
        //        BaseDamage = 1000,
        //        KeyCode = KeyCode.P,
        //        Mp = 220,
        //        PlusDamage = 200,
        //        SpellDistance = 4f,
        //        CD = 5f,
        //        Count = 4,
        //        Damage = new Damage { PlusDamage = 500 },
        //        SkillName = "W技能",
        //        IconPath = "00041",
        //        LongDescription = "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化," +
        //            "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
        //            "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化" +
        //            "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化",
        //        SkillLevel = 6,
        //        TargetEffect = targetEnemryEffect,
        //        SkillTargetType = UnitType.Everything,
        //        SkillInfluenceRadius = 10
        //    }
        //});
        LearnSkill(TestDatabase.Instance.baseSkills[0]);
        LearnSkill(TestDatabase.Instance.baseSkills[1]);
        LearnSkill(TestDatabase.Instance.baseSkills[2]);
        LearnSkill(TestDatabase.Instance.baseSkills[3]);
        LearnSkill(TestDatabase.Instance.baseSkills[5]);
    }
    //================================================
    #endregion

    // 单位初始化身上所有模块的方法
    public void InitModule() {
        //============================
        // ·初始化声音模块
        audioSource = GetComponent<AudioSource>();
        characterAudio = new CharacterAudio(characterModel);
        characterAudio.Bind(this, audioSource);

        //==============================
        // ·初始化被动技能的订阅事件
        OnLearnSkill += (CharacterMono learner, BaseSkill skill) => {
            CalculatePlusAttribute();
        };
        OnForgetSkill += (CharacterMono learner, BaseSkill skill) => {
            CalculatePlusAttribute();
        };
        OnGetItem += (CharacterMono characterMono, ItemGrid itemGrid) => {
            CalculatePlusAttribute();
        };
        OnLostItem += (CharacterMono characterMono, ItemGrid itemGrid) => {
            CalculatePlusAttribute();
        };
        OnAddNewBattleStatus += (BattleState battleState) => {
            CalculatePlusAttribute();
        };
        OnRemoveBattleStatus += (BattleState battleState) => {
            CalculatePlusAttribute();
        };
    }


    /// <summary>
    /// 根据CharacterModel，初始化动画状态与移动代理的方法
    /// </summary>
    public void InitAnimatorAndNavAgent() {
        agent.speed = characterModel.MovingSpeed;
        agent.angularSpeed = characterModel.TurningSpeed;
    }

    public void Init() {

        #region 测试
        if (CompareTag("Player"))
            Install();
        //if (CompareTag("Enermy"))
        //    wayPointsUnit = new WayPointsUnit(WayPointEnum.UpRoad,UnitFaction.Red);

        characterModel.Hp = characterModel.maxHp;
        characterModel.Mp = characterModel.maxMp;

        // 获得该单位身上绑定的组件
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 获得该单位周围的敌人
        arroundEnemies = transform.Find("SearchTrigger").GetComponent<SearchTrigger>().arroundEnemies;
        // 该单位周围友军
        arroundFriends = transform.Find("SearchTrigger").GetComponent<SearchTrigger>().arroundFriends;

        // 初始化六个物品格子,六个物品格子在物品栏中的摆放顺序是
        // 从上到下,从左到右,依次顺序,123456
        characterModel.itemGrids = new List<ItemGrid> {
            new ItemGrid{ item=null,ItemCount=0,hotKey=KeyCode.Alpha1,index=1 },
            new ItemGrid{ item=null,ItemCount=0,hotKey=KeyCode.Alpha2,index=2 },
            new ItemGrid{ item=null,ItemCount=0,hotKey=KeyCode.Alpha3,index=3 },
            new ItemGrid{ item=null,ItemCount=0,hotKey=KeyCode.Alpha4,index=4 },
            new ItemGrid{ item=null,ItemCount=0,hotKey=KeyCode.Alpha5,index=5 },
            new ItemGrid{ item=null,ItemCount=0,hotKey=KeyCode.Alpha6,index=6 },
        };

        //============================
        // 与ViewModel双向绑定
        Bind();

        #region 对所有单位的测试
        //================================================
        // 测试
        if (CompareTag("Player")) {
            characterModel.itemGrids[0].item = new Item {
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
            };
            characterModel.itemGrids[0].ItemCount = 2;

        }

        //HaloSkill haloSkill = new HaloSkill() { SkillLevel = 1, inflenceRadius = 10, targetFaction = UnitFaction.Red, HaloEffect= stateHolderEffect };
        //haloSkill.Execute(this);
        #endregion

        #endregion

        InitModule();

        // 根据CharacterModel初始化身上的组件
        InitAnimatorAndNavAgent();
    }

    private void Start() {
        if (!CompareTag("Player"))
            Init();
    }

    public virtual void Update() {

        // 处理单位的状态
        for (int i = 0; i < battleStates.Count;) {
            BattleState battleState = battleStates[i];

            // 更新状态
            battleState.Update(this);

            // 如果该状态没有消失，去更新下一个状态
            // 如果该状态消失了，那么i不进行++
            if (!battleState.IsStateDying) {
                i++;
            }

        }

    }

    #region 用于处理物品的获取与消耗/丢失
    /// <summary>
    /// 单位获得物品的方法，返回True表示单位成功获得该物品，
    /// 返回Fals表示因为单位物品栏限制，单位获得物品失败。
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool GetItem(ItemGrid item) {
        for (int i = 0; i < characterModel.itemGrids.Count; i++) {
            ItemGrid itemGrid = characterModel.itemGrids[i];
            if (itemGrid.item == null) {
                itemGrid.item = item.item;
                itemGrid.ItemCount = item.ItemCount;

                if (OnGetItem != null) OnGetItem(this,item);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 单位遗失某个技能的方法，返回True表示遗失成功，返回False表示没有在物品栏找到该物品
    /// </summary>
    /// <returns></returns>
    public bool LostItem(ItemGrid item) {
        for (int i=0;i<characterModel.itemGrids.Count;i++) {
            ItemGrid itemGrid = characterModel.itemGrids[i];
            if (itemGrid.item!=null && itemGrid.item.name == item.item.name) {
                itemGrid.ItemCount -= 1;

                if (OnLostItem != null) OnLostItem(this,item);
                if (itemGrid.ItemCount == 0 && OnDelteItem != null) OnDelteItem(this,item);
                return true;
            }
        }
        return false;
    }
    public bool LostItem(int index) {
        try {
            return LostItem(characterModel.itemGrids[index]);
        } catch (Exception e) {
            Debug.LogWarning(e.Message);
            return false;
        }
    }
    #endregion

    #region 用于处理人物学习/遗忘技能

    /// <summary>
    /// 单位学习技能的方法,返回True表示学习成功，
    /// 返回False表示因为单位身上最大技能数量的限制，学习失败
    /// </summary>
    /// <param name="baseSkill"></param>
    /// <returns></returns>
    public bool LearnSkill(BaseSkill baseSkill) {
        if (characterModel.BaseSkills.Count() < characterModel.MaxSkillCount) {
            characterModel.BaseSkills.Add(baseSkill);
            
            if (baseSkill is ActiveSkill) {
                characterModel.activeSkills.Add(baseSkill as ActiveSkill);
            } else {
                characterModel.passiveSkills.Add(baseSkill as PassiveSkill);
            }

            //================================================================================
            // 用于判断学习的技能是否是光环技能,对于光环技能来说,学习时自动执行他的Execute方法
            // 目的是为了给单位增加光环触发器
            if (baseSkill is HaloSkill) {
                (baseSkill as HaloSkill).Execute(this);
            }

            // 触发学习技能事件
            if (OnLearnSkill != null) OnLearnSkill(this,baseSkill);
        }
        return false;
    }

    /// <summary>
    /// 单位遗忘技能的方法，遗忘成功返回True，技能列表中没有这招技能即为False
    /// </summary>
    /// <param name="baseSkill"></param>
    /// <returns></returns>
    public bool ForgetSkill(BaseSkill baseSkill) {
        for (int i=0;i< characterModel.BaseSkills.Count();i++) {
            var skill = characterModel.BaseSkills[i];
            if (skill.SkillName == baseSkill.SkillName) {
                characterModel.BaseSkills.RemoveAt(i);

                //================================================================================
                // 用于判断学习的技能是否是光环技能,对于光环技能来说,学习时自动执行他的Execute方法
                // 目的是为了给单位增加光环触发器
                if (baseSkill is HaloSkill) {
                    (baseSkill as HaloSkill).CancelExecute(this);
                }

                // 触发遗忘技能事件
                if (OnForgetSkill != null) OnForgetSkill(this,baseSkill);
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Plus属性的计算
    /// <summary>
    /// 用于计算单位身上的Plus属性，即通过被动技能、装备、状态增加的属性
    /// </summary>
    public void CalculatePlusAttribute() {

        //===============================
        // 新的Plus属性
        int attackPlusNew = 0;
        int defensePlusNew = 0;
        int maxHPPlusNew = 0;
        int maxMPPlusNew = 0;
        int movingSpeedPlusNew = 0;
        float attackDistancePlusNew = 0;
        float attackSpeedPlusNew = 0;
        float DodgeRatePlusNew = 0;
        float magicalResistancePlusNew = 0;
        float physicalResistancePlusNew = 0;

        //===================================
        // 遍历被动技能
        foreach (var skill in characterModel.passiveSkills) {
            GetAdditionalPlusAttribute(skill,ref attackPlusNew,ref defensePlusNew,ref maxHPPlusNew,ref maxMPPlusNew,
                ref movingSpeedPlusNew,ref attackDistancePlusNew,ref attackSpeedPlusNew,ref DodgeRatePlusNew,ref magicalResistancePlusNew,
                ref physicalResistancePlusNew);
        }

        //=====================================
        // 遍历状态
        for (int i=0;i<battleStates.Count;i++) {
            BattleState battleState = battleStates[i];
            if (battleState == null || battleState.IsStateDying || battleState.statePassiveSkills==null) {
                continue;
            }
            foreach (var skill in battleState.statePassiveSkills) {
                GetAdditionalPlusAttribute(skill, ref attackPlusNew, ref defensePlusNew, ref maxHPPlusNew, ref maxMPPlusNew,
                    ref movingSpeedPlusNew, ref attackDistancePlusNew, ref attackSpeedPlusNew, ref DodgeRatePlusNew, 
                    ref magicalResistancePlusNew, ref physicalResistancePlusNew);
            }
        }

        //=========================================
        // 遍历装备
        foreach (var itemGrid in characterModel.itemGrids) {
            if (itemGrid.item!=null && itemGrid.ItemCount != 0) {
                //===============================
                // 遍历装备技能
                foreach (var skill in itemGrid.item.itemPassiveSkills) {
                    GetAdditionalPlusAttribute(skill, ref attackPlusNew, ref defensePlusNew, ref maxHPPlusNew, ref maxMPPlusNew,
                        ref movingSpeedPlusNew, ref attackDistancePlusNew, ref attackSpeedPlusNew, ref DodgeRatePlusNew,
                        ref magicalResistancePlusNew, ref physicalResistancePlusNew);
                }
            }
        }

        //================================
        // 设置新的Plus属性
        characterModel.AttackPlus = attackPlusNew;
        characterModel.DefensePlus = defensePlusNew;
        characterModel.MovingSpeedPlus = movingSpeedPlusNew;
        characterModel.AttackDistancePlus = attackDistancePlusNew;
        characterModel.DodgeRatePlus = DodgeRatePlusNew;
        characterModel.MagicalResistancePlus = magicalResistancePlusNew;
        characterModel.PhysicalResistancePlus = physicalResistancePlusNew;

    }

    /// <summary>
    /// 根据某一个GainAttr被动技能，
    /// </summary>
    /// <param name="passiveSkill"></param>
    public void GetAdditionalPlusAttribute(PassiveSkill skill,
        ref int attackPlusNew,ref int defensePlusNew,ref int maxHPPlusNew,ref int maxMPPlusNew,
        ref int movingSpeedPlusNew,ref float attackDistancePlusNew,ref float attackSpeedPlusNew,
        ref float DodgeRatePlusNew,ref float magicalResistancePlusNew,ref float physicalResistancePlusNew) {

        CharacterAttribute attribute = CharacterAttribute.Attack;
        int attributePlusNew = 0;
        float attributePlusNewF = 0;

        if (skill.TiggerType == PassiveSkillTriggerType.GainAttribute) {
            skill.Execute(characterModel, out attributePlusNew, out attribute);
            switch (attribute) {
                case CharacterAttribute.Attack:
                    attackPlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.Defense:
                    defensePlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.MaxHP:
                    maxHPPlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.MaxMp:
                    maxMPPlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.movingSpeed:
                    movingSpeedPlusNew += attributePlusNew;
                    break;
            }
            skill.Execute(characterModel, out attributePlusNewF, out attribute);
            switch (attribute) {
                case CharacterAttribute.AttackDistance:
                    attackDistancePlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.AttackSpeed:
                    attackSpeedPlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.DodgeRate:
                    DodgeRatePlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.MagicalResistance:
                    magicalResistancePlusNew += attributePlusNew;
                    break;
                case CharacterAttribute.PhysicalResistance:
                    physicalResistancePlusNew += attributePlusNew;
                    break;
            }
        }
    }

    #endregion

    #region 人物的逻辑操作,包括 追逐敌人、攻击敌人、施法、移动等操作
    //=====================================================
    // 人物的逻辑操作,包括 追逐敌人、攻击敌人、施法、移动等操作

    /// <summary>
    /// 处理人物追击的逻辑
    /// 当人物追击完成(也就是移动到了目标单位面前)返回true,否则返回false 
    /// <para></para>
    /// 追击部分
    /// 当移动到小于攻击距离时，自动停止移动,
    /// 否则继续移动,直到追上敌人,或者敌人消失在视野中
    /// </summary>
    /// <param name="targetTransform">要追击的单位的位置</param>
    /// /// <param name="forwardDistance">跟目标的距离</param>
    /// <returns></returns>
    public bool Chasing(Vector3 position,float forwardDistance) {

        // 获得当前单位与目标单位的距离
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(position.x, position.z)
        );

        if (!agent.pathPending && distance <= forwardDistance) {
            //animator.SetBool("isRun", false);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetNextAnimatorStateInfo(0).IsName("Run")) {
                DoCharacterMonoAnimation(AnimatorEnumeration.Idle);
                agent.isStopped = true;
                //agent.
            }
            return true;
        } else {
            //animator.SetBool("isRun", true);
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Run") && !animator.GetNextAnimatorStateInfo(0).IsName("Run"))
                DoCharacterMonoAnimation(AnimatorEnumeration.Run);
            agent.isStopped = false;
            agent.SetDestination(position);

            // 触发移动事件
            if (OnMove != null) OnMove(this,position);

            return false;
        }

    }

    /// <summary>
    /// 判断目标是否处于当前单位的正面
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsTargetFront(CharacterMono target) {
        // 获得从当前单位到目标单位的方向向量
        Vector3 direction = target.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, direction) <= 30f)
            return true;
        else {

            //==============================
            // 表明此单位正在转身，触发OnMove事件
            if (OnMove != null) OnMove(this, transform.position);

            return false;
        }
    }

    /// <summary>
    /// 处理人物攻击的函数,完成一次攻击返回True，否则返回False
    /// </summary>
    /// <param name="isAttackFinish">本次攻击是否完成</param>
    /// <param name="targetTransform">目标敌人的Transform</param>
    /// <param name="target">目标敌人的Mono对象</param>
    public virtual bool Attack(ref bool isAttackFinish, Transform targetTransform, CharacterMono target) {

        if (!target.IsCanBeAttack()) {
            ResetAttackStateAnimator();
            arroundEnemies.Remove(target);
            return false;
        }

        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        // 判断单位是否正对目标，如果没有，则转身面对目标在进行攻击(注意必须是在单位没有攻击时，才转向敌人)
        if (!IsTargetFront(target) && !currentAnimatorStateInfo.IsName("attack")) {

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), characterModel.TurningSpeed * Time.deltaTime * 0.1f);

            return false;
        }

        //======================================
        // 播放攻击动画
        // 如果准备开始攻击,那么播放动画
        if (!currentAnimatorStateInfo.IsName("attack") && !nextAnimatorStateInfo.IsName("attack")) {
            //animator.SetTrigger("attack");
            DoCharacterMonoAnimation(AnimatorEnumeration.Attack);
            isAttackFinish = false;
        }


        //======================================
        // 伤害判断
        if (currentAnimatorStateInfo.IsName("attack") &&
            nextAnimatorStateInfo.IsName("Idle") &&
            !isAttackFinish) {

            if (characterModel.projectileModel == null) {
                Damage damage = characterModel.GetDamage(target.characterModel);

                
                foreach (PassiveSkill passiveSkill in characterModel.passiveSkills) {
                    if (passiveSkill.TiggerType == PassiveSkillTriggerType.WhenAttack || passiveSkill.TiggerType == PassiveSkillTriggerType.WhenNormalAttack) {
                        // 执行所有倍增伤害的被动技能
                        passiveSkill.Execute(this,target,ref damage);

                        // 执行攻击时特效
                        passiveSkill.Execute(this,target);
                    }
                }

                target.characterModel.Damaged(target,damage,this);

                // 攻击事件，向所有订阅近战攻击的观察者发送消息
                if (OnAttack != null) OnAttack(this, target, damage);

            } else {
                Vector3 shootPosition = transform.Find("shootPosition").position;
                Damage damage = characterModel.GetDamage(target.characterModel);

                // 执行所有倍增伤害的被动技能
                foreach (PassiveSkill passiveSkill in characterModel.passiveSkills) {
                    if (passiveSkill.TiggerType == PassiveSkillTriggerType.WhenAttack || passiveSkill.TiggerType == PassiveSkillTriggerType.WhenNormalAttack) {
                        passiveSkill.Execute(this, target, ref damage);
                    }
                }

                ProjectileMono projectileMono = ProjectileMonoFactory.AcquireObject(this,target,shootPosition,damage,templateObject:projectile);

                // 攻击事件，向所有订阅近战攻击的观察者发送消息
                if (OnAttack != null) OnAttack(this, target, projectileMono.damage);
            }
            isAttackFinish = true;
            return true;
        }

        return false;
    }

    #region 辅助动画相关
    /// <summary>
    /// 重置人物的攻击动画
    /// </summary>
    public void ResetAttackStateAnimator() {
        animator.ResetTrigger("attack");
    }

    /// <summary>
    /// 重置人物的施法动画
    /// </summary>
    public void ResetSpeellStateAnimator() {
        animator.ResetTrigger("spell");
    }

    /// <summary>
    /// 重置目前单位的所有动画,如攻击动画、移动动画、施法动画。
    /// </summary>
    public void ResetAllStateAnimator() {
        animator.ResetTrigger("spell");
        animator.ResetTrigger("attack");
        animator.SetBool("isRun", false);
    }

    /// <summary>
    /// 单位回到Idle状态时进行的处理
    /// </summary>
    public void ResetIdle() {
        // 清空动画状态
        ResetAllStateAnimator();
        // 设置agent为不可行动
        agent.isStopped = true; 
    }

    /// <summary>
    /// 触发动画播放事件的代理播放动画的方法,
    /// 使用此方法的目的是:
    ///     1. 防止后面瞎几把改Animator里面的各种trigger,bool时候,不会搞得整个代码都要重新改
    ///     2. 解耦网络通信与游戏逻辑,当播放动画时,自动向服务器发送播放动画的协议
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animationStr"></param>
    public void DoCharacterMonoAnimation(string animationStr) {
        switch (animationStr) {
            case "Spell":
                animator.SetTrigger("spell");
                break;
            case "Attack":
                animator.SetTrigger("attack");
                break;
            case "Run":
                animator.ResetTrigger("spell");
                animator.ResetTrigger("attack");
                animator.SetBool("isRun", true);
                break;
            case "Idle":
                animator.SetBool("isRun", false);
                break;
            case "Died":
                animator.SetTrigger("died");
                break;
            default:
                // IDLE
                animator.ResetTrigger("spell");
                animator.ResetTrigger("attack");
                animator.SetBool("isRun", false);
                break;
        }
        if (OnPlayAnimation != null) OnPlayAnimation(this,animationStr);
    }

    #endregion
    /// <summary>
    /// 移动到指定地点,移动结束返回False,移动尚未结束返回True
    /// </summary>
    /// <param name="position">指定地点</param>
    /// <returns></returns>
    public bool Move(Vector3 position) {
        ResetAttackStateAnimator();
        //animator.SetBool("isRun", true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run") && !animator.GetNextAnimatorStateInfo(0).IsName("Run"))
            DoCharacterMonoAnimation(AnimatorEnumeration.Run);
        agent.isStopped = false;
        agent.SetDestination(position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            //animator.SetBool("isRun", false);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetNextAnimatorStateInfo(0).IsName("Run"))
                DoCharacterMonoAnimation(AnimatorEnumeration.Idle);
            return false;
        }

        // 触发移动事件
        if (OnMove != null) OnMove(this,position);

        return true;
    }

    /// <summary>
    /// 判断人物当前准备释放的技能是不是立即释放技能,如果是,那么返回True,反之返回False
    /// </summary>
    /// <returns></returns>
    public bool IsImmediatelySpell() {
        return prepareSkill.SpellDistance == 0;
    }


    /// <summary>
    /// 判断某个单位可以被准备释放的技能攻击
    /// </summary>
    /// <returns></returns>
    protected bool CanBeExecuteToTarget(CharacterMono target) {
        // 1. 施法者和目标阵营不一样，且目标阵营不属于中立阵营，则目标属于敌人
        if (!CompareOwner(target)) {
            if (prepareSkill.ContainsTarget(UnitType.Enermy))
                return true;
        } else {
            // 2. 施法者和目标阵营一样，则目标属于朋友单位
            if (prepareSkill.ContainsTarget(UnitType.Friend)) {
                return true;
            }
        }

        // 3. 目标是英雄单位
        if (target is HeroMono) {
            if (prepareSkill.ContainsTarget(UnitType.Hero)) {
                return true;
            }
        }

        // 4. 目标是建筑物
        if ((target.characterModel.unitType & UnitType.Building) == UnitType.Building)
            if (prepareSkill.ContainsTarget(UnitType.Building))
                return true;

        // 5. 目标是守卫
        if ((target.characterModel.unitType & UnitType.Guard) == UnitType.Guard) {
            if (prepareSkill.ContainsTarget(UnitType.Guard))
                return true;
        }

        // default:
        return false;
    }

    /// <summary>
    /// 适用于指定敌人的施法技能
    /// 释放技能的函数,施法结束返回True,施法失败或施法未完成返回False
    /// </summary>
    public bool Spell(CharacterMono enemryMono, Vector3 position) {

        // 如果目标已经不可攻击,那么返回False
        if (enemryMono!=null && !enemryMono.IsCanBeAttack()) {
            ResetSpeellStateAnimator();
            arroundEnemies.Remove(enemryMono);

            return false;
        }

        // 如果指定目标不符合准备释放的技能的指定目标,返回
        if (enemryMono != null && !CanBeExecuteToTarget(enemryMono)) {
            Debug.Log("单位不符合目标技能指定目标 目标单位type:"+enemryMono.characterModel.unitType+" 技能指定的Type:"+prepareSkill.SkillTargetType);
            return false;
        }

        // 获得当前动画和下一个动画状态
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        // 判断单位是否正对目标，如果没有，则转身面对目标在进行攻击(注意必须是在单位没有攻击时，才转向敌人)
        if (enemryMono != null && !IsTargetFront(enemryMono) && !currentAnimatorStateInfo.IsName("Spell")) {

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(enemryMono.transform.position - transform.position), characterModel.TurningSpeed * 0.1f * Time.deltaTime);

            return false;
        }

        if (IsImmediatelySpell()) {
            // 原地释放技能,此时直接释放技能

            // 播放释放技能的动画
            if (!currentAnimatorStateInfo.IsName("Spell"))
                //animator.SetTrigger("spell");
                DoCharacterMonoAnimation(AnimatorEnumeration.Spell);

            // 如果技能释放结束,那么产生特效,计算伤害
            if (currentAnimatorStateInfo.IsName("Spell") &&
                nextAnimatorStateInfo.IsName("Idle")) {
                    
                if (!isPrepareUseItemSkill)
                    prepareSkill.Execute(this, position);
                else {
                    prepareItemSkillItemGrid.ExecuteItemSkill(this, position);
                    isPrepareUseItemSkill = false;
                    prepareItemSkillItemGrid = null;
                }

                isPrepareUseSkill = false;
                prepareSkill = null;
                return true;
            }
        } else {
            // 指向型技能

            // 当前距离敌人 > 施法距离,进行追击
            if (Chasing(position,prepareSkill.SpellDistance)) {
                //======================================
                // 播放施法动画
                // 如果准备开始施法,那么播放动画
                if (!currentAnimatorStateInfo.IsName("Spell") && !nextAnimatorStateInfo.IsName("Spell")) {
                    DoCharacterMonoAnimation(AnimatorEnumeration.Spell);
                }

                // 如果技能释放结束,那么产生特效,计算伤害
                if (currentAnimatorStateInfo.IsName("Spell") &&
                    nextAnimatorStateInfo.IsName("Idle")) {

                    if (!isPrepareUseItemSkill) {
                        if (prepareSkill.IsMustDesignation) {

                            // 触发施法事件
                            if (OnSpell != null) OnSpell(this, enemryMono, enemryMono.transform.position,prepareSkill);

                            prepareSkill.Execute(this, enemryMono);
                        } else {

                            // 触发施法事件
                            if (OnSpell != null) OnSpell(this, enemryMono,position, prepareSkill);

                            prepareSkill.Execute(this, position);
                        }
                    } else {

                        // 触发施法事件
                        if (OnSpell != null) OnSpell(this, enemryMono, enemryMono.transform.position, prepareSkill);

                        prepareItemSkillItemGrid.ExecuteItemSkill(this, enemryMono);
                        isPrepareUseItemSkill = false;
                        prepareItemSkillItemGrid = null;
                    }


                    isPrepareUseSkill = false;
                    prepareSkill = null;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 适用于指定地点的施法技能
    /// 释放技能的函数,施法结束返回True,施法失败或施法未完成返回False
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Spell(Vector3 position) {
        return Spell(null,position);
    }

    /// <summary>
    /// 重置身上的所有属性，用于在在对象池中获取一个旧对象时，
    /// 将其状态恢复
    /// </summary>
    public void ResetCharacterModel() {
        // 清除状态
        RemoveAllBattleState();

        // 清除死亡标志
        isDying = false;

        // 清除动画
        ResetAllStateAnimator();

        // 回满血、魔
        characterModel.Hp = characterModel.maxHp;
        characterModel.Mp = characterModel.maxMp;

        // 启动AI系统
        BehaviorTree behaviorTree = GetComponent<BehaviorTree>();
        if (behaviorTree != null)
            behaviorTree.enabled = true;

        CharacterOperationFSM characterOperationFSM = GetComponent<CharacterOperationFSM>();
        if (characterOperationFSM != null)
            characterOperationFSM.enabled = true;
    }

    
    /// <summary>
    /// 当单位受到伤害时执行的事件
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attacker"></param>
    private void OnDamged(CharacterMono victim,Damage damage, CharacterMono attacker, int nowHp) {
        //=======================================================
        // 处理单位死亡时,敌对单位获得经验的行为
        if (attacker!=null && isDying && !attacker.CompareOwner(this) && nowHp==0) {
            // 在攻击者的目标位置以r为半径的区域内,所有跟攻击者一个阵营的单位获得经验
            Collider[] colliders = Physics.OverlapSphere(attacker.transform.position,5);
            foreach (var collider in colliders) {
                HeroMono characterMono = collider.GetComponent<HeroMono>();
                if (characterMono != null) {
                    characterMono.HeroModel.Exp += this.characterModel.supportExp;
                }
            }
        }
    }

    #region 单位的死亡逻辑 hp=0 -> OnDying -> Dying -> Died -> IsDied -> Destory(this)
    /// <summary>
    /// 单位进入垂死状态
    /// </summary>
    /// <returns></returns>
    private void Dying() {

        // 设置isDying为True
        isDying = true;

        //================================
        // 清除单位目前身上所有状态
        RemoveAllBattleState();

        // 停止目前一切动作
        ResetAllStateAnimator();

        // 把人物的AI系统暂停
        BehaviorTree behaviorTree = GetComponent<BehaviorTree>();
        if (behaviorTree != null)
            behaviorTree.enabled = false;

        CharacterOperationFSM characterOperationFSM = GetComponent<CharacterOperationFSM>();
        if (characterOperationFSM != null)
            characterOperationFSM.enabled = false;

        // 播放死亡动画
        animator.SetTrigger("died");
    }

    /// <summary>
    /// 判断单位是否确确实实死了
    /// <para>确确实实死亡指的是目标单位的死亡动画已经播放完毕了</para>
    /// </summary>
    /// <returns></returns>
    private bool IsDied() {
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        // 当死亡动画播放完毕,单位确实死了
        if (isDying && currentAnimatorStateInfo.IsName("Death") && nextAnimatorStateInfo.IsName("Idle")) {

            // 触发死亡事件
            if (OnUnitDied != null) OnUnitDied(this);

            return true;
        }
        return false;
    }

    /// <summary>
    /// 与CharacterModel的Hp属性绑定的方法,当Hp为0时,宣告单位死亡
    /// </summary>
    /// <param name="oldHp"></param>
    /// <param name="newHp"></param>
    private void OnDying(int oldHp,int newHp) {
        if (newHp == 0) {
            // 单位进入垂死状态,执行相关操作,如暂停行为树的执行
            Dying();

            // 开启单位死后善后的协程
            StartCoroutine(Died());
        }
    }
    
    /// <summary>
    /// 人物死亡进行的操作,每帧判断一次,当人物死亡动画播放完毕时,摧毁该单位
    /// </summary>
    /// <returns></returns>
    public IEnumerator Died() {

        while (isDying) {
            if (IsDied()) {
                //Destroy(gameObject);
                gameObject.SetActive(false);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// 判断单位是否可以被攻击,
    /// 不可以被攻击的单位可能:
    ///     1.垂死的
    ///     2.已被摧毁的
    ///     3.已死亡的
    ///     4.无敌的
    ///     ...待续
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsCanBeAttack() {

        CharacterMono target = this;

        // 如果单位被摧毁,那么目标单位无法被攻击
        if (target == null || !target.enabled) return false;


        // 垂死单位不可被攻击
        if (target.isDying) return false;

        // 无敌的单位不可被攻击
        if (!target.characterModel.canBeAttacked) return false;

        return true;
    }

    #endregion

    /// <summary>
    /// 判断目标和自己是否同属一个阵营,当目标为中立单位时,同样返回True
    /// </summary>
    public bool CompareOwner(CharacterMono target) {
        if (characterModel.unitFaction == target.characterModel.unitFaction || target.characterModel.unitFaction == UnitFaction.Neutral)
            return true;
        else
            return false;
    }

    #endregion

    #region 绑定事件，监听单位的死亡事件和被攻击事件
    //======================================
    // ●绑定Model中的各项属性到ViewModel中
    protected virtual void Bind() {
        characterModel.HpValueChangedHandler += OnDying;        // 绑定监测死亡的函数
        characterModel.OnDamaged += OnDamged;
    }
    #endregion
}