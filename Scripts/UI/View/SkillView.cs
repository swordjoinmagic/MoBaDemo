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
    public CharacterMono characterMono;
    private CharacterModel character;
    private List<ActiveSkill> activeSkills;

    public List<SkillPanelView> skillPanelViews;


    // 技能信息提示窗口预制体
    public SkillTipsMessageView skillTipsMessageViewPrefab;
    // 技能提示窗口
    private SkillTipsMessageView skillTipsMessageView;

    //===============================
    // UI控件
    public List<Image> images;

    private Camera UICamera;
    private Canvas canvas;

    private void Start() {

        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for (int i=0;i<skillPanelViews.Count;i++) {

            // 对每一个技能面板进行初始化
            SkillPanelView skillPanelView = skillPanelViews[i];
            BaseSkill baseSkill = characterMono.characterModel.activeSkills[i];
            skillPanelView.BindingContext = new SkillPanelViewModel();
            skillPanelView.BindingContext.Modify(baseSkill);

            // 监听EventTrigger控件事件

            // 鼠标悬停事件
            var enterViewEntry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerEnter,
            };
            enterViewEntry.callback.AddListener(eventData => {
                Debug.Log("鼠标进入" + skillPanelView.name + "号技能");
                if (skillTipsMessageView == null) {
                    skillTipsMessageView = GameObject.Instantiate<SkillTipsMessageView>(skillTipsMessageViewPrefab, canvas.transform);
                    skillTipsMessageView.BindingContext = new SkillPanelViewModel();
                }

                // 获得当前鼠标所在位置在UICamera摄像机下的世界坐标
                Vector3 vector3 = UICamera.ScreenToWorldPoint(Input.mousePosition);
                vector3.z = 100;
                // 设置提示窗口出现位置
                skillTipsMessageView.transform.position = vector3;

                skillTipsMessageView.BindingContext.Modify(baseSkill);
                skillTipsMessageView.Reveal();
            });

            // 鼠标离开事件
            var exitViewEntry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerExit
            };
            exitViewEntry.callback.AddListener(eventData => {
                skillTipsMessageView.Hide(immediate:true);
                Debug.Log("鼠标离开"+ skillPanelView.name + "号技能");
            });


            // eventTrigger添加监听事件
            EventTrigger eventTrigger = skillPanelView.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(enterViewEntry);
            eventTrigger.triggers.Add(exitViewEntry);
        }
    }

    private void Update() {

        character = characterMono.characterModel;
        activeSkills = character.activeSkills;

        for (int i=0;i<activeSkills.Count;i++) {

            ActiveSkill activeSkill = activeSkills[i];

            float coolDown = activeSkill.CD;
            float finalSpellTime = activeSkill.FinalSpellTime;

            float different = Time.time - finalSpellTime;

            float rate = 1 - Mathf.Clamp01(different/coolDown);
            images[i].fillAmount = rate;
        }
    }
}

