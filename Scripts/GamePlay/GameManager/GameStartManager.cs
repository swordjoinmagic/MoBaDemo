using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来管理游戏刚刚初始化完毕之后,要做的必要操作
/// </summary>
public class GameStartManager : MonoBehaviour{

    private HeroMono characterMono;
    public GameObject characterPrafab;
    public SkillView skillView;
    public StoreView storeView;
    public MouseCursorChanged mouseCursorChanged;
    public HPView hPView;
    public AvatarView avatarView;
    public ShowPlayerMoney showPlayerMoneyView;
    public ItemListView itemListView;
    public BattleStatusListView battleStatusListView;
    public GamePlayManager gamePlayManager;
    public GameObject operateCharacterProjector;


    // 单例类
    public static GameStartManager instance;
    public static GameStartManager Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<GameStartManager>();
            return instance;
        }
    }

    private void Start() {
        StartGame();
    }

    // 开始游戏的方法
    private void StartGame() {

        characterMono = CharacterMonoFactory.AcquireObject(TestDatabase.Instance.characterModels[0], characterPrafab, new Vector3(17, 0, 18)) as HeroMono;
        characterMono.Init();
        #region Test
        characterMono.IsOperateByNowPlayer = true;
        GameObject test = GameObject.Instantiate(operateCharacterProjector,characterMono.transform,false);
        test.transform.localPosition = new Vector3(0,2,0);
        #endregion

        InitUI();
        InitGamePlay();

        if (NetWorkManager.Instance == null) {
            new GameObject().AddComponent<NetWorkManager>();
        }
            

        // 判断此时是在联机对战还是在单人游戏
        if (NetWorkManager.Instance.IsConnect()) {
            gamePlayManager.OnCreateNPCGameObject += NetWorkManager.Instance.AddNetworkNpc;

            // 联机对战

            Debug.Log("根据当前玩家的阵营:" + NetWorkManager.Instance.NowPlayerFaction + "来创建玩家");
            if (NetWorkManager.Instance.NowPlayerFaction == "Red") {
                characterMono.characterModel.unitFaction = UnitFaction.Red;
                NetWorkManager.Instance.StartGame(new Vector3(17, 0, 18), characterMono);
                Camera.main.transform.position = new Vector3(15, Camera.main.transform.position.y, 6);
            } else {
                characterMono.characterModel.unitFaction = UnitFaction.Blue;
                NetWorkManager.Instance.StartGame(new Vector3(181, 0, 179), characterMono);
                Camera.main.transform.position = new Vector3(181, Camera.main.transform.position.y, 179);
            }

            Debug.Log("当前玩家是否是房主:" + NetWorkManager.Instance.IsHomeowner);
            // 判断当前玩家是否是房主,如果是房主,开启出兵,并监听
            if (NetWorkManager.Instance.IsHomeowner) {
                gamePlayManager.Init();
            }
        } else {
            gamePlayManager.Init();
            Camera.main.transform.position = new Vector3(15, Camera.main.transform.position.y, 6);
            characterMono.GetComponent<CharacterOperationFSM>().enabled = true;
            
        }
    }

    // 初始化各类UI
    public void InitUI() {
        skillView.Init(characterMono);
        storeView.Init(characterMono);
        InstallHpView();
        InstallAvaterView();
        showPlayerMoneyView.Init(characterMono);
        itemListView.Init(characterMono);
        battleStatusListView.Init(characterMono);
    }

    public void InitGamePlay() {
        mouseCursorChanged.Init();
    }

    private void InstallHpView() {
        hPView.characterMono = characterMono;
        hPView.BindingContext = new HPViewModel();
        hPView.BindingContext.Init(characterMono.characterModel);
    }

    private void InstallAvaterView() {
        avatarView.Init(characterMono);
        avatarView.BindingContext = new AvatarViewModel();
        avatarView.BindingContext.Modify(characterMono.HeroModel);
        characterMono.avatarViewModel = avatarView.BindingContext;

    }
}

