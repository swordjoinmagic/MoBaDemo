using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveTitleUI : MonoBehaviour {

    public RectTransform panelTransform;

    public Button[] buttons;

    // 扰动频率
    public float disturbanceInterval = 0.5f;
    // 扰动范围
    public float disturbanceRange = 20f;

    public RectTransform titleTransform;

    // Use this for initialization
    void Start() {

        // 使按钮不可见
        foreach (var button in buttons) {
            button.transform.localScale = Vector3.zero;
        }

        panelTransform.anchoredPosition = new Vector2(1920,0);
        panelTransform.localRotation = Quaternion.Euler(0,90,0);
        panelTransform.DOAnchorPosX(0, 1.5f);
        panelTransform.DORotate(new Vector3(0,0,0), 1).OnComplete(()=> {
            AnimationInit();
            BindButtonEvent();
        });
    }

    public void AnimationInit() {
        for (int i = 0; i < buttons.Length; i++) {
            Button button = buttons[i];
            RectTransform transform = button.transform as RectTransform;

            // 使按钮可见
            transform.localScale = Vector3.one;

            float y = transform.anchoredPosition.y;
            float x = transform.anchoredPosition.x;
            transform.anchoredPosition = new Vector2(x + transform.sizeDelta.x, y + transform.sizeDelta.y);
            y = transform.anchoredPosition.y;
            x = transform.anchoredPosition.x;
            transform.DOAnchorPosY(y - 100, 0.3f).OnComplete(() => {
                transform.DOAnchorPosX(x - 700, 0.3f).OnComplete(() => {
                    Disturbance(transform);
                });
            }).SetDelay(i * 0.2f);
        }
        Disturbance(panelTransform);
    }

    void BindButtonEvent() {
        foreach (var button in buttons) {
            RectTransform bgTransform = button.transform.Find("background") as RectTransform;
            EventTrigger eventTrigger = button.GetComponent<EventTrigger>();

            // 鼠标进入事件
            EventTrigger.Entry onMouseEnter = new EventTrigger.Entry();
            onMouseEnter.eventID = EventTriggerType.PointerEnter;
            onMouseEnter.callback.AddListener((eventdata) => {
                Debug.Log(bgTransform.anchoredPosition);
                bgTransform.DOSizeDelta(new Vector2(800,bgTransform.sizeDelta.y),0.5f);
            });

            // 鼠标离开事件
            EventTrigger.Entry onMouseLeave = new EventTrigger.Entry();
            onMouseLeave.eventID = EventTriggerType.PointerExit;
            onMouseLeave.callback.AddListener((eventdata) => {
                Debug.Log(bgTransform.anchoredPosition);
                bgTransform.DOSizeDelta(new Vector2(0, bgTransform.sizeDelta.y), 0.5f);
            });

            eventTrigger.triggers.Add(onMouseEnter);
            eventTrigger.triggers.Add(onMouseLeave);
        }
    }

    void Disturbance(RectTransform rectTransform) {
        float x = rectTransform.anchoredPosition.x;
        float y = rectTransform.anchoredPosition.y;

        Vector2 origin = new Vector2(x,y);
        Vector2 randomVector2 = new Vector2(x+Random.Range(-this.disturbanceRange, this.disturbanceRange),y+ Random.Range(-this.disturbanceRange, this.disturbanceRange));

        // 扰动
        Tweener tweener = rectTransform.DOAnchorPos(randomVector2, disturbanceInterval).OnComplete(()=> {
            // 归位
            Rest(rectTransform, origin);
        });
        tweener.SetEase(Ease.InOutSine);

    }

    void Rest(RectTransform rectTransform,Vector2 origin, bool isTitle = false) {
        // 归位
        Tweener tweener = rectTransform.DOAnchorPos(origin, disturbanceInterval).OnComplete(() => {
            // 扰动
            Disturbance(rectTransform);
        });
        tweener.SetEase(Ease.InOutSine);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
