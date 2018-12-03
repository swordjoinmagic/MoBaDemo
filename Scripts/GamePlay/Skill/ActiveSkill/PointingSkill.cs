using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//=================================
// ACTIVESKILL TYPE : 指向伤害技能类

/// <summary>
/// 指向伤害技能类,此技能要求单击一个目标进行释放,对目标直接造成伤害
/// </summary>
public class PointingSkill : ActiveSkill{
    public override void Execute(CharacterMono spller, CharacterMono target) {

        base.Execute(spller,target);

        target.characterModel.Damaged(new Damage() { BaseDamage=BaseDamage,PlusDamage=PlusDamage });
    }
}

