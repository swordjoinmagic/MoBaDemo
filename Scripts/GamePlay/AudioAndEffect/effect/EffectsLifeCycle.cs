using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 用于管理特效对象的生命周期
/// </summary>
public class EffectsLifeCycle : MonoBehaviour{

    public EffectConditional conditional;

    public void Init(EffectConditonalType type,BattleState battleState=null,CharacterMono target=null,float during=-1,ParticleSystem particleSystem=null) {
        switch (type) {
            case EffectConditonalType.Auto:
                Assert.IsNotNull(particleSystem,"生成了一个Auto型生命周期的特效对象,但是没有为该对象赋值粒子特效系统");
                conditional = new AutoConditional(particleSystem);
                break;
            case EffectConditonalType.BattleState:
                Assert.IsNotNull(battleState, "生成了一个Battle型生命周期的特效对象,但是没有为该对象赋值 battle 属性");
                Assert.IsNotNull(target, "生成了一个Battle型生命周期的特效对象,但是没有为该对象赋值 target 属性");
                conditional = new StateConditional(target,battleState);
                break;
            case EffectConditonalType.During:
                Assert.AreNotEqual<float>(-1,during, "生成了一个During型生命周期的特效对象,但是没有为该对象赋值 during 属性");
                conditional = new DuringConditional(during);
                break;
        }
    }

    private void Update() {
        if (conditional == null) return;
        conditional.Update();

        // 如果粒子对象不合法,删除此特效对象
        if (!conditional.IsValid()) {
            Destroy(this.gameObject);
        }
    }
}

