using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 技能组,包含一系列主动技能的主动技能,
/// 当此技能释放时,会执行该技能组所有技能的Execute(speller,target)方法,
/// 该类表示范围释放的技能组
/// </summary>
public class RangeSkillGroup : ActiveSkill{
    public override bool IsMustDesignation {
        get {
            return false ;
        }
    }

    //======================================
    // 此技能开放的接口
    public ActiveSkill[] activeSkills;

    public override void Execute(CharacterMono speller, Vector3 position) {
        base.Execute(speller, position);

        // 首先遍历技能组,释放该技能组中的所有无需对目标单位释放的技能
        foreach (var skill in activeSkills) {
            skill.Execute(speller,position);
        }

        // 在目标地点产生一个球形检测区域,半径由skillInflence决定,
        // 对检测区域内所有CharacterMono单位释放技能组中的单体技能
        Collider[] colliders = Physics.OverlapSphere(position,skillInfluenceRadius);
        foreach (var collider in colliders) {
            CharacterMono target = collider.GetComponent<CharacterMono>();
            if (target!=null) {
                foreach (var skill in activeSkills) {
                    skill.Execute(speller,target);
                }
            }
        }
    }
}

