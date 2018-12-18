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
        characterMono.OnMove += TransformSynchronize;
        characterMono.OnPlayAnimation += AniamtionSynchronize;
        characterMono.characterModel.LevelChangedHandler += LevelSynchronize;
    }

    private float time;

    // 位置同步
    public void TransformSynchronize(CharacterMono characterMono, Vector3 pos) {
        //time += Time.smoothDeltaTime;
        //if (time >= 0.1f) {
        NetWorkManager.Instance.SendPos();
        //Debug.Log("单位正在移动");
        //time = 0;
        //}
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
}

