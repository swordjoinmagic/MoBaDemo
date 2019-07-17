using UnityEngine;

/// <summary>
/// During型生命周期,在此生命周期中,
/// 当粒子特效出现时间大于during时间,
/// 就会自动消失
/// </summary>
public class DuringConditional : EffectConditional {

    private float during = 0;

    public DuringConditional(float during) {
        this.during = during;
    }

    public override bool IsValid() {
        // 如果该粒子特效所经历的时间大于规定的时间(during),表示,此时
        // 该粒子是非法的
        if (time>=during) {
            return false;
        }
        return true;
    }
}

