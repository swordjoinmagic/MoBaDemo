using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 持续施法技能组
/// </summary>
public class ContinuousRangeSkillGroup : ActiveSkill{
    public override bool IsMustDesignation {
        get {
            return false;
        }
    }

    //======================================================
    // 此技能开放的接口
    public ActiveSkill[] activeSkills;                  // 技能组所包含的所有主动技能
    public SkillDelayAttribute[] skillDelayAttributes;  // 技能的延迟属性

    // 已经经过的时间
    private float time;

    public override bool ContinuousExecute(CharacterMono speller, Vector3 position) {

        time += Time.smoothDeltaTime;
        if (time > SpellDuration) {
            time = 0;
            return true;
        }

        // 每经过一秒钟造成一次技能效果
        if (Time.time - finalSpellTime >= 1 || (time- Time.smoothDeltaTime) == 0) {            
            finalSpellTime = Time.time;
            CreateEffect(speller, position);
            Execute(speller, position);                      
        }
        return false;
    }

    public override void Execute(CharacterMono speller, Vector3 position) {
        base.Execute(speller, position);

        //========================================
        // 为延迟技能(只能对非指向型技能)增加监听事件
        for (int i = 0; i < activeSkills.Count(); i++) {
            var skill = activeSkills[i];
            var delayAttribute = skillDelayAttributes[i];
            if (delayAttribute.isDelay && !skill.IsMustDesignation) {
                var activeSkill = activeSkills[delayAttribute.index];
                if (!activeSkill.IsMustDesignation) {
                    OnSkillCompeleteHandler delayExcute = null;
                    delayExcute = () => {
                        skill.Execute(speller, position);
                    };
                    activeSkill.OnCompelte += delayExcute;
                }
            }
        }



        // 首先遍历技能组,释放该技能组中的所有无需对目标单位释放的技能
        for (int i = 0; i < activeSkills.Count(); i++) {
            var skill = activeSkills[i];
            var delayAttribute = skillDelayAttributes[i];
            if (!skill.IsMustDesignation && !delayAttribute.isDelay)
                skill.Execute(speller, position);
        }

        // 在目标地点产生一个球形检测区域,半径由skillInflence决定,
        // 对检测区域内所有CharacterMono单位释放技能组中的单体技能
        Collider[] colliders = Physics.OverlapSphere(position, skillInfluenceRadius);
        foreach (var collider in colliders) {
            CharacterMono target = collider.GetComponent<CharacterMono>();
            if (target != null) {
                foreach (var skill in activeSkills) {
                    if (skill.IsMustDesignation)
                        skill.Execute(speller, target);
                }
            }
        }
    }

}

