using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test1 : MonoBehaviour {

    public Camera UIcamera;
    public RectTransform rectTransform;
    public Text text;

    public Transform target;

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0)) {
            //Vector2 position;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, UIcamera, out position);
            //print("UIPosiiton:" + UIcamera.WorldToScreenPoint(text.transform.position));
            //Vector3 vector3;
            //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,Input.mousePosition,UIcamera,out vector3)) {
            //    text.transform.position = vector3;
            //}
            //Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
            //Vector3 vector3;
            //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPosition, UIcamera, out vector3)) {
            //    text.transform.position = vector3;
            //}
            print("ScreenPosition:"+Input.mousePosition);
            print(UIcamera.WorldToScreenPoint(text.rectTransform.position));

        }

    }
}
