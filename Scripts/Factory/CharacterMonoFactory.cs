using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 产生CharacterMono的工厂类
/// </summary>
public class CharacterMonoFactory {
    /// <summary>
    /// 获得一个
    /// </summary>
    /// <param name="templateObjectt"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static CharacterMono AcquireObject(CharacterModel characterModel,GameObject templateObjectt, Vector3 position) {
        GameObject gameObject = GameObject.Instantiate(templateObjectt,position,Quaternion.identity);
        CharacterMono characterMono = gameObject.GetComponent<CharacterMono>();
        if (characterMono == null)
            characterMono = gameObject.AddComponent<CharacterMono>();
        // 赋值CharacterModel属性
        characterMono.characterModel = characterModel.DeepCopy();        
        // 更改GameObject的名字
        characterMono.name = characterModel.Name;

        // 将新创建的单位增加到战争迷雾系统中
        FogSystem.Instace.AddFOVUnit(characterMono);

        return characterMono;
    }
}

