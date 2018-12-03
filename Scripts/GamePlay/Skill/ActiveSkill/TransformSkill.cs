using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 位移技能，在N秒内将单位位移至目标地点，时间为0时，表示是闪现技能
/// </summary>
public class TransformSkill : ActiveSkill{
    public override bool IsMustDesignation {
        get {
            return false;
        }
    }

    public override void Execute(CharacterMono speller, Vector3 position) {
        base.Execute(speller, position);

        speller.transform.position = position;
    }
}

