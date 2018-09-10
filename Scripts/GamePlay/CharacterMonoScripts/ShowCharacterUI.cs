using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCharacterUI : MonoBehaviour {

    // 简易人物视图，用于显示每个人头顶上的血条及名字
    // 一个预制体
    public SimpleCharacterView simpleCharacterView;
    public Camera UICamera;
    public RectTransform Canvas;

    private CharacterMono character;

    private SimpleCharacterView UI;

    private bool isVisible = false;

    private void Start() {
        print("GameObjectName:"+gameObject.name);
        character = GetComponent<CharacterMono>();
    }

    // Update is called once per frame
    void Update () {
        if (UI!=null && isVisible) {
            UI.transform.localPosition = WorldPointToUIPosition(transform.position);
        }
    }

    private float GetObjectYSize() {
        print(gameObject.name+" : "+ GetComponent<MeshFilter>().mesh.bounds.size.y);
        return GetComponent<MeshFilter>().mesh.bounds.size.y;
    }

    private Vector2 WorldPointToUIPosition(Vector3 worldPoint) {
        Vector3 topWorldPosition = new Vector3(worldPoint.x,worldPoint.y+GetObjectYSize(),worldPoint.z);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(topWorldPosition);
        //print(string.Format("根据该monmo的屏幕坐标{0}绘制ui坐标", screenPosition));
        Vector2 UIPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas,screenPosition,UICamera,out UIPosition);
        //print("绘制出的UI坐标是:"+UIPosition);

        // 头顶UI
        return UIPosition;
    }

    /// <summary>
    /// 当物体在摄像机视野内时，显示UI
    /// </summary>
    private void OnBecameVisible() {
        print(gameObject.name + "OnBecameVisible");
        isVisible = true;
        if (UI==null) {
            UI = Instantiate<SimpleCharacterView>(simpleCharacterView,Canvas);
            UI.transform.localPosition = WorldPointToUIPosition(transform.position);
            UI.BindingContext = new SimpleCharacterViewModel();
            UI.BindingContext.Modify(character.characterModel);
            character.SimpleCharacterViewModel = UI.BindingContext;
        }
        UI.Reveal(true);
    }

    /// <summary>
    /// 当物体消失在摄像机视野内时,UI消失
    /// </summary>
    private void OnBecameInvisible() {
        isVisible = false;
        //print("OnBecameINVisible");
        if (UI != null) {
            UI.Hide(true);
        }
    }
}
