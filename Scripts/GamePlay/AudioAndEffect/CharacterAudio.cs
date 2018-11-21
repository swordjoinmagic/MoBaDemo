using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CharacterAudio{
    // 用于保存单位身上的音频
    private Dictionary<string, AudioClip> audios;

    /// <summary>
    /// 初始化时，根据目标单位的音频属性，对AudioClip进行读取，并保存至字典对应的键中
    /// </summary>
    public CharacterAudio(CharacterModel characterModel) {
        audios = new Dictionary<string, AudioClip>();

        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/"+characterModel.attackAudioClip);
        audios["AttackClip"] = audioClip;
    }
}

