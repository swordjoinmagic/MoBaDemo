using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using uMVVM;
using UnityEngine.EventSystems;

/// <summary>
/// 用于管理一个连续多个技能的技能面板
/// </summary>
public class SkillView : MonoBehaviour{
    private HeroMono characterMono;
    private HeroModel character;
    private List<ActiveSkill> activeSkills;

    public List<SkillPanelView> skillPanelViews;

    // 技能信息提示窗口预制体
    public SkillTipsMessageView skillTipsMessageViewPrefab;
    // 技能提示窗口
    private SkillTipsMessageView skillTipsMessageView;

    //===============================
    // UI控件
    public List<Image> images;  // 冷却用Mask
    public CanvasGroup skillLevelUpPanel;
    public List<Button> skillLevelButtons;


    private Camera UICamera;
    private Canvas canvas;

    #region 绑定SkillPoint改变的事件
    private void ChangedSkillLevelUpPanelActive(bool isShow) {
        if (isShow) {
            skillLevelUpPanel.alpha = 1;
            skillLevelUpPanel.transform.localScale = Vector3.one;

            // 遍历所有技能,当该技能目前英雄等级可以学习时,就出现技能升级按钮
            for(int i=0;i<skillLevelButtons.Count;i++) {

                BaseSkill skill = character.BaseSkills[i];

                if (character.Level >= skill.NextLevelNeedHeroLevel) {
                    skillLevelButtons[i].gameObject.SetActive(true);
                }
            }

        } else {
            skillLevelUpPanel.alpha = 0;
            skillLevelUpPanel.transform.localScale = Vector3.zero;

            // Rest
            foreach (var button in skillLevelButtons)
                button.gameObject.SetActive(false);
        }
    }

    private void OnSkillPointChanged(int oldSkillPoint, int newSkillPoint) {
        ChangedSkillLevelUpPanelActive(newSkillPoint > 0);
    }

    private void Bind() {
        characterMono.HeroModel.SkillPointChangedHandler += OnSkillPointChanged;
    }
    #endregion

    /// <summary>
    /// 在初始化时，
    /// 根据单位的技能列表（BaseSkills），设置各个技能面板
    /// </summary>
    public void Init(HeroMono characterMono) {
        this.characterMono = characterMono;
        character = characterMono.HeroModel;
        Init();
    }
    
    private void Init() {

        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        Bind();

        for (int i=0;i<skillPanelViews.Count;i++) {
            // 对每一个技能面板进行初始化
            SkillPanelView skillPanelView = skillPanelViews[i];
            BaseSkill baseSkill = characterMono.characterModel.BaseSkills[i];
            skillPanelView.BindingContext = new SkillPanelViewModel();
            skillPanelView.BindingContext.Modify(baseSkill);

            #region 设置每个技能面板的鼠标事件

            // 鼠标悬停事件
            var enterViewEntry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerEnter,
            };
            enterViewEntry.callback.AddListener(eventData => {
                if (skillTipsMessageView == null) {
                    skillTipsMessageView = GameObject.Instantiate<SkillTipsMessageView>(skillTipsMessageViewPrefab, canvas.transform);
                    skillTipsMessageView.BindingContext = new SkillPanelViewModel();
                }

                // 设置提示窗口出现位置
                skillTipsMessageView.transform.SetParent(skillPanelView.transform);
                (skillTipsMessageView.transform as RectTransform).anchoredPosition = new Vector2((skillPanelView.transform as RectTransform).sizeDelta.x/2, (skillPanelView.transform as RectTransform).sizeDelta.y/2);
                skillTipsMessageView.transform.SetParent(canvas.transform);

                skillTipsMessageView.BindingContext.Modify(baseSkill);
                skillTipsMessageView.Reveal();
            });

            // 鼠标离开事件
            var exitViewEntry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerExit
            };
            exitViewEntry.callback.AddListener(eventData => {
                skillTipsMessageView.Hide(immediate:true);
            });

            // eventTrigger添加监听事件
            EventTrigger eventTrigger = skillPanelView.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(enterViewEntry);
            eventTrigger.triggers.Add(exitViewEntry);
            #endregion
        }

        #region 设置各个升级按钮的click事件
        for (int i=0;i<skillLevelButtons.Count;i++) {
            // 升级按钮和技能一一对应
            Button levelUpButtonn = skillLevelButtons[i];
            BaseSkill skill = characterMono.characterModel.BaseSkills[i];
            levelUpButtonn.onClick.AddListener(() => {
                if (skill.NextLevelNeedHeroLevel <= character.Level && character.SkillPoint > 0) {
                    skill.SkillLevel += 1;
                    character.SkillPoint -= 1;
                }
            });
        }
        #endregion
    }

    private void Update() {
        if (characterMono == null) return;

        character = characterMono.HeroModel;
        activeSkills = character.activeSkills;

        for (int i=0;i<activeSkills.Count;i++) {

            ActiveSkill activeSkill = activeSkills[i];
            if (activeSkill.SkillLevel == 0) continue;  // 0级的技能是还没有学会的技能
            float coolDown = activeSkill.CD;

            float rate = 1 - Mathf.Clamp01(activeSkill.CDRate);
            images[i].fillAmount = rate;
        }
    }
}

