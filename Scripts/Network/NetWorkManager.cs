using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkManager : MonoBehaviour {

    public GameObject NpcPrefab;
    public GameObject playerPrefab;

    public InputField text;
    private string nowPlayerID = "sjm";


    private bool isHomeowner = false;
    private List<string> Npcs;  // 保存一份NPC列表

    // 单例
    private static NetWorkManager instance;
    public static NetWorkManager Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<NetWorkManager>();
            }
            return instance;
        }
    }

    public string NowPlayerID {
        get {
            return nowPlayerID;
        }
    }

    public bool IsHomeowner {
        get {
            return isHomeowner;
        }
        set {
            isHomeowner = value;
        }
    }

    private Connection connection = null;

    /// <summary>
    /// 用于管理 在该场景中的所有网络单位
    /// </summary>
    public Dictionary<string, GameObject> networkPlayers = new Dictionary<string, GameObject>();

    /// <summary>
    /// 产生一个网络对象
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void AddNetworkPlayer(string id, Vector3 position) {
        GameObject player = GameObject.Instantiate(playerPrefab, position, Quaternion.identity);
        player.transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);

        // 设置CharacterMono的网络ID
        player.GetComponent<CharacterMono>().NetWorkPlayerID = id;

        // 给产生的网络对象附加伤害同步的监听方法
        player.GetComponent<HeroMono>().characterModel.OnDamaged += DamageSynchronize;


        networkPlayers.Add(id, player);
    }

    /// <summary>
    /// 增加NPC对象,监听 GamePlayeManager中对象池创建小兵的事件,这是房主产生NPC的事件
    /// 来判断是否给NPC赋予AI系统.
    /// </summary>
    public void AddNetworkNpc(Vector3 position, GameObject soliderObject,GameObjectPool poolObjectFactory) {
        // 将NPC加入networkPlayers字典,并发送SendPos协议,告诉其他客户端,游戏生成了一个NPC对象
        // 最后,给NPC加上AI系统,并且,给NPC加上SyncNPC类,用于监控NPC的行为
        if (!networkPlayers.ContainsKey(soliderObject.GetComponent<CharacterMono>().NetWorkPlayerID)) {

            // 如果本次生成的对象不是旧对象,那么将该对象放到networkPlayers字典中去,同时给对象增加AI系统和同步系统

            string id = "NPC#" + poolObjectFactory.Length;
            networkPlayers[id] = soliderObject;

            // 给单位增加AI系统
            soliderObject.AddComponent<BehaviorTree>().ExternalBehavior = Resources.Load<ExternalBehavior>("External Behavior Tree/OffseniveAI Behavior");

            // 设置网络ID
            soliderObject.GetComponent<CharacterMono>().NetWorkPlayerID = id;

            // 给产生的网络对象附加伤害同步的监听方法
            soliderObject.GetComponent<CharacterMono>().characterModel.OnDamaged += DamageSynchronize;

            // NPC增加同步系统
            new SynchronizeNPC(soliderObject.GetComponent<CharacterMono>());

            // 同步NPC位置
            SendNpcPos(id);
        }
    }

    /// <summary>
    /// 非房主产生NPC对象的方法,非房主产生的NPC无AI系统也无同步系统,其主要根据房主的NPC进行同步.
    /// </summary>
    /// <param name="id"></param>
    public void AddNetworkNpc(string id,Vector3 pos) {        
        if (!networkPlayers.ContainsKey(id)) {
            // 如果本次生成的对象不是旧对象,那么将该对象放到networkPlayers字典中去,同时给对象增加AI系统和同步系统
            GameObject NPC = GameObject.Instantiate(NpcPrefab,pos,Quaternion.identity);

            networkPlayers[id] = NPC;

            // 设置网络ID
            NPC.GetComponent<CharacterMono>().NetWorkPlayerID = id;

            // 给产生的网络对象附加伤害同步的监听方法
            NPC.GetComponent<CharacterMono>().characterModel.OnDamaged += DamageSynchronize;

        }
    }

    public void Connect() {
        // 连接本地
        connection = new Connection("127.0.0.1", 8081);

        // 初始化监听方法
        InitProtocolListener();
    }

    public void Send(ProtocolBytes protocolBytes) {
        connection.Send(protocolBytes);
    }

    /// <summary>
    /// 单击开始游戏按钮
    /// </summary>
    public void StartGame() {

        // 测试,当单击开始游戏按钮后,一个 游戏单位(表示本地玩家) 就被创建出来了
        AddNetworkPlayer(NowPlayerID, UnityEngine.Random.insideUnitCircle * 5);

        // 开启本地玩家的 状态机, 非本地玩家的状态机不开启
        networkPlayers[NowPlayerID].GetComponent<CharacterOperationFSM>().enabled = true;

        // synchronizeTest对象监听 NowPlayer 的 位置/动画/等级/技能/物品 同步事件
        new SynchronizeTest(networkPlayers[NowPlayerID].GetComponent<CharacterMono>());

        // 发送本地玩家的位置给其他所有客户端
        SendPos();
    }

    public void SendNpcPos(string id) {
        Transform playerTransform = networkPlayers[id].transform;
        Vector3 pos = playerTransform.position;
        Vector3 rotation = playerTransform.rotation.eulerAngles;

        // 构造位置改变消息
        ProtocolBytes protocolBytes = new ProtocolBytes();
        protocolBytes.AddString("UpdateInfo");
        protocolBytes.AddString(id);
        protocolBytes.AddFloat(pos.x);
        protocolBytes.AddFloat(pos.y);
        protocolBytes.AddFloat(pos.z);
        protocolBytes.AddFloat(rotation.x);
        protocolBytes.AddFloat(rotation.y);
        protocolBytes.AddFloat(rotation.z);
        connection.Send(protocolBytes);
    }

    public void SendPos() {
        Transform playerTransform = networkPlayers[NowPlayerID].transform;
        Vector3 pos = playerTransform.position;
        Vector3 rotation = playerTransform.rotation.eulerAngles;

        // 构造位置改变消息
        ProtocolBytes protocolBytes = new ProtocolBytes();
        protocolBytes.AddString("UpdateInfo");
        protocolBytes.AddString(NowPlayerID);
        protocolBytes.AddFloat(pos.x);
        protocolBytes.AddFloat(pos.y);
        protocolBytes.AddFloat(pos.z);
        protocolBytes.AddFloat(rotation.x);
        protocolBytes.AddFloat(rotation.y);
        protocolBytes.AddFloat(rotation.z);
        connection.Send(protocolBytes);
    }
    
    /// <summary>
    /// 根据用户名和密码构造登录协议
    /// </summary>
    /// <param name=""></param>
    public void SendLogin(string userName,string password) {

        if (connection == null) {
            Connect();
        }

        ProtocolBytes protocolBytes = new ProtocolBytes();
        // 协议名
        protocolBytes.AddString("LoginConn");

        // 协议参数
        protocolBytes.AddString(userName);
        protocolBytes.AddString(password);

        connection.Send(protocolBytes);
    }

    public void OnLoginSuccess(string userName) {
        nowPlayerID = userName;
    }

    /// <summary>
    /// 基于Updateinfo协议，根据ID更新一个游戏单位的位置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    public void UpdateInfo(string id, Vector3 pos,Vector3 rotation) {

        #region NPC测试
        // 单位id带有NPC#并且当前玩家不是房主
        if (id.Contains("NPC#") && !IsHomeowner) {
            if (!networkPlayers.ContainsKey(id)) {
                AddNetworkNpc(id, pos);
            } else {
                Debug.Log("更新NPC的位置");
                networkPlayers[id].transform.position = pos;
                networkPlayers[id].transform.rotation = Quaternion.Euler(rotation);
            }
            return;
        }

        #endregion

        if (id != NowPlayerID) {
            if (!networkPlayers.ContainsKey(id)) {
                AddNetworkPlayer(id, UnityEngine.Random.insideUnitCircle * 5);
            } else {
                networkPlayers[id].transform.position = pos;
                networkPlayers[id].transform.rotation = Quaternion.Euler(rotation);
            }
        }
    }

    /// <summary>
    /// 初始化所有协议的监听方法
    /// </summary>
    public void InitProtocolListener() {
        NetWorkManager.Instance.AddListener("UpdateInfo", TreateUpdateInfoProtocol);
        NetWorkManager.Instance.AddListener("AnimationOperation", TreateAnimationOperation);
        NetWorkManager.Instance.AddListener("Damage", TreateDamageProtocol);
        NetWorkManager.Instance.AddListener("Level", TreateLevelProtocol);
        NetWorkManager.Instance.AddListener("SpellSkill", TreateSpellSkillProtocol);
        NetWorkManager.Instance.AddListener("AddItem", TreateGetItemProtocol);
        NetWorkManager.Instance.AddListener("DeleteItem", TreateDeleteItemProtocol);
    }

    public void DispatchMsgEvent(ProtocolBytes protocolBytes) {
        string name = protocolBytes.GetString();
        Debug.Log("分发协议："+name);
        connection.TreateProtocol(name,protocolBytes);
    }

    /// <summary>
    /// 用于为某一个具体的协议增加监听(回调)方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="protocolHandler"></param>
    public void AddListener(string name, Connection.ProtocolHandler protocolHandler) {
        connection.AddListener(name,protocolHandler);
    }

    public void TreateUpdateInfoProtocol(ProtocolBytes protocolBytes) {
        Debug.Log("处理位置改变协议中");
        string id = protocolBytes.GetString();

        // 不处理本地玩家,因为本地玩家已经由客户端进行处理了
        if (id == NowPlayerID) return;
        if (id.Contains("NPC#") && IsHomeowner) return;

        float x = protocolBytes.GetFloat();
        float y = protocolBytes.GetFloat();
        float z = protocolBytes.GetFloat();
        float tx = protocolBytes.GetFloat();
        float ty = protocolBytes.GetFloat();
        float tz = protocolBytes.GetFloat();
        UpdateInfo(id, new Vector3(x, y, z), new Vector3(tx, ty, tz));
    }

    /// <summary>
    /// 处理AnimationOperation协议
    /// </summary>
    public void TreateAnimationOperation(ProtocolBytes protocolBytes) {

        // 用户名
        string userName = protocolBytes.GetString();

        if (userName.Contains("NPC#") && IsHomeowner) return;

        // 操作
        string operation = protocolBytes.GetString();

        // 执行操作
        if (userName != NowPlayerID) {
            NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>().DoCharacterMonoAnimation(operation);
        }
    }

    /// <summary>
    /// 处理Damage协议
    /// </summary>
    /// <param name="protocolBytes"></param>
    public void TreateDamageProtocol(ProtocolBytes protocolBytes) {

        // 获得攻击者ID
        string attackerId = protocolBytes.GetString();

        // 不处理本地玩家的伤害事件,因为本地客户端会自行处理
        if (attackerId == NowPlayerID) return;
        if (attackerId.Contains("NPC#") && IsHomeowner) return;


        CharacterMono attacker = NetWorkManager.Instance.networkPlayers[attackerId].GetComponent<CharacterMono>();
        // 获得受害者ID
        string victimId = protocolBytes.GetString();
        CharacterMono victim = NetWorkManager.Instance.networkPlayers[victimId].GetComponent<CharacterMono>();
        // 获得基础伤害
        int baseDamage = protocolBytes.GetInt();
        // 获得附加伤害
        int plusDamage = protocolBytes.GetInt();

        Damage damage = new Damage(baseDamage, plusDamage);

        victim.characterModel.Damaged(victim, damage, attacker);
    }

    /// <summary>
    /// 处理Level协议
    /// </summary>
    /// <param name="protocolBytes"></param>
    public void TreateLevelProtocol(ProtocolBytes protocolBytes) {
        // 获得用户名
        string userName = protocolBytes.GetString();

        int level = protocolBytes.GetInt();

        // 只有用户名不等于当前玩家ID时，才执行操作，
        // 因为当用户名 ==当前玩家ID时，说明这个操作已经在当前玩家客户端执行过一次了
        if (userName != NetWorkManager.Instance.NowPlayerID) {
            // 更改该玩家的远程Clone的等级
            NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>().characterModel.Level = level;
        }
    }

    /// <summary>
    /// 用于处理SpellSkill协议
    /// </summary>
    /// <param name="protocol"></param>
    public void TreateSpellSkillProtocol(ProtocolBytes protocol) {

        // 释放技能的单位的ID
        string userName = protocol.GetString();

        // 不处理本地玩家的SpellSkill协议
        if (userName == NetWorkManager.Instance.NowPlayerID) return;
        if (userName.Contains("NPC#") && IsHomeowner) return;


        Debug.Log("处理SpellSkill协议");

        // 要释放的技能的ID
        int skillId = protocol.GetInt();

        // 要释放的技能的等级
        int skillLevel = protocol.GetInt();

        // 技能目标
        string skillTarget = protocol.GetString();

        ActiveSkill skill = TestDatabase.Instance.baseSkills[skillId] as ActiveSkill;

        if (skillTarget == "Position") {
            float x = protocol.GetFloat();
            float y = protocol.GetFloat();
            float z = protocol.GetFloat();

            skill.Execute(NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>(), new Vector3(x, y, z));

        } else {
            string targetId = protocol.GetString();

            skill.Execute(NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>(), NetWorkManager.Instance.networkPlayers[targetId].GetComponent<CharacterMono>());
        }
    }

    /// <summary>
    /// 处理LearnSkill协议
    /// </summary>
    /// <param name="protocol"></param>
    public void TreateLearnSkillProtocol(ProtocolBytes protocol) {
        // 获得用户名
        string userName = protocol.GetString();

        if (userName == NetWorkManager.Instance.NowPlayerID) return;

        // 获得学习的技能的ID
        int skillID = protocol.GetInt();

    }

    /// <summary>
    /// 处理获得物品协议
    /// </summary>
    /// <param name="protocol"></param>
    public void TreateGetItemProtocol(ProtocolBytes protocol) {
        // 获得用户名
        string userName = protocol.GetString();

        if (userName == NetWorkManager.Instance.NowPlayerID) return;

        // 获得物品ID
        int itemId = protocol.GetInt();
        // 获得物品数量
        int itemNumber = protocol.GetInt();

        // 根据用户名获得英雄
        CharacterMono character = NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>();

        // 构造物品格子
        ItemGrid itemGrid = new ItemGrid {
            item = TestDatabase.Instance.items[0],
            ItemCount = itemNumber
        };

        // 处理单位获得物品事件
        character.GetItem(itemGrid);
    }

    /// <summary>
    /// 处理删除物品的协议
    /// </summary>
    /// <param name="protocol"></param>
    public void TreateDeleteItemProtocol(ProtocolBytes protocol) {

        // 获得用户名
        string userName = protocol.GetString();
        // 获得物品ID
        int id = protocol.GetInt();

        // 根据用户名获得英雄
        CharacterMono character = NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>();

        // 遍历英雄物品格子,删除物品id和id一样的物品
        for (int i = 0; i < character.characterModel.itemGrids.Count; i++) {

            ItemGrid itemGrid = character.characterModel.itemGrids[i];

            if (itemGrid.item != null && itemGrid.item.ItemId == id) {
                itemGrid.ItemCount = 0;
                return;
            }
        }
    }

    // 伤害同步
    public void DamageSynchronize(CharacterMono victim, Damage damage, CharacterMono attacker, int nowHp) {

        // 判断攻击者是不是本地玩家,只有攻击者是本地玩家时,才发送伤害事件
        if (attacker.NetWorkPlayerID == NetWorkManager.Instance.NowPlayerID) {

            // 首先获得攻击者的PlayerID
            string attackerID = NetWorkManager.Instance.NowPlayerID;
            // 获得受害者的ID
            string victimID = victim.NetWorkPlayerID;
            // 基础伤害
            int baseDamage = damage.BaseDamage;
            // 附加伤害
            int plusDamage = damage.PlusDamage;

            ProtocolBytes protocol = CreateDamageProtocol(attackerID, victimID, baseDamage, plusDamage);
            NetWorkManager.Instance.Send(protocol);

            return;
        }

        //===================================================================
        // 同步NPC与NPC,NPC与玩家,玩家与NPC之间的伤害
        // 只有当 当前玩家是房主,且攻击者ID是NPC时,才触发NPC的同步伤害
        if (attacker.NetWorkPlayerID.Contains("NPC#") && IsHomeowner) {

            Debug.Log("NPC发送攻击协议");

            // 首先获得攻击者的PlayerID
            string attackerID = attacker.NetWorkPlayerID;
            // 获得受害者的ID
            string victimID = victim.NetWorkPlayerID;
            // 基础伤害
            int baseDamage = damage.BaseDamage;
            // 附加伤害
            int plusDamage = damage.PlusDamage;

            ProtocolBytes protocol = CreateDamageProtocol(attackerID, victimID, baseDamage, plusDamage);
            NetWorkManager.Instance.Send(protocol);
        }
    }

    /// <summary>
    /// 构造伤害协议
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="victim"></param>
    /// <param name="baseDamage"></param>
    /// <param name="plusDamage"></param>
    private ProtocolBytes CreateDamageProtocol(string attacker, string victim, int baseDamage, int plusDamage) {
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("Damage");

        // 参数
        protocol.AddString(attacker);
        protocol.AddString(victim);
        protocol.AddInt(baseDamage);
        protocol.AddInt(plusDamage);

        return protocol;
    }

    // Update is called once per frame
    void Update () {
        if (connection == null) return;
        while (connection.MsgList.Count > 0) {
            ProtocolBytes protocolBytes = null;
            lock (connection.MsgList) {
                protocolBytes = connection.MsgList.Dequeue();
            }
            DispatchMsgEvent(protocolBytes);
        }
	}

    #region 测试

    public void TestConnect() {
        nowPlayerID = text.text;
        Connect();
    }
    public CharacterMono characterMono;
    SynchronizeTest synchronizeTest;
    private void Start() {

    }

    #endregion
}
