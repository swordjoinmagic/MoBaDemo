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
class SkillView : MonoBehaviour{
    public HeroMono characterMono;
    private HeroModel character;
    private List<ActiveSkill> activeSkills;

    public List<SkillPanelView> skillPanelViews;


    // 技能信息提示窗口预制体
    public SkillTipsMessageView skillTipsMessageViewPrefab;
    // 技能提示窗口
    private SkillTipsMessageView skillTipsMessageView;

    //===============================
    // UI控件
    public List<Image> images;
    public CanvasGroup skillLevelUpPanel;
    public List<Button> skillLevelButtons;


    private Camera UICamera;
    private Canvas canvas;

    #region 绑定SkillPoint改变的事件
    private void ChangedSkillLevelUpPanelActive(bool isShow) {
        if (isShow) {
            skillLevelUpPanel.alpha = 1;
            skillLevelUpPanel.transform.localScale = Vector3.one;
        } else {
            skillLevelUpPanel.alpha = 0;
            skillLevelUpPanel.transform.localScale = Vector3.zero;
        }
    }

    private void OnSkillPointChanged(int oldSkillPoint, int newSkillPoint) {
        if (newSkillPoint > 0) {
            ChangedSkillLevelUpPanelActive(true);
        } else if (newSkillPoint == 0) {
            ChangedSkillLevelUpPanelActive(false);
        }
    }

    private void Bind() {
        characterMono.HeroModel.SkillPointChangedHandler += OnSkillPointChanged;
    }
    #endregion

    private void Start() {

        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        Bind();

        for (int i=0;i<skillPanelViews.Count;i++) {
            // 对每一个技能面板进行初始化
            SkillPanelView skillPanelView = skillPanelViews[i];
            BaseSkill baseSkill = characterMono.characterModel.baseSkills[i];
            skillPanelView.BindingContext = new SkillPanelViewModel();
            skillPanelView.BindingContext.Modify(baseSkill);

            // 监听EventTrigger控件事件

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
        }

        for (int i=0;i<skillLevelButtons.Count;i++) {
            Button levelUpButtonn = skillLevelButtons[i];
            BaseSkill skill = characterMono.characterModel.baseSkills[i];
            levelUpButtonn.onClick.AddListener(() => {
                if (skill.NextLevelNeedHeroLevel <= character.Level && character.SkillPoint > 0) {
                    skill.SkillLevel += 1;
                    character.SkillPoint -= 1;
                }
            });
        }
    }

    private void Update() {

        character = characterMono.HeroModel;
        activeSkills = character.activeSkills;

        for (int i=0;i<activeSkills.Count;i++) {

            ActiveSkill activeSkill = activeSkills[i];
            if (activeSkill.SkillLevel == 0) continue;
            float coolDown = activeSkill.CD;
            //float finalSpellTime = activeSkill.FinalSpellTime;

            //float different = Time.time - finalSpellTime;

            float rate = 1 - Mathf.Clamp01(activeSkill.CDRate);
            images[i].fillAmount = rate;
        }
    }
}

