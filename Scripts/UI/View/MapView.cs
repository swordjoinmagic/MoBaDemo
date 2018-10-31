using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class MapView : MonoBehaviour {

    private EventTrigger eventTrigger;
    private Vector2 position;
    public RectTransform rectTransform;
    public Camera UICamera;
    public RenderTexture renderTexture;

    private Vector2 UIToTex;
    private Vector2 TexToWorld;
    // 用于修正因为相机视角带来的摄像机距离理想位置的偏移
    public Vector2 cameraOffset = new Vector2(7f,15f);

    private void Start() {
        // 从UI坐标到贴图坐标的转换
        UIToTex = new Vector2(256 / rectTransform.sizeDelta.x, 256 / rectTransform.sizeDelta.y);
        // 从贴图坐标到世界坐标的转换
        TexToWorld = new Vector2((300f) / 256f, (300f )/ 256f);

        eventTrigger = GetComponent<EventTrigger>();
        var clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener(eventdata => {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,Input.mousePosition,UICamera,out position);
            Vector2 cameraWorldPosition = TexToWorld * (UIToTex * position);
            Camera.main.transform.position = new Vector3(cameraWorldPosition.x-cameraOffset.x,Camera.main.transform.position.y,cameraWorldPosition.y-cameraOffset.y);
            Debug.Log("cameraWorldPosition:"+cameraWorldPosition);
        });
        eventTrigger.triggers.Add(clickEntry);

    }
}
