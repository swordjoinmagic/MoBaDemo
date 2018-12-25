using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 物品栏视图
/// </summary>
public class ItemListView : MonoBehaviour{

    // 和单位的ItemGrids数组一一对应
    public ItemPanelView[] itemViews;
    private Image[] maskImage;

    private CharacterMono characterMono;
    private List<ItemGrid> itemGrids;

    // 物品提示窗口预制体
    public ItemTipsView ItemTipsViewPrefab;
    // 物品提示窗口
    private ItemTipsView itemTipsView;

    private Canvas canvas;
    private Camera UICamera;

    public void Init(CharacterMono characterMono) {
        this.characterMono = characterMono;
        Init();
    }

    private void Init() {

        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        
        maskImage = new Image[itemViews.Count()];
        for (int i=0;i<itemViews.Count();i++) {
            var itemView = itemViews[i];
            maskImage[i] = itemView.transform.Find("ItemImagePanel").GetComponentInChildren<Mask>().GetComponent<Image>();
        }

        itemGrids = characterMono.characterModel.itemGrids;
        for (int i=0;i< itemViews.Count(); i++) {
            ItemPanelView itemPanelView = itemViews[i];
            ItemGrid itemGrid = itemGrids[i];

            itemPanelView.itemGrid = itemGrid;
            itemPanelView.BindingContext = new ItemViewModel();
            itemPanelView.BindingContext.Modify(itemGrid);

            // 为每一个ItemPanelView添加鼠标进入和离开事件

            // 鼠标进入事件
            var enterViewEntry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerEnter,
            };
            enterViewEntry.callback.AddListener(eventData => {
                if (itemGrid.item == null) return;

                if (itemTipsView == null) {
                    itemTipsView = GameObject.Instantiate<ItemTipsView>(ItemTipsViewPrefab, canvas.transform);
                    itemTipsView.BindingContext = new ItemViewModel();
                }

                // 设置提示窗口出现位置
                itemTipsView.transform.SetParent(itemPanelView.transform);
                (itemTipsView.transform as RectTransform).anchoredPosition = new Vector2((itemPanelView.transform as RectTransform).sizeDelta.x / 2, (itemPanelView.transform as RectTransform).sizeDelta.y / 2);
                itemTipsView.transform.SetParent(canvas.transform);

                itemTipsView.BindingContext.Modify(itemGrid);
                itemTipsView.Reveal();
            });

            // 鼠标离开事件
            var exitViewEntry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerExit
            };
            exitViewEntry.callback.AddListener(eventData => {
                if (itemGrid.item == null) return;
                itemTipsView.Hide(immediate: true);
            });


            // eventTrigger添加监听事件
            EventTrigger eventTrigger = itemPanelView.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(enterViewEntry);
            eventTrigger.triggers.Add(exitViewEntry);
        }
    }

    private void Update() {
        if (characterMono == null) return;
        for (int i= 0;i<6;i++) {

            ItemGrid itemGrid = itemGrids[i];

            Item item = itemGrid.item;

            if (item != null) {
                ActiveSkill activeSkill = item.itemActiveSkill;
                if (activeSkill == null) return;

                float coolDown = activeSkill.CD;
                //float finalSpellTime = activeSkill.FinalSpellTime;

                //float different = Time.time - finalSpellTime;

                float rate = 1 - Mathf.Clamp01(activeSkill.CDRate);

                maskImage[i].fillAmount = rate;
            }
        }
    }
}

