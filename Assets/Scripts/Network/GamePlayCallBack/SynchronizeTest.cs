using UnityEngine;

public class SynchronizeTest {

    private CharacterMono characterMono;

    public SynchronizeTest(CharacterMono characterMono) {
        this.characterMono = characterMono;
        Init(characterMono);
    }

    public void Init(CharacterMono characterMono) {
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

}

