using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class MapView : MonoBehaviour {

    public RectTransform Canvas;
    private EventTrigger eventTrigger;

    private Vector2 UIposition;

    // 小地图相关信息
    public RectTransform mapRectTransform;

    // 小地图游标
    public Image CameraCursor;

    public Camera UICamera;

    // 从UI坐标转向贴图坐标的比值
    // (UI坐标和贴图坐标原点相同,要将他们互相转换,即将UI坐标的每个值乘于
    // Vector2( 贴图坐标的宽度/UI坐标的宽度 , 贴图坐标的高度/UI坐标的高度 )  即转换比例
    private Vector2 UIToTex;

    // 从贴图坐标转向世界坐标的比值
    private Vector2 TexToWorld;

    // 用于修正因为相机视角带来的摄像机距离理想位置的偏移
    public Vector2 cameraOffset = new Vector2(7f,15f);

    // 世界大小
    public float worldSize = 200;
    // 贴图大小
    public float textureSize = 256f;

    // 判断当前鼠标的位置是否在小地图上
    private bool isMouseInMapView = false;
    
    private void Start() {
        // 从UI坐标到贴图坐标的转换
        UIToTex = new Vector2(textureSize / mapRectTransform.sizeDelta.x, textureSize / mapRectTransform.sizeDelta.y);
        // 从贴图坐标到世界坐标的转换
        TexToWorld = new Vector2( worldSize / textureSize, worldSize / textureSize );

        eventTrigger = GetComponent<EventTrigger>();
        var pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener(eventdata => {
            //Debug.Log("PointerDown");
            isMouseInMapView = true;
                   
        });

        var pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener(eventdata => {
            //Debug.Log("PointerUp");
            isMouseInMapView = false;
        });

        eventTrigger.triggers.Add(pointerDownEntry);
        eventTrigger.triggers.Add(pointerUpEntry);

        // 绑定摄像机移动事件与小地图游标的位置改变
        Camera.main.GetComponent<MainCameraManager>().OnCameraMove += MoveCursor;
    }

    public void MoveCameraByInputDown() {
        // 获得UI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRectTransform, Input.mousePosition, UICamera, out UIposition);

        // 此时摄像机的坐标等于 UI坐标转换为贴图坐标,再由贴图坐标转换为世界坐标
        Vector2 cameraWorldPosition = TexToWorld * (UIToTex * UIposition);

        // 对摄像机位置进行些许偏移
        Vector3 cameraNewPosition = new Vector3(cameraWorldPosition.x - cameraOffset.x, Camera.main.transform.position.y, cameraWorldPosition.y - cameraOffset.y); 

        // 设置摄像机新的位置
        Camera.main.GetComponent<MainCameraManager>().MoveCamera(cameraNewPosition);
    }

    /// <summary>
    /// 根据摄像机当前位置来设置游标位置
    /// </summary>
    public void MoveCursor(Vector3 oldPosition,Vector3 newPosition) {

        // 取消偏移
        Vector2 cameraPosition = new Vector2(Camera.main.transform.position.x + cameraOffset.x, Camera.main.transform.position.z + cameraOffset.y);

        // 将摄像机当前位置转换为贴图位置
        Vector2 texturePosition = cameraPosition / TexToWorld;

        //Debug.Log("texPosition:"+texturePosition);

        // 贴图位置转换为UI位置
        Vector2 newUIPosition = texturePosition / UIToTex;

        //Debug.Log("newUIPosition:"+newUIPosition);

        // 设置小地图游标新的位置
        CameraCursor.rectTransform.anchoredPosition = newUIPosition;

        //Vector2 cameraPosition;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, Input.mousePosition, UICamera, out cameraPosition);

        //CameraCursor.rectTransform.anchoredPosition = UIposition + mapRectTransform.anchoredPosition;
    }

    private void Update() {
        if (isMouseInMapView) {
                Debug.Log("按下左键");

                MoveCameraByInputDown();
        }
    }

}
