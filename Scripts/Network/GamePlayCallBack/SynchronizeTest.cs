using UnityEngine;

public class SynchronizeTest {

    private CharacterMono characterMono;

    public SynchronizeTest(CharacterMono characterMono) {
        this.characterMono = characterMono;
        Init(characterMono);
    }

    public void Init(CharacterMono characterMono) {
        NetWorkManager.Instance.AddListener("AnimationOperation", TreateAnimationOperation);
        NetWorkManager.Instance.AddListener("Damage", TreateDamageProtocol);
        NetWorkManager.Instance.AddListener("Level",TreateLevelProtocol);
        NetWorkManager.Instance.AddListener("SpellSkill",TreateSpellSkillProtocol);
        NetWorkManager.Instance.AddListener("AddItem",TreateGetItemProtocol);
        NetWorkManager.Instance.AddListener("DeleteItem",TreateDeleteItemProtocol);
        characterMono.OnMove += TransformSynchronize;
        characterMono.OnPlayAnimation += AniamtionSynchronize;
        characterMono.characterModel.LevelChangedHandler += LevelSynchronize;
        characterMono.OnSpell += SpellSkillSynchronize;
        characterMono.OnGetItem += GetItemSynchronize;
        characterMono.OnDelteItem += DeleteItemSynchronize;
    }

    private float time;

    // 位置同步
    public void TransformSynchronize(CharacterMono characterMono, Vector3 pos) {
        NetWorkManager.Instance.SendPos();
    }

    // 动画同步
    public void AniamtionSynchronize(CharacterMono characterMono, string operation) {
        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("AnimationOperation");

        // 参数
        protocol.AddString(NetWorkManager.Instance.NowPlayerID);
        protocol.AddString(operation);

        NetWorkManager.Instance.Send(protocol);
    }

    // 处理AnimationOperation协议
    public void TreateAnimationOperation(ProtocolBytes protocolBytes) {

        // 用户名
        string userName = protocolBytes.GetString();
        // 操作
        string operation = protocolBytes.GetString();

        // 执行操作
        if (userName != NetWorkManager.Instance.NowPlayerID) {
            NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>().DoCharacterMonoAnimation(operation);
        }
    }

    // 伤害同步
    public void DamageSynchronize(CharacterMono victim, Damage damage, CharacterMono attacker, int nowHp) {
        // 判断攻击者是不是本地玩家
        if (attacker.NetWorkPlayerID == NetWorkManager.Instance.NowPlayerID) {

            // 首先获得攻击者的PlayerID
            string attackerID = NetWorkManager.Instance.NowPlayerID;
            // 获得受害者的ID
            string victimID = victim.NetWorkPlayerID;
            // 基础伤害
            int baseDamage = damage.BaseDamage;
            // 附加伤害
            int plusDamage = damage.PlusDamage;

            ProtocolBytes protocol = CreateDamageProtocol(attackerID,victimID,baseDamage,plusDamage);
            NetWorkManager.Instance.Send(protocol);
        }
    }

    public void TreateDamageProtocol(ProtocolBytes protocolBytes) {

        // 获得攻击者ID
        string attackerId = protocolBytes.GetString();
        CharacterMono attacker = NetWorkManager.Instance.networkPlayers[attackerId].GetComponent<CharacterMono>();
        // 获得受害者ID
        string victimId = protocolBytes.GetString();
        CharacterMono victim = NetWorkManager.Instance.networkPlayers[victimId].GetComponent<CharacterMono>();
        // 获得基础伤害
        int baseDamage = protocolBytes.GetInt();
        // 获得附加伤害
        int plusDamage = protocolBytes.GetInt();

        Damage damage = new Damage(baseDamage,plusDamage);

        victim.characterModel.Damaged(victim,damage,attacker);
    }

