using UnityEngine;

public class SynchronizeNPC {

    private CharacterMono NPC;

    public SynchronizeNPC(CharacterMono characterMono) {
        NPC = characterMono;
        Init(NPC);
    }

    public void Init(CharacterMono characterMono) {
        characterMono.OnMove += TransformSynchronize;
        characterMono.OnPlayAnimation += AniamtionSynchronize;
        characterMono.OnSpell += SpellSkillSynchronize;
    }

    // 位置同步
    public void TransformSynchronize(CharacterMono characterMono, Vector3 pos) {
        NetWorkManager.Instance.SendNpcPos(characterMono.NetWorkPlayerID);
    }

    // 动画同步
    public void AniamtionSynchronize(CharacterMono characterMono, string operation) {
        // 构造协议
        ProtocolBytes protocol = new ProtocolBytes();

        // 协议名
        protocol.AddString("AnimationOperation");

        // 参数
        protocol.AddString(characterMono.NetWorkPlayerID);
        protocol.AddString(operation);

        Debug.Log("NPC同步动画:"+operation);
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
        protocol.AddString(NPC.NetWorkPlayerID);
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

        Debug.Log("NPC发送SpellSkill协议");

        // 发送协议
        NetWorkManager.Instance.Send(protocol);
    }
}

