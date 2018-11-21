using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

class StoreView : MonoBehaviour{

    //==================================================
    // 商店对应的HeroMono类，在游戏初始化时对其进行初始化
    public HeroMono heroMono;

    /// <summary>
    ///  商店逻辑类
    /// </summary>
    private StoreLogic storeLogic;

    // 目前用于展示的分类类型
    private CommditType showCommidtyType = CommditType.All;
    // 用于展示的物品总数
    private List<ItemGrid> showItemGrids;
    private Camera UICamera;
    private Canvas canvas;

    //===============================
    // 此View管理的UI控件
    private CanvasGroup canvasGroup;
    private List<StoreItemPanelView> itemGridsView = new List<StoreItemPanelView>();
    public Transform contentRectTransform;
    public StoreItemPanelView itemPanelViewPrefab;
    public ItemTipsView ItemTipsViewPrefab;
    private ItemTipsView itemTipsView;

    // 用于分类的按钮,0:all,1:weaponButton，顺序和Commditype的枚举值一致
    public Button[] categoryButtons;
    public Button OnOrOffButton;
    public Text OnOrOffButtonText;

    /// <summary>
    /// 用于初始化
    /// </summary>
    private void Start() {
        canvas = GameObject.FindObjectOfType<Canvas>();
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        canvasGroup = GetComponent<CanvasGroup>();
        storeLogic = new StoreLogic();
        BindButtonEvent();
        BindCanBuyButtonEvent();
        Reveal();
    }

    /// <summary>
    /// 显示这个StoreView的方法，
    /// 在显示的同时，根据当前的分类category来显示所有物品
    /// </summary>
    public void Reveal() {
        (transform as RectTransform).DOSizeDelta(new Vector2(250f, (transform as RectTransform).sizeDelta.y),1f);
        canvasGroup.DOFade(1,1f);
        //transform.localScale = Vector3.one;

        UpdateShowCommdities(showCommidtyType);
    }

    public void Hide() {
        (transform as RectTransform).DOSizeDelta(new Vector2(0, (transform as RectTransform).sizeDelta.y), 1f);
        canvasGroup.DOFade(0, 1f);
    }

    /// <summary>
    /// 更新用于展示的商品列表
    /// </summary>
    public void UpdateShowCommdities(CommditType commidtyType) {
        showItemGrids = storeLogic.FindItemsWithCommidtyType(commidtyType);

        int i = 0;
        for (;i<showItemGrids.Count;i++) {
            ItemGrid itemGrid = showItemGrids[i];
            itemGrid.index = i;
            itemGrid.CanBuy = storeLogic.IsCanBuyItem(itemGrid,heroMono);
            // 如果当前ItemPanelView够用就用之前的创建的
            // 当不够用的时候,创建新的ItemPanelView
            if (i >= itemGridsView.Count) {
                CreateItemPanel(itemGrid);
                itemGridsView[i].BindingContext = new ItemViewModel();
            }
            itemGridsView[i].Reveal();
            itemGridsView[i].BindingContext.Modify(itemGrid);
            SetItemPanelMouseEvent(itemGrid,itemGridsView[i]);
        }
        for (;i<itemGridsView.Count;i++) {
            itemGridsView[i].Hide();
        }
    }

