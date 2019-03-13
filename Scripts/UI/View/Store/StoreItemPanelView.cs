using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 用于显示在商店的物品视图
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class StoreItemPanelView : MonoBehaviour{
    //===============================
    // 此视图管理的UI元素
    public Image iconImage;
    public Text remainCountText;
    public Outline outline;
    public ItemTipsView ItemTipsViewPrefab;
    private ItemTipsView itemTipsView;
    private Canvas canvas;
    private StoreView storeView;


    // 冷却的图形
    public Image coolDownImage;

    // 此View所依附的ItemGrid
    public ItemGrid itemGrid;

    private void Start() {
        canvas = GameObject.FindObjectOfType<Canvas>();
        storeView = GameObject.FindObjectOfType<StoreView>();
    }

    public void Init(ItemGrid itemGrid) {

        if (this.itemGrid != null) {
            //===============================
            // 解除绑定
            this.itemGrid.OnIconPathChanged -= OnIconImageChanged;
            this.itemGrid.OnItemCountChanged -= OnRemainCountChanged;
            this.itemGrid.OnCanBuyChanged -= OnOutlineColorChanged;
        }

        // 设置新的ItemGrid,并触发ItemGrid更替事件
        OnIconImageChanged("",itemGrid.ItemImagePath);
        OnRemainCountChanged(0,itemGrid.ItemCount);
        OnOutlineColorChanged(false,itemGrid.CanBuy);

        this.itemGrid = itemGrid;

        //==========================================
        // 监听CharacterMono的物品改变事件
        this.itemGrid.OnIconPathChanged += OnIconImageChanged;
        this.itemGrid.OnItemCountChanged += OnRemainCountChanged;
        this.itemGrid.OnCanBuyChanged += OnOutlineColorChanged;
        SetItemPanelMouseEvent();
    }

    /// <summary>
    /// 当物品的图片地址发生改变
    /// </summary>
    public void OnIconImageChanged(string oldIconImagePath,string newIconImagePath) {
        if (newIconImagePath != null) {
            iconImage.sprite = Resources.Load("UIImage/" + newIconImagePath, typeof(Sprite)) as Sprite;
            iconImage.color = new Color(1, 1, 1, 1);
        } else
            iconImage.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// 当物品剩余数量发生改变时
    /// </summary>
    public void OnRemainCountChanged(int oldItemCount,int newItemCount) {
        Debug.Log("新的物品数量是:"+newItemCount);
        if (newItemCount != 0)
            remainCountText.text = newItemCount.ToString();
        else
            remainCountText.text = "";
        if(itemGrid!=null && itemGrid.IsCoolDowning)
            remainCountText.text = "已售空";
    }

    public void OnOutlineColorChanged(bool oldValue,bool newValue) {
        if (newValue) {
            outline.effectColor = Color.green;
        } else {
            outline.effectColor = Color.red;
        }
    }

    private void Update() {
        if (itemGrid!=null && itemGrid.item != null && itemGrid.IsCoolDowning) {
            // 更新冷却条
            if (itemGrid.item.itemPayInteral != 0 && itemGrid.TimeProgressRate!=0) {
                float rate = Mathf.Clamp01(itemGrid.TimeProgressRate / itemGrid.item.itemPayInteral);
                coolDownImage.fillAmount = 1 - rate;
            }
        }
    }

    public void Reveal() {
        GetComponent<CanvasGroup>().DOFade(1,0.5f);
    }

    public void Hide() {
        GetComponent<CanvasGroup>().DOFade(0, 0.5f);
    }

    /// <summary>
    /// 给单位的ItemPanel视图设置鼠标停留、离开事件（用于显示提示视图）、还有购买物品
    /// </summary>
    private void SetItemPanelMouseEvent() {
        EventTrigger.Entry onMouseEnter = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerEnter
        };
        onMouseEnter.callback.AddListener(eventData => {
            if (itemGrid.item == null) return;

            if (itemTipsView == null) {
                itemTipsView = GameObject.Instantiate<ItemTipsView>(ItemTipsViewPrefab, canvas.transform);
                itemTipsView.BindingContext = new ItemViewModel();
            }

            // 设置提示窗口出现位置
            itemTipsView.transform.SetParent(transform);
            (itemTipsView.transform as RectTransform).anchoredPosition = new Vector2((transform as RectTransform).sizeDelta.x / 2, (transform as RectTransform).sizeDelta.y / 2);
            itemTipsView.transform.SetParent(canvas.transform);

            itemTipsView.BindingContext.Modify(itemGrid);
            itemTipsView.Reveal();
        });
        EventTrigger.Entry onMouseExit = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerExit
        };
        onMouseExit.callback.AddListener(eventData => {
            if (itemGrid.item == null) return;
            itemTipsView.Hide(immediate: true);
        });

        // 右键单击购买物品的事件
        EventTrigger.Entry onMouseClick = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerClick
        };
        onMouseClick.callback.AddListener(eventData => {
            // 当物品不处于冷却状态时,才能购买此物品
            if (Input.GetMouseButtonUp(1) && !itemGrid.IsCoolDowning) {
                itemTipsView.Hide();
                storeView.Sell(itemGrid);
            }
        });

        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(onMouseEnter);
        eventTrigger.triggers.Add(onMouseExit);
        eventTrigger.triggers.Add(onMouseClick);
    }
}

