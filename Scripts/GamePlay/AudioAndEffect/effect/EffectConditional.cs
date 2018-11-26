using UnityEngine;

public abstract class EffectConditional {

    /// <summary>
    /// 目前此粒子特效所经历的时间
    /// </summary>
    protected float time = 0;

    /// <summary>
    /// 判断某特效对象是否存在合法
    /// </summary>
    /// <returns></returns>
    public abstract bool IsValid();

    /// <summary>
    /// 更新该粒子特效的存活时间
    /// </summary>
    public virtual void Update() {
        time += Time.smoothDeltaTime;
    }
}

