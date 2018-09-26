using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 投射物基类
/// </summary>
public class ProjectileModel : CharacterModel{

    public Vector3 targetPosition;
    public Transform targetEnermyTransform;

    private Damage damage = null;

    public ProjectileModel(Damage damage) {
        this.damage = damage;
        canBeAttacked = false;
    }
    public ProjectileModel(CharacterModel character) {
        attack = character.attack;
        defense = character.defense;
        attackType = character.attackType;
        defenseType = character.defenseType;
        canBeAttacked = false;
    }

    /// <summary>
    /// 此投射物计算伤害的函数
    /// </summary>
    /// <param name="target"></param>
    public void Execute(CharacterModel target) {
        // 优先使用Damage类对伤害进行计算
        if (damage != null) {
            target.Damaged(damage);
        } else{
            Damage damage = new Damage {
                BaseDamage = attack
            };
            target.Damaged(damage);
        }
    }
}
