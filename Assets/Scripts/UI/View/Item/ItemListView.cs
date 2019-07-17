using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

    public Color pressColor = Color.grey;
    public Vector2 pressSizeData = new Vector2(0.95f,0.95f);
     
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
            // 也就是显示Tips视图的事件
            BindTipsViewEvent(itemGrid,itemPanelView);

            // 为每一个ItemPanelView添加鼠标点击事件(拿起物品和使用物品)
            BindPickUpItemEvent(itemGrid,itemPanelView);
        }
    }

    private void BindTipsViewEvent(ItemGrid itemGrid,ItemPanelView itemPanelView) {
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

    private void BindPickUpItemEvent(ItemGrid itemGrid,ItemPanelView itemPanelView) {
        // 鼠标单击事件
        var pointerDownEntry = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerDown,
        };
        pointerDownEntry.callback.AddListener(eventData => {
            if (characterMono == null) {
                return;
            }

            itemPanelView.GetComponent<Image>().color = pressColor;
            (itemPanelView.transform as RectTransform).localScale = pressSizeData;
        });

        var pointerUpEntry = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener(eventData => {
            if (characterMono == null) {
                return;
            }

            // 检测MouseUp,来判断当前玩家用的是鼠标左键还是右键
            if (Input.GetMouseButtonUp(0)) {

                // 鼠标左键
                if (characterMono.IsPickUpItem) {
                    // 如果此时玩家已经拿着物品了，那么将该物品放置到指定的格子上
                    characterMono.PlaceItem(itemGrid);
                }

            } else if (Input.GetMouseButtonUp(1)) {
                // 鼠标右键

                // 拿起该物品,具体表现为该物品的缩小UI一直跟随鼠标
                if (!characterMono.IsPickUpItem) {
                    characterMono.PickUpItem(itemGrid);
                    
                }

            }

            itemPanelView.GetComponent<Image>().color = Color.white;
            (itemPanelView.transform as RectTransform).localScale = Vector2.one;
        });

        EventTrigger eventTrigger = itemPanelView.GetComponent<EventTrigger>();
        eventTrigger.triggers.Add(pointerDownEntry);
        eventTrigger.triggers.Add(pointerUpEntry);
    }

    private void Update() {
        if (characterMono == null) return;

        // 处理每个物品的冷却事件(即显示当前冷却情况)
        TreateCoolDown();
    }

    /// <summary>
    /// 处理物品的冷却
    /// </summary>
    private void TreateCoolDown() {       
        for (int i = 0; i < 6; i++) {
            ItemGrid itemGrid = itemGrids[i];
            Item item = itemGrid.item;

            if (item != null) {
                ActiveSkill activeSkill = item.itemActiveSkill;
                if (activeSkill == null) continue;

                float coolDown = activeSkill.CD;

                float rate = 1 - Mathf.Clamp01(activeSkill.CDRate);

                maskImage[i].fillAmount = rate;
            } else {
                maskImage[i].fillAmount = 0;
            }
        }
    }
}

