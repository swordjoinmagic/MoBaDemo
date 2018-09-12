using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 判断准备释放的技能是不是指向性技能的行为树节点Conditional
/// </summary>
class IsPrepareSkillIsPointingSkill : Conditional{
    private CharacterMono characterMono;

    public override void OnStart() {
        characterMono = GetComponent<CharacterMono>();
    }

    public override TaskStatus OnUpdate() {
        ActiveSkill activeSkill = characterMono.prepareSkill;

        Debug.Log( activeSkill.SpellDistance != 0 ? "是指向性技能":"不是指向性技能");

        // 如果释放范围!=0,说明此技能是指向性技能
        if (activeSkill.SpellDistance != 0) {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}