    /// <summary>
    /// 构造伤害协议
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="victim"></param>
    /// <param name="baseDamage"></param>
    /// <param name="plusDamage"></param>
    private ProtocolBytes CreateDamageProtocol(string attacker,string victim,int baseDamage,int plusDamage) {
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

    // 等级同步
    public void LevelSynchronize(int oldLevel,int newLevel) {
        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("Level");

        // 参数
        protocol.AddString(NetWorkManager.Instance.NowPlayerID);
        protocol.AddInt(newLevel);

        NetWorkManager.Instance.Send(protocol);
    }
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
    /// 技能释放同步,监听施法事件(OnSpell)
    /// </summary>
    public void SpellSkillSynchronize(CharacterMono Spller, CharacterMono Target, Vector3 position, ActiveSkill activeSkill) {
        // 获得当前要释放的技能的ID
        int skillID = activeSkill.SkillID;
        // 判断该技能是否一定要指定敌人
        string skillTarget = activeSkill.IsMustDesignation ? "Target" : "Position";

        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("SpellSkill");

        // 协议参数
        protocol.AddString(NetWorkManager.Instance.NowPlayerID);
        protocol.AddInt(skillID);
        protocol.AddInt(activeSkill.SkillLevel);
        protocol.AddString(skillTarget);

        // 根据技能是否一定要指定敌人来构造 SpellSkil协议
        if (activeSkill.IsMustDesignation) {
            // 添加敌人的ID
            protocol.AddString(Target.NetWorkPlayerID);
        } else {
            // 添加目标位置
            protocol.AddFloat(position.x);
            protocol.AddFloat(position.y);
            protocol.AddFloat(position.z);
        }

        Debug.Log("发送SpellSkill协议");

        // 发送协议
        NetWorkManager.Instance.Send(protocol);
    }

    /// <summary>
    /// 用于处理SpellSkill协议
    /// </summary>
    /// <param name="protocol"></param>
    public void TreateSpellSkillProtocol(ProtocolBytes protocol) {

        // 释放技能的单位的ID
        string userName = protocol.GetString();

        // 不处理本地玩家的SpellSkill协议
        if ( userName == NetWorkManager.Instance.NowPlayerID ) return;

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

            skill.Execute(NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>(), new Vector3(x,y,z));

        } else {
            string targetId = protocol.GetString();

            skill.Execute(NetWorkManager.Instance.networkPlayers[userName].GetComponent<CharacterMono>(), NetWorkManager.Instance.networkPlayers[targetId].GetComponent<CharacterMono>());
        }
    }


    /// <summary>
    /// 技能学习同步,监听技能学习事件
    /// </summary>
    /// <param name="learner"></param>
    /// <param name="skill"></param>
    public void LearnSkillSynchronize(CharacterMono learner, BaseSkill skill) {
        // 当前要学习的技能的ID
        int skillID = skill.SkillID;

        // 构造协议
        ProtocolBytes protocolBytes = new ProtocolBytes();

        // 协议名
        protocolBytes.AddString("LearnSkill");

        // 协议参数
        protocolBytes.AddString(NetWorkManager.Instance.NowPlayerID);
        protocolBytes.AddInt(skillID);

        // 发送协议,告诉其他客户端,本地玩家NowPlayerID学习了X技能
        NetWorkManager.Instance.Send(protocolBytes);
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
    /// 获得物品同步,监听获得物品事件
    /// </summary>
    /// <param name="characterMono"></param>
    /// <param name="itemGrid"></param>
    public void GetItemSynchronize(CharacterMono characterMono, ItemGrid itemGrid) {        
        if (itemGrid.item == null) return;

        // 获得物品ID
        int itemId = itemGrid.item.ItemId;

        // 获得用户名
        string userName = NetWorkManager.Instance.NowPlayerID;

        // 构造获得物品协议
        ProtocolBytes protocolBytes = new ProtocolBytes();

        // 协议名
        protocolBytes.AddString("AddItem");

        // 参数
        protocolBytes.AddString(userName);
        protocolBytes.AddInt(itemId);
        protocolBytes.AddInt(itemGrid.ItemCount);

        // 发送协议
        NetWorkManager.Instance.Send(protocolBytes);
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
    /// 删除物品同步,监听删除物品事件
    /// </summary>
    /// <param name="characterMono"></param>
    /// <param name="itemGrid"></param>
    public void DeleteItemSynchronize(CharacterMono characterMono, ItemGrid itemGrid) {
        if (itemGrid.item == null) return;

        // 获得物品ID
        int itemId = itemGrid.item.ItemId;

        // 获得用户名
        string userName = NetWorkManager.Instance.NowPlayerID;

        // 构造获得物品协议
        ProtocolBytes protocolBytes = new ProtocolBytes();

        // 协议名
        protocolBytes.AddString("DeleteItem");

        // 参数
        protocolBytes.AddString(userName);
        protocolBytes.AddInt(itemId);

        // 发送协议
        NetWorkManager.Instance.Send(protocolBytes);
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
        for (int i=0;i<character.characterModel.itemGrids.Count;i++) {

            ItemGrid itemGrid = character.characterModel.itemGrids[i];

            if (itemGrid.item != null && itemGrid.item.ItemId == id) {
                itemGrid.ItemCount = 0;
                return;
            }
        }
    }
}

