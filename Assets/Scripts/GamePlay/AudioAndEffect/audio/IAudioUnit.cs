using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 此接口用于表示具有发声功能的单位
/// </summary>
public interface IAudioUnit {
    string AttackAudioPath {
        get; set;
    }
    string MoveAudioPath {
        get;set;
    }
}

