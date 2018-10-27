using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于绘制战争迷雾贴图的Mono类
/// </summary>
public class CrateFOGTexture : MonoBehaviour{
    public Texture2D texture;

    public float worldSize = 25;

    public int textureSize = 128;

    // 地图
    public Transform Plan;
    // 战争迷雾
    public Transform Fog;

    public Image image;

    // 纹理原点在世界空间下的坐标
    public Vector3 origin;

    private Color32[] color32s;
    private Color32[] color32s1;

    private void Start() {
        float xSize = Plan.GetComponent<MeshRenderer>().bounds.size.x;
        float ZSize = Plan.GetComponent<MeshRenderer>().bounds.size.z;
        Plan.localScale = new Vector3(worldSize/xSize,1.0f,worldSize/ZSize);

        float xSizeFog = Fog.GetComponent<MeshRenderer>().bounds.size.x;
        float ZSizeFog = Fog.GetComponent<MeshRenderer>().bounds.size.z;
        Fog.localScale = new Vector3(worldSize / xSizeFog, 1.0f, worldSize / ZSizeFog);

        texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false) {
            wrapMode = TextureWrapMode.Clamp
        };
        origin = new Vector3(-worldSize * 0.5f, 0, -worldSize * 0.5f);
        color32s = new Color32[textureSize*textureSize];
        color32s1 = new Color32[textureSize*textureSize];

        for (int i=0;i<color32s.Length;i++) {
            color32s[i] = new Color32(0,0,0,255);
            color32s1[i] = new Color32(0,0,0,255);
        }
        texture.SetPixels32(color32s);
        texture.Apply();
        Fog.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);

        //image.sprite = Sprite.Create(texture,new Rect(0,0,textureSize,textureSize),new Vector2(0,0));
    }

    public void Generate() {
        // Position relative to the fog of war
        // 将角色的坐标从世界坐标转为纹理空间内的坐标?
        // getPosition获得该视野单位在地图上的坐标

        /*
         * 将getPosition - mOrigin 表示将单位当前位置减去战争迷雾的逻辑原点 
         * 如果纹理大小和地图大小一样,那么该值就表示纹理坐标
         * 但实际中因为纹理和地图大小不一样,这里运用它们之间的倍乘关系,
         * 也就是 TextureSize / WorldSize,来获得纹理坐标
         */
        float worldToTex = (float)textureSize / worldSize;
        Vector3 pos = (transform.position - origin) * worldToTex;
        Debug.Log("TexPos:" + pos);
        float radius = 5 * worldToTex;

        // Coordinates we'll be dealing with

        // xmin,ymin,xmax,ymax共同组成了一个矩形
        // 以(xmin,ymin)为原点,xmax-xmin为宽,ymax-ymin为高
        // Rect(x = xmin,y = ymin,width = xmax-xmin,height = ymax-ymin)
        // 也就是从角色的左下角到右上角的一个矩形,相当于角色视野(圆形)的补全矩形
        // 用来判断某个小方格是否在角色的视野内
        int xmin = Mathf.RoundToInt(pos.x - radius);
        int ymin = Mathf.RoundToInt(pos.z - radius);
        int xmax = Mathf.RoundToInt(pos.x + radius);
        int ymax = Mathf.RoundToInt(pos.z + radius);

        //==========================================
        // 处理角色坐标,转换为整数
        int cx = Mathf.RoundToInt(pos.x);
        int cy = Mathf.RoundToInt(pos.z);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);

        // 用于线性规划方程,x^2 + y^2 = r^2
        // 换言之,如果x1^2 + y^2 < r^2,那么点(x1,y1)在角色的视野圆形内
        int radiusSqr = Mathf.RoundToInt(radius * radius);

        for (int y = ymin; y < ymax; ++y) {
            if (y > -1 && y < textureSize) {
                int yw = y * textureSize;

                for (int x = xmin; x < xmax; ++x) {
                    if (x > -1 && x < textureSize) {
                        int xd = x - cx;
                        int yd = y - cy;
                        int dist = xd * xd + yd * yd;

                        // Reveal this pixel
                        if (dist < radiusSqr) {
                            color32s[x + yw].r = 255;
                        }
                    }
                }
            }
        }
    }

    public void CanSeeGenerate() {
        for (int i=0;i<textureSize*textureSize;i++) {
            if (color32s[i].g < color32s[i].r) {
                color32s[i].g = color32s[i].r;
            }
        }
    }

    public void ClearSeeThroungh() {
        for (int i = 0; i < textureSize * textureSize; i++) {
            color32s[i].r = 0;
        }
    }
    void RevealMap() {
        for (int index = 0; index < textureSize*textureSize; ++index) {
            if (color32s[index].g < color32s[index].r) {
                color32s[index].g = color32s[index].r;
            }
        }
    }

    void MergeBuffer() {
        for (int index = 0; index < textureSize*textureSize; ++index) {
            color32s[index].b = color32s[index].r;
            color32s[index].a = color32s[index].g;
        }
    }

    private void Update() {
        ClearSeeThroungh();
        Generate();
        RevealMap();
        texture.SetPixels32(color32s);
        texture.Apply();
        Fog.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        MergeBuffer();
    }
}

