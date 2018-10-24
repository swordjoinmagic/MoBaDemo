using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FogSystem : MonoBehaviour{

    // 地图大小
    public float worldSize = 128;

    // 战争迷雾贴图大小
    public int textureSize = 128;

    // 战争迷雾在世界坐标下的坐标原点
    public Vector3 originPosition = Vector3.zero;
    
    // 战争迷雾贴图大小的平方
    private int textureSizeSqr;

    public Texture2D texture;
    private Color32[] buffer0;

    // 用于测试的视野单位
    public Transform player;

    private void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        textureSizeSqr = textureSize * textureSize;

        // 初始化战争迷雾贴图
        texture = new Texture2D(textureSize,textureSize,TextureFormat.ARGB32,false);

        // 初始化buffer数组
        buffer0 = new Color32[textureSizeSqr];
    }

    /// <summary>
    /// 使用视野单位更新可见区域
    /// </summary>
    public void RevealUsingVision() {

        // 将视野单位的世界坐标转换为贴图坐标
        float worldToTex = textureSize / worldSize;

        Vector3 texPosition = (player.position - originPosition) * worldToTex;
        float radius = 5 * worldToTex;

        // 判断单位的圆形视野
        int minX = Mathf.FloorToInt(texPosition.x - radius);
        int minY = Mathf.FloorToInt(texPosition.z - radius);
        int maxX = Mathf.FloorToInt(texPosition.x + radius);
        int maxY = Mathf.FloorToInt(texPosition.z + radius);

        // 从左下到右上遍历
        for (int x=minX;x<maxX;x++) {
            int xw = x * textureSize;
            for (int y=minY;y<maxY;y++) {



                // 判断当前点是否在角色的视野内
                if (x*x + y*y < radius*radius) {

                }
            }
        }
    }
}