    private void SetItemPanelMouseEvent(ItemGrid itemGrid,StoreItemPanelView itemPanel) {
        EventTrigger.Entry onMouseEnter = new EventTrigger.Entry();
        onMouseEnter.eventID = EventTriggerType.PointerEnter;
        onMouseEnter.callback.AddListener(eventData => {
            if (itemGrid.item == null) return;

            if (itemTipsView == null) {
                itemTipsView = GameObject.Instantiate<ItemTipsView>(ItemTipsViewPrefab, canvas.transform);
                itemTipsView.BindingContext = new ItemViewModel();
            }

            // 获得当前鼠标所在位置在UICamera摄像机下的世界坐标
            Vector3 vector3 = UICamera.ScreenToWorldPoint(Input.mousePosition);
            vector3.z = 100;
            // 设置提示窗口出现位置
            itemTipsView.transform.position = vector3;


            itemTipsView.BindingContext.Modify(itemGrid);
            itemTipsView.Reveal();
        });
        EventTrigger.Entry onMouseExit = new EventTrigger.Entry();
        onMouseExit.eventID = EventTriggerType.PointerExit;
        onMouseExit.callback.AddListener(eventData => {
            if (itemGrid.item == null) return;
            itemTipsView.Hide(immediate: true);
        });

        // 右键单击购买物品的事件
        EventTrigger.Entry onMouseClick = new EventTrigger.Entry();
        onMouseClick.eventID = EventTriggerType.PointerClick;
        onMouseClick.callback.AddListener(eventData => {
            if (Input.GetMouseButtonUp(1)) {
                storeLogic.Sell(heroMono, showItemGrids[itemGrid.index]);
            }
        });

        EventTrigger eventTrigger = itemPanel.GetComponent<EventTrigger>();
        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(onMouseEnter);
        eventTrigger.triggers.Add(onMouseExit);
        eventTrigger.triggers.Add(onMouseClick);
    }

    /// <summary>
    /// 新建ItemPanel的方法，在创建的同时，为其增加绑定方法
    /// </summary>
    /// <param name="itemGrid">要进行绑定的实体类对象(Model),当实体类对象,View会自动改变,实质上是View订阅了实体类改变的事件</param>
    public void CreateItemPanel(ItemGrid itemGrid) {
        StoreItemPanelView itemPanel = GameObject.Instantiate<StoreItemPanelView>(itemPanelViewPrefab ,parent: contentRectTransform,worldPositionStays:false);
        itemGridsView.Add(itemPanel);
        itemGrid.OnIconPathChanged += OnIconPathValueChanged;
        itemGrid.OnItemCountChanged += OnItemCountChanged;
    }

    private void OnIconPathValueChanged(string oldIconPathValue,string newIconPathValue,int index) {
        itemGridsView[index].BindingContext.iconPath.Value = newIconPathValue;
    }
    private void OnItemCountChanged(int oldValue,int newValue,int index) {
        itemGridsView[index].BindingContext.itemCount.Value = newValue;
    }

    /// <summary>
    /// 绑定按钮的事件
    /// </summary>
    private void BindButtonEvent() {
        // 绑定分类按钮的Click事件
        for (int i=0;i<categoryButtons.Count();i++) {
            CommditType type = (CommditType)i;
            Button button = categoryButtons[i];
            button.onClick.AddListener(()=> {
                UpdateShowCommdities(type);
                UpdateButtonForcus(button);
            });
        }

        // 绑定展开/收起商店按钮事件
        OnOrOffButton.onClick.AddListener(()=> {
            if (canvasGroup.alpha == 1) {
                // 商店处于开启状态,点击按钮进行收缩
                Hide();
                OnOrOffButtonText.text = "<<< 展开商店";
            } else if (canvasGroup.alpha == 0) {
                // 商店处于关闭状态,点击按钮进行展开
                Reveal();
                OnOrOffButtonText.text = "收起 >>>";
            }
        });
    }

    /// <summary>
    /// 将某个按钮设为焦点
    /// </summary>
    private void UpdateButtonForcus(Button forcusButton) {
        foreach (var button in categoryButtons) {
            if (button == forcusButton) {
                (button.transform as RectTransform).sizeDelta = new Vector2(65, 40);
                button.image.color = new Color32(105,107,111,255);
            } else {
                (button.transform as RectTransform).sizeDelta = new Vector2(65, 25);
                button.image.color = new Color32(64,72,91,200);
            }
        }
    }

    /// <summary>
    /// 更新商品的购买边框
    /// </summary>
    private void UpdateCanBuyButton() {
        Debug.Log("showItemGrids.Count:"+showItemGrids.Count);
        for (int i=0;i<showItemGrids.Count;i++) {
            ItemGrid itemGrid = showItemGrids[i];
            itemGrid.CanBuy = storeLogic.IsCanBuyItem(itemGrid, heroMono);
            itemGridsView[i].BindingContext.Modify(itemGrid);
        }
    }

    private void BindCanBuyButtonEvent() {
        heroMono.Owner.OnMoneyChanged += UpdateCanBuyButton;
    }
}

