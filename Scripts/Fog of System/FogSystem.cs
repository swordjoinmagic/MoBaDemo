using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FogSystem : MonoBehaviour {

    // 单例类
    public static FogSystem instace;
    public static FogSystem Instace {
        get {
            if (instace == null)
                instace = FindObjectOfType<FogSystem>();
            return instace;
        }
    }

    // 地图大小
    public float worldSize = 128;

    // 战争迷雾贴图大小
    public int textureSize = 128;

    // 战争迷雾贴图坐标原点在世界坐标下的坐标原点
    public Vector3 originPosition = Vector3.zero;

    // 战争迷雾贴图大小的平方
    private int textureSizeSqr;

    // 战争迷雾贴图
    private Texture2D texture;
    private Color32[] buffer0;

    // 用于测试的视野单位
    public List<Transform> players = new List<Transform>();
    public List<CharacterMono> characterMonos = new List<CharacterMono>();
    public List<Vector3> playersPositions = new List<Vector3>();
    public List<IFOVUnit> fOVUnits = new List<IFOVUnit>();

    public Image image;
    
    // 战争迷雾
    public GameObject fog;

    // 线程是否开始
    public bool isthreadStart = false;
    private Thread thread;
    private Thread thread2;

    //=====================================
    // 小地图部分
    private Color32[] minMapBuffer;
    private Texture2D minMapTexture;
    public Image minMapMask;

    public enum FogBlendingThreadStatus {
        Update,
        Finished
    }

    private FogBlendingThreadStatus threadStatus = FogBlendingThreadStatus.Update;

    public void RemoveListData<T>(int index, List<T> list) {
        lock (list) {
            list.RemoveAt(index);
        }
    }
    public void AddListData<T>(T value,List<T>list) {
        lock (list) {
            list.Add(value);
        }
    }

    private void Start() {

        textureSizeSqr = textureSize * textureSize;
        playersPositions = new List<Vector3>(players.Count());
        playersPositions.AddRange(new Vector3[players.Count()]);
        // 初始化战争迷雾贴图
        texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false) {
            wrapMode = TextureWrapMode.Clamp
        };

        // 初始化buffer数组
        buffer0 = new Color32[textureSizeSqr];
        for (int i = 0; i < textureSizeSqr; i++) {
            buffer0[i] = new Color32(0, 0, 0, 255);
        }

        texture.SetPixels32(buffer0);
        texture.Apply();

        // 为战争迷雾设置贴图
        fog.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        if (image != null)
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        fOVUnits = new List<IFOVUnit>();
        for (int i = 0; i < characterMonos.Count(); i++) {
            fOVUnits.Add(characterMonos[i].characterModel);
        }

        //======================================
        // 小地图
        // 初始化小地图
        minMapBuffer = new Color32[textureSizeSqr];
        for (int i = 0; i < textureSizeSqr; i++) {
            minMapBuffer[i] = new Color32(0, 0, 0, 255);
        }
        // 设置小地图
        minMapTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false) {
            wrapMode = TextureWrapMode.Clamp
        };
        minMapTexture.SetPixels32(minMapBuffer);
        minMapTexture.Apply();
        minMapMask.material.SetTexture("_MainTex",minMapTexture);

        isthreadStart = true;
        thread = new Thread(OnUpdate);
        thread.Start();
        //thread2 = new Thread(UpdateUnitVisibleStatus);
        //thread2.Start();

    }

    private void Update() {

        for (int i = 0; i < players.Count();) {
            if (players[i] != null) {
                playersPositions[i] = players[i].position;
                i++;
            } else {
                RemoveListData<Transform>(i, players);
                RemoveListData<Vector3>(i, playersPositions);
            }
        }
        for (int i = 0; i < characterMonos.Count();) {
            if (characterMonos[i] != null) {
                fOVUnits[i].Position = characterMonos[i].transform.position;
                i++;
            } else {
                RemoveListData<CharacterMono>(i, characterMonos);
                RemoveListData<IFOVUnit>(i, fOVUnits);
            }

        }
        if (threadStatus == FogBlendingThreadStatus.Finished) {
            texture.SetPixels32(buffer0);
            texture.Apply();
            minMapTexture.SetPixels32(minMapBuffer);
            minMapTexture.Apply();
            threadStatus = FogBlendingThreadStatus.Update;
        }
    }

    private void OnDestroy() {
        isthreadStart = false;
        // 等待线程结束
        if (thread != null)
            thread.Join();
        if (thread2 != null)
            thread2.Join();
    }

    /// <summary>
    /// 更新每个单位在战争迷雾的逻辑可见状态
    /// </summary>
    private void UpdateUnitVisibleStatus() {
        for (int i = 0; i < fOVUnits.Count();) {
            if (fOVUnits[i] != null) {
                if (IsUnitVisible(fOVUnits[i])) {
                    fOVUnits[i].IsVisible = true;
                } else {
                    fOVUnits[i].IsVisible = false;
                }
                i++;
            } else {
                RemoveListData<IFOVUnit>(i, fOVUnits);
            }
        }
    }

    /// <summary>
    /// 判断一个单位是否处于可见范围
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    private bool IsUnitVisible(IFOVUnit unit) {
        // 获得当前单位位置
        Vector3 position = unit.Position;

        // 将单位坐标转换到贴图坐标中
        position *= textureSize / worldSize;

        // 判断当前贴图坐标的r通道是否大于等于255,如果大于，那么当前单位是可见
        int z = Mathf.Clamp(Mathf.FloorToInt(position.z), 0, textureSize);
        int x = Mathf.Clamp(Mathf.FloorToInt(position.x), 0, textureSize);
        if (buffer0[z * textureSize + x].r >= 255) {
            return true;
        } else {
            return false;
        }

    }

    private void OnUpdate() {
        Thread.Sleep(200);
        while (isthreadStart) {
            if (threadStatus == FogBlendingThreadStatus.Update) {
                ClearVisibledRegion();
                RevalMap();
                GeneratePassedRegion();
                UpdateUnitVisibleStatus();

                // 更新人物在小地图上的点
                ClearMinMap();
                DrawMinMap();

                threadStatus = FogBlendingThreadStatus.Finished;
            }
            Thread.Sleep(100);
        }
    }

    public void RevalMap() {
        for (int i = 0; i < playersPositions.Count(); i++) {
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
        float radius = 10 * WorldToTex;

        // 探查视野的范围
        int minX = Mathf.Clamp(Mathf.FloorToInt(position.x - radius), 0, textureSize - 1);
        int minZ = Mathf.Clamp(Mathf.FloorToInt(position.z - radius), 0, textureSize - 1);
        int maxX = Mathf.Clamp(Mathf.FloorToInt(position.x + radius), 0, textureSize - 1);
        int maxZ = Mathf.Clamp(Mathf.FloorToInt(position.z + radius), 0, textureSize - 1);

        for (int z = minZ; z <= maxZ; z++) {
            int zw = z * textureSize;
            for (int x = minX; x <= maxX; x++) {

                int cx = x - Mathf.FloorToInt(position.x);
                int cz = z - Mathf.FloorToInt(position.z);
                // 判断(x,z)是否在目标视野中
                if (cx * cx + cz * cz <= radius * radius) {
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
        for (int i = 0; i < textureSizeSqr; i++) {
            buffer0[i].r = 0;
        }
    }

    /// <summary>
    /// 生成已通过的区域,也就是将r通道的值赋给g通道
    /// </summary>
    public void GeneratePassedRegion() {
        for (int i = 0; i < textureSizeSqr; i++) {
            if (buffer0[i].g < buffer0[i].r)
                buffer0[i].g = buffer0[i].r;
        }
    }


    /// <summary>
    /// 清除小地图上的点
    /// </summary>
    public void ClearMinMap() {
        for (int i=0;i<textureSizeSqr;i++) {
            minMapBuffer[i].g = 0;
        }
    }

    // 绘制小地图上单位的点
    public void DrawMinMap() {
        for (int i = 0; i < playersPositions.Count(); i++) {
            lock (playersPositions) {
                Vector3 position = playersPositions[i];

                // 世界坐标转贴图坐标
                position *= textureSize / worldSize;

                // 在当前贴图坐标下绘制一个小的绿色矩形
                int minX = Mathf.Clamp(Mathf.FloorToInt(position.x - 3),0,textureSize) ;
                int minZ = Mathf.Clamp(Mathf.FloorToInt(position.z - 3),0,textureSize) ;
                int maxX = Mathf.Clamp(Mathf.FloorToInt(position.x + 3),0,textureSize) ;
                int maxZ = Mathf.Clamp(Mathf.FloorToInt(position.z + 3),0,textureSize) ;

                for (int z=minZ;z<=maxZ;z++) {
                    int zw = z * textureSize;
                    for (int x=minX;x<=maxX;x++) {
                        minMapBuffer[zw + x].g = 255;
                    }
                }
            }
        }
    }
}


