using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class FogSystem : MonoBehaviour{

    // 地图大小
    public float worldSize = 128;

    // 战争迷雾贴图大小
    public int textureSize = 128;

    // 战争迷雾贴图坐标原点在世界坐标下的坐标原点
    public Vector3 originPosition = Vector3.zero;
    
    // 战争迷雾贴图大小的平方
    private int textureSizeSqr;

    private Texture2D texture;
    private Color32[] buffer0;

    // 用于测试的视野单位
    public Transform[] players;
    private Vector3[] playersPositions;

    // 战争迷雾
    public GameObject fog;

    // 线程是否开始
    public bool isthreadStart = false;
    private Thread thread;

    public enum FogBlendingThreadStatus {
        Update,
        Finished
    }

    private FogBlendingThreadStatus threadStatus = FogBlendingThreadStatus.Update;

    private void Start() {

        textureSizeSqr = textureSize * textureSize;
        playersPositions = new Vector3[players.Count()];
        // 初始化战争迷雾贴图
        texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false) {
            wrapMode = TextureWrapMode.Clamp
        };

        // 初始化buffer数组
        buffer0 = new Color32[textureSizeSqr];
        for (int i=0;i<textureSizeSqr;i++) {
            buffer0[i] = new Color32(0,0,0,255);
        }

        texture.SetPixels32(buffer0);
        texture.Apply();

        // 为战争迷雾设置贴图
        fog.GetComponent<MeshRenderer>().material.SetTexture("_MainTex",texture);

        isthreadStart = true;
        thread = new Thread(new ThreadStart(OnUpdate));
        thread.Start();
        
    }

    private void Update() {
        for(int i=0;i<players.Count();i++) {
            playersPositions[i] = players[i].position; 
        }
        if (threadStatus == FogBlendingThreadStatus.Finished) {
            texture.SetPixels32(buffer0);
            texture.Apply();
            threadStatus = FogBlendingThreadStatus.Update;
        }
    }

    private void OnDestroy() {
        isthreadStart = false;
        // 等待线程结束
        
    }

    private void OnUpdate() {
        while (isthreadStart) {
            if (threadStatus == FogBlendingThreadStatus.Update) {
                ClearVisibledRegion();
                RevalMap();
                GeneratePassedRegion();
                threadStatus = FogBlendingThreadStatus.Finished;
            }
        }
    }

    public void RevalMap() {
        for (int i=0;i<playersPositions.Count();i++) {
            lock (playersPositions) {
                Vector3 vector3 = playersPositions[i];
                RevealUsingVision(vector3);
            }
            //Vector3 position = transform.position;
            
        }
    }

    /// <summary>
    /// 使用视野单位更新可见区域
    /// </summary>
    /// <param name="position">视野单位的坐标</param>
    public void RevealUsingVision(Vector3 position) {

        //====================================
        // 第一步
        // 将视野单位的世界坐标转为贴图坐标
        float WorldToTex = textureSize / worldSize;
        position *= WorldToTex;

        //===================================
        // 第二步,根据视野单位的视野,设置可见范围,y轴表现在贴图坐标上是从下到上的
        float radius = 5*WorldToTex;

        // 探查视野的范围
        int minX = Mathf.Clamp(Mathf.FloorToInt(position.x - radius), 0, textureSize-1);
        int minZ = Mathf.Clamp(Mathf.FloorToInt(position.z - radius),0,textureSize-1);
        int maxX = Mathf.Clamp(Mathf.FloorToInt(position.x + radius),0,textureSize-1);
        int maxZ = Mathf.Clamp(Mathf.FloorToInt(position.z + radius),0,textureSize-1); 

        for (int z = minZ;z<=maxZ;z++) {
            int zw = z * textureSize;
            for (int x = minX;x<=maxX;x++) {

                int cx = x - Mathf.FloorToInt(position.x);
                int cz = z - Mathf.FloorToInt(position.z);
                // 判断(x,z)是否在目标视野中
                if (cx*cx+cz*cz <= radius*radius) {
                    // 设置当前点可见
                    buffer0[x + zw].r = 255;
                }
            }
        }
    }

    /// <summary>
    /// 清除可见区域,也就是将颜色的r通道设为0
    /// </summary>
    public void ClearVisibledRegion() {
        for (int i=0;i<textureSizeSqr;i++) {
            buffer0[i].r = 0;
        }
    }

    /// <summary>
    /// 生成已通过的区域,也就是将r通道的值赋给g通道
    /// </summary>
    public void GeneratePassedRegion() {
        for (int i = 0; i < textureSizeSqr; i++) {
            if(buffer0[i].g < buffer0[i].r)
                buffer0[i].g = buffer0[i].r;
        }
    }

}

