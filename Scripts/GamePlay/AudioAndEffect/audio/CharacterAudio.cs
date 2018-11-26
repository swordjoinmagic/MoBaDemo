using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 单位的声音模块，基于Bind方法对各种发出声音的事件进行订阅，当事件发生时，发出声音
/// 从而达到跟其他模块解耦的目的
/// </summary>
public class CharacterAudio{

    // 用于保存单位身上的音频
    private Dictionary<UnitAudioType, AudioClip> audios;

    /// <summary>
    /// 初始化时，根据目标单位的音频属性，对AudioClip进行读取，并保存至字典对应的键中
    /// </summary>
    public CharacterAudio(CharacterModel characterModel) {
        audios = new Dictionary<UnitAudioType, AudioClip>();

        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/"+characterModel.AttackAudioPath);
        audios[UnitAudioType.AttackAudio] = audioClip;
        audioClip = Resources.Load<AudioClip>("Sounds/" + characterModel.MoveAudioPath);
        audios[UnitAudioType.MoveAudio] = audioClip;
    }

    /// <summary>
    /// 向单位固定事件进行订阅，当事件发生时，声音模块将发出声音
    /// </summary>
    public void Bind(CharacterMono target,AudioSource audioSource) {
        try {
            target.OnAttack += (CharacterMono Attacker, CharacterMono Suffered, Damage damage) => {
                audioSource.clip = audios[UnitAudioType.AttackAudio];
                audioSource.Play();
            };
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }
}

