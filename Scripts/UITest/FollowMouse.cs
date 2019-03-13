using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Image跟随鼠标测试
/// </summary>
public class FollowMouse : MonoBehaviour{
    public RectTransform image;
    public RectTransform canvas;
    public Camera UICamera;

    private void Update() {

        Vector2 mousePosition = Input.mousePosition;

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mousePosition,UICamera,out localPoint);

        localPoint.x += image.sizeDelta.x/2 + image.sizeDelta.x/5;
        image.anchoredPosition = localPoint;
    }
}

