using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageItemView : MonoBehaviour {

    //=============================
    // 管理的UI控件
    public Image image;
    public Text text;

    private string imagePath;
    private string textStr;

    public string ImagePath {
        get {
            return imagePath;
        }

        set {
            imagePath = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
