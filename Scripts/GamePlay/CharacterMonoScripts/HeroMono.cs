using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 英雄单位的Mono类
/// </summary>
public class HeroMono : CharacterMono{
    public HPViewModel HPViewModel;

    public HeroModel HeroModel {
        get {
            return characterModel as HeroModel;
        }
    }

    public override void Update() {
        base.Update();
    }

    #region 对经验值改变和升级事件进行监听
    protected override void Bind() {
        base.Bind();
        HeroModel.ExpChangedHandler += OnExpChanged;
        HeroModel.LevelChangedHandler += OnLevelChanged;
    }
    public void OnExpChanged(int oldExp,int newExp) {

        // 下一级所需经验
        int nextLevelNeedExp = HeroModel.NextLevelNeedExp;

        // 获得当前经验比率
        int expRate = Mathf.Clamp(Mathf.FloorToInt( ( (float)newExp / HeroModel.NextLevelNeedExp ) * 100 ),0,100);

        //==============================
        // 当经验值满值时,修改等级
        if (expRate == 100) {
            HeroModel.Level += 1;

            // 由于升级的缘故,当前经验值减去之前升级所需经验
            // 可以看作是消耗了这些经验导致升级
            HeroModel.Exp -= nextLevelNeedExp;
        }        
    }
    public void OnLevelChanged(int oldLevel, int newLevel) {
        // 升级时，单位获得技能点
        if (newLevel > oldLevel) {
            HeroModel.SkillPoint += HeroModel.SkillPointGrowthPoint * (newLevel - oldLevel);
        }        
    }
    #endregion
}

