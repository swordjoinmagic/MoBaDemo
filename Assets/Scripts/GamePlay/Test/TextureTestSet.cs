using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class TextureTestSet : MonoBehaviour{

    private Texture2D texture;
    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void Start() {
        Debug.Log("Start");
        texture = new Texture2D(1024,1024,TextureFormat.ARGB32,false);
        texture.wrapMode = TextureWrapMode.Clamp;
        image.sprite = Sprite.Create(texture, new Rect(0, 0, 1024, 1024), new Vector2(0, 0));


        Color32[] color32s = new Color32[1024*1024];

        for (int i = 0; i < 512 * 500; i++) {
            color32s[i] = new Color32(255, 0, 0, 255);
        }

        texture.SetPixels32(color32s);
        texture.Apply();

    }
}

