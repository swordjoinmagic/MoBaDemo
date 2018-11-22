using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using uMVVM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTipsMessageView : UnityGuiView<SkillPanelViewModel> {

    //=========================
    // 此View管理的UI控件
    public Text skillNameText;
    public Text mpText;
    public Text longDescriptionText;
    public Text levelText;
    public Text targetDescriptionText;
    public Text shortDescriptionText;
    public Text spellDistanceText;
    public Text duringTimeText;
    public Text CDText;
    public Text nextLevelText;

    //===================================
    // 用于高度自适应的控件
    public RectTransform SkillTargetDescriptionPanel;
    public RectTransform SkillDescriptionPanel;
    public RectTransform SkillShortDescriptionPanel;
    public RectTransform SkillRangeTimeLevelDescriptionPanel;
    public RectTransform SkillTipsPanel;
    public RectTransform SkillNameAndLevelPanel;

    protected override void OnInitialize() {
        base.OnInitialize();

        // 对UI控件绑定监听变化的函数
        binder.Add<string>("skillName", OnSkillNameChanged);
        binder.Add<string>("skillLevel", OnSkillLevelChanged);
        binder.Add<string>("mp", OnNeedMpChanged);
        binder.Add<string>("longDescription", OnDescriptinoChanged);
        binder.Add<string>("shortDescription",OnShortDescriptionChanged);
        binder.Add<string>("spellDistance",OnSpellDistanceChanged);
        binder.Add<string>("targetDescription",OnTargetDescriptionChanged);
        binder.Add<string>("nextLevel",OnNextLevelChanged);
    }

    // 技能名称
    private void OnSkillNameChanged(string oldSkillName, string newSkillName) {
        skillNameText.text = newSkillName;
    }

    // 消耗mp
    private void OnNeedMpChanged(string oldMp, string newMp) {
        mpText.text = newMp;
    }

    // 技能等级
    private void OnSkillLevelChanged(string oldSkillLevel, string newSkillLevel) {
        levelText.text = "Lv" + newSkillLevel;
    }

    // 技能描述
    private void OnDescriptinoChanged(string oldDescription, string newDescription) {
        //int cows = 0;
        longDescriptionText.text = "技能描述：\n" + newDescription;

        //Canvas.ForceUpdateCanvases();

        //SkillDescriptionPanel.sizeDelta = new Vector2(
        //    SkillDescriptionPanel.sizeDelta.x,
        //    (longDescriptionText.transform as RectTransform).sizeDelta.y+10);

        //if (newDescription == null || newDescription == "")
        //    cows = 0;
        //else
        //    cows = newDescription.Length / 15 + 2;

        //SkillDescriptionPanel.sizeDelta = new Vector2(
        //    SkillDescriptionPanel.sizeDelta.x,
        //    12.5f * cows
        //);
        //HeightAdaptive();
    }

    // 短技能描述
    private void OnShortDescriptionChanged(string oldDescription,string newDescription) {
        shortDescriptionText.text = newDescription;

        //Canvas.ForceUpdateCanvases();

        //SkillShortDescriptionPanel.sizeDelta = new Vector2(
        //    SkillShortDescriptionPanel.sizeDelta.x,
        //    (shortDescriptionText.transform as RectTransform).sizeDelta.y+10);

        //int cows = newDescription.Length / 15 + 1;
        //SkillShortDescriptionPanel.sizeDelta = new Vector2(
        //    SkillShortDescriptionPanel.sizeDelta.x,
        //    12.5f * cows
        //);
        //HeightAdaptive();
    }

    // 技能伤害,施加状态,作用目标等描述
    private void OnTargetDescriptionChanged(string oldDescription, string newDescription) {
        targetDescriptionText.text = newDescription;

        //Canvas.ForceUpdateCanvases();

        //SkillTargetDescriptionPanel.sizeDelta = new Vector2(
        //    SkillTargetDescriptionPanel.sizeDelta.x,
        //    (targetDescriptionText.transform as RectTransform).sizeDelta.y+10);

        //// 进行高度自适应，根据text的\n的数量计算Panel的高度
        //int cows = newDescription.FindAnyCharCount('\n')+1;
        //SkillTargetDescriptionPanel.sizeDelta = new Vector2(SkillTargetDescriptionPanel.sizeDelta.x,12.5f*cows);

        //HeightAdaptive();
    }

    // 技能施法范围描述
    private void OnSpellDistanceChanged(string oldDistance,string newDistance) {
        spellDistanceText.text = "施法范围："+newDistance;
    }

    // 下一等级描述
    private void OnNextLevelChanged(string oldLevel,string newLevel) {
        nextLevelText.text = "下一次升级在英雄<color=#ffA555>" + newLevel + "</color>级时"; 
    }

    /// <summary>
    /// 高度自适应
    /// </summary>
    public void HeightAdaptive() {
        // 技能长描述的位置随TargetPanel的高度而变化
        SkillDescriptionPanel.anchoredPosition = new Vector2(
            SkillDescriptionPanel.anchoredPosition.x,
            SkillTargetDescriptionPanel.anchoredPosition.y-SkillTargetDescriptionPanel.sizeDelta.y);

        // 技能短描述的位置随长描述的高度而变化
        SkillShortDescriptionPanel.anchoredPosition = new Vector2(
            SkillShortDescriptionPanel.anchoredPosition.x,
            SkillDescriptionPanel.anchoredPosition.y-SkillDescriptionPanel.sizeDelta.y
        );

        // 最后一个技能面板依赖于技能短描述的位置的变化
        SkillRangeTimeLevelDescriptionPanel.anchoredPosition = new Vector2(
            SkillRangeTimeLevelDescriptionPanel.anchoredPosition.x,
            SkillShortDescriptionPanel.anchoredPosition.y - SkillShortDescriptionPanel.sizeDelta.y
        );

        // 对整个面板的高度进行调整
        SkillTipsPanel.sizeDelta = new Vector2(
            SkillTipsPanel.sizeDelta.x,
            SkillNameAndLevelPanel.sizeDelta.y+
            SkillTargetDescriptionPanel.sizeDelta.y+
            SkillDescriptionPanel.sizeDelta.y+
            SkillShortDescriptionPanel.sizeDelta.y+
            SkillRangeTimeLevelDescriptionPanel.sizeDelta.y+
            18
        );
    }
}

