using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShowUIPosition : MonoBehaviour{

    public RectTransform rectTransform;
    private RectTransform canvas;
    private Camera UICamera;


    private void Start() {

        canvas = GameObject.Find("Canvas").transform as RectTransform;
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        Debug.Log("Canvas:"+canvas.anchoredPosition);
        Debug.Log("anchoredPosition:"+rectTransform.anchoredPosition);
        Debug.Log("localPosition:" + rectTransform.localPosition);
        Debug.Log("position:" + rectTransform.position);
        Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(UICamera,rectTransform.position);
        Debug.Log("ScreenPosition:"+vector2);
        Vector2 locationPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas,vector2,UICamera,out locationPosition);
        Debug.Log("locationPosition:"+ locationPosition);
    }
}

