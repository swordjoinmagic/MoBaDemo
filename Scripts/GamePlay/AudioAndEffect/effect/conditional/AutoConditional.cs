using System;
using UnityEngine;

/// <summary>
/// Auto型生命周期,当一个粒子特效播放完毕,
/// 他会自动消失
/// </summary>
public class AutoConditional : DuringConditional {

    private ParticleSystem particleSystem;

    public AutoConditional(ParticleSystem particleSystem) : base(particleSystem.main.duration) { }
}

