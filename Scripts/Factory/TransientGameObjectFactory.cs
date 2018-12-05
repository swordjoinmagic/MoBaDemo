using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 临时GameObject对象工厂
/// </summary>
public class TransientGameObjectFactory{

    /// <summary>
    /// 获得一个临时GameObject对象,并为其添加EffectsLifeCycle脚本管理其生命周期
    /// </summary>
    /// <param name="templateObject">模板对象</param>
    /// <returns></returns>
    public static EffectsLifeCycle AcquireObject(EffectConditonalType type,GameObject templateObject=null,BattleState battleState = null, CharacterMono target = null, float during = -1, ParticleSystem particleSystem = null) {
        GameObject result = null;

        if (templateObject == null) {
            result = new GameObject();
        } else {
            result = GameObject.Instantiate(templateObject);
        }

        EffectsLifeCycle effectsLifeCycle = result.AddComponent<EffectsLifeCycle>();
        effectsLifeCycle.Init(type,battleState,target,during,particleSystem);
        

        return effectsLifeCycle;
    }
}
