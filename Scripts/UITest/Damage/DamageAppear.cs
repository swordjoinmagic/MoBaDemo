using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 用来实现伤害数字往上飘的动画,
/// 用到的UI层是Screen Only UI
/// </summary>
class DamageAppear : MonoBehaviour{

    public Transform Canvas;
    public Text text;
    // 目标位置
    public Transform targetTransfrom;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetTransfrom.position);
            print(screenPoint);

            Text text = Instantiate<Text>(this.text, Canvas);

            text.text = "-123";
            text.rectTransform.position = screenPoint;

            RectTransform rectTransform = text.rectTransform;

            Color color = text.color;
            color.a = 1;
            text.color = color;

            rectTransform.DOMoveY(rectTransform.position.y + 50, 1f);
            text.DOFade(0, 0.3f).SetDelay(0.2f).onComplete = ()=>{
                Destroy(text.gameObject);
            };
        }

    }
}

