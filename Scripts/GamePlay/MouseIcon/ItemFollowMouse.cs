using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 当单位拾取物品时,开启此脚本,目标物品的图标跟随鼠标
/// </summary>
[RequireComponent(typeof(Image))]
public class ItemFollowMouse : MonoBehaviour{

    // 单例
    private static ItemFollowMouse instance;
    public static ItemFollowMouse Instace {
        get {
            if (null == instance) {
                instance = FindObjectOfType<ItemFollowMouse>();
            }
            return instance;
        }
    }

    // 要跟随鼠标的图像的位置
    private RectTransform imageTransform;

    // 要跟随鼠标的图像
    private Image image;

    // 要跟随鼠标的图像
    private Texture2D texture;

    // 用于UI显示的Canvas
    private RectTransform canvas;

    // UI摄像机
    private Camera UICamera;

    public Texture2D Texture {
        get {
            return texture;
        }

        set {
            texture = value;
            if (texture != null)
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            else
                image.sprite = null;
        }
    }

    public void StartPickUpItem(Texture2D texture) {
        if (image == null || imageTransform==null || canvas == null) Init();
        this.gameObject.SetActive(true);
        Texture = texture;
    }

    public void StartPickUpItem(string imagePath) {
        Texture2D texture = Resources.Load<Texture2D>("UIImage/" + imagePath);
        StartPickUpItem(texture);
    }

    public void CancelPickUpItem() {
        Texture = null;
        this.gameObject.SetActive(false);
    }

    private void Init() {
        imageTransform = transform.GetComponent<RectTransform>();
        image = transform.Find("ItemImage").GetComponent<Image>();
        // 整个游戏只用一个Canvas,故可以直接find来找
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    private void Update() {

        if (canvas == null || UICamera == null || imageTransform == null) return;

        Vector2 mousePosition = Input.mousePosition;

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mousePosition, UICamera, out localPoint);
        localPoint.x += imageTransform.sizeDelta.x / 2 + imageTransform.sizeDelta.x / 5;
        localPoint.y -= imageTransform.sizeDelta.y / 5;

        imageTransform.anchoredPosition = localPoint;
    }
}

