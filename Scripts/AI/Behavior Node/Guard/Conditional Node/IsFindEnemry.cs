using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[TaskCategory("guard Action")]
public class IsFindEnemry : Conditional{

    private CharacterMono characterMono;

    // 要攻击的敌人
    public SharedGameObject target;

    public SharedBool isFindEnermy;


    public override void OnAwake() {
        characterMono = GetComponent<CharacterMono>();
    }

    // 获得当前单位与目标单位的平方距离(欧几里得距离)
    private float GetDistance(CharacterMono unit) {
        float x1 = unit.transform.position.x;
        float x2 = characterMono.transform.position.x;
        float z1 = unit.transform.position.z;
        float z2 = characterMono.transform.position.z;

        return (x2 - x1) * (x2 - x1) + (z2 - z1) * (z2 - z1);
    }

    /* 选择一个敌人来进攻    
    单位剩余生命值（优先攻击声明值少的） - 30%
    单位魔法值（优先攻击魔法值多的） - 20%
    单位是英雄单位（优先攻击英雄单位） - 20%
    单位距离最近（优先攻击距离近的单位）- 30%
    */
    public CharacterMono ChooseOneEnemry() {
        try {

            // 单位权值数组
            float[] enemryValue = new float[characterMono.arroundEnemies.Count];
            // 目前最高分数
            float maxValue = 0;
            // 目前分数最高的单位的下标
            int maxIndex = 0;

            for (int i=0;i<characterMono.arroundEnemies.Count;i++) {

                CharacterMono unit = characterMono.arroundEnemies[i];

                // 计算单位剩余生命值的权重
                enemryValue[i] += (1 - (float)unit.characterModel.Hp / (float)unit.characterModel.maxHp) * 3f;
                // 计算单位剩余魔法值的权重
                enemryValue[i] += (1 - (float)unit.characterModel.Mp / (float)unit.characterModel.maxMp) * 2f;
                // 英雄单位权重
                enemryValue[i] += unit is HeroMono ? 2f : 0;
                // 单位距离权重(不开根号比较)
                enemryValue[i] += GetDistance(unit) / 100f * 3;

                if (enemryValue[i] > maxValue) {
                    maxValue = enemryValue[i];
                    maxIndex = i;
                }
            }

            return characterMono.arroundEnemies[maxIndex];

        } catch (System.Exception e) {
            Debug.LogWarning("AI单位-"+characterMono.name+"在选择敌人时出错！");
            return null;
        }
    }

    public override TaskStatus OnUpdate() {

        //Debug.Log("IsFindEnemry ConditonalNode Running");

        if (characterMono.arroundEnemies.Count > 0) {

            CharacterMono enemry = ChooseOneEnemry();

            if (enemry == null) return TaskStatus.Failure;

            target.Value = enemry.gameObject;
            isFindEnermy.Value = true;
            
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}

