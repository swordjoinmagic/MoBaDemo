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

    #region 待重构~ TODO~
    //// 用于测试的视野单位
    //public List<Transform> players = new List<Transform>();
    //public List<CharacterMono> characterMonos = new List<CharacterMono>();

    //// 用于更新战争迷雾的显示层
    //public List<Vector3> playersPositions = new List<Vector3>();

    //// 用于更新战争迷雾的逻辑层，这个数组
    //public List<IFOVUnit> fOVUnits = new List<IFOVUnit>();
    #endregion

    /// <summary>
    /// 逻辑层物体
    /// </summary>
    private List<CharacterMono> LogicalLayerObjects = new List<CharacterMono>();

    /// <summary>
    /// 表示层物体
    /// </summary>
    private List<IFOVUnit> PresentationLayerObjects = new List<IFOVUnit>();

    // 战争迷雾
    public GameObject fog;

    // 线程是否开始
    public bool isthreadStart = false;
    private Thread thread;

    #region 小地图属性
    //=====================================
    // 小地图部分
    private Color32[] minMapBuffer;
    private Texture2D minMapTexture;
    public Image minMapCharacterMask;
    public Image minMap;
    #endregion

    public enum FogBlendingThreadStatus {
        Update,
        Finished
    }

    private FogBlendingThreadStatus threadStatus = FogBlendingThreadStatus.Update;

    #region 待重构
    //public void RemoveListData<T>(int index, List<T> list) {
    //    lock (list) {
    //        list.RemoveAt(index);
    //    }
    //}
    //public void AddListData<T>(T value, List<T> list) {
    //    lock (list) {
    //        list.Add(value);
    //    }
    //}
    #endregion

    #region 视野体的增加和移除
    /// <summary>
    /// 移除视野体
    /// </summary>
    /// <param name="i"></param>
    public void RemoveFOVUnit(int i) {
        lock(LogicalLayerObjects){
            IFOVUnit fOVUnit = LogicalLayerObjects[i].characterModel;
            // 删除逻辑层物体
            LogicalLayerObjects.RemoveAt(i);
            // 同步删除表示层物体
            lock (PresentationLayerObjects) {
                PresentationLayerObjects.Remove(fOVUnit);
            }
        }
    }

    /// <summary>
    /// 增加视野体
    /// </summary>
    /// <param name="characterMono"></param>
    public void AddFOVUnit(CharacterMono characterMono) {
        lock (LogicalLayerObjects) {
            LogicalLayerObjects.Add(characterMono);
            lock(PresentationLayerObjects){
                PresentationLayerObjects.Add(characterMono.characterModel);
            }
        }
    }
    #endregion

    #region 测试
    public List<CharacterMono> TestCharacterMonos = new List<CharacterMono>();
    #endregion

    private void Start() {

        #region 根据测试数组，将物体加入战争迷雾
        foreach (var character in TestCharacterMonos) {
            AddFOVUnit(character);
        }
        #endregion

        //Debug.Log("添加完成 逻辑层物体有："+LogicalLayerObjects.Count()+" 表示层物体有："+PresentationLayerObjects.Count());
        

        textureSizeSqr = textureSize * textureSize;

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
        minMapCharacterMask.material.SetTexture("_MainTex",minMapTexture);
        minMap.material.SetTexture("_MainTex",texture);

        isthreadStart = true;
        thread = new Thread(OnUpdate);
        thread.Start();

        //Debug.Log("战争迷雾初始化完成");
    }

    private void Update() {

        #region 待重构~TODO~
        ///* 
        // * 更新每个逻辑层物体的坐标
        // * 同时，每个单位在更新时都可能会被Remove，
        // * 所以为了保证安全性，多了一个对null的判断
        //*/
        //for (int i = 0; i < players.Count();) {
        //    if (players[i] != null) {
        //        playersPositions[i] = players[i].position;
        //        i++;
        //    } else {
        //        RemoveListData<Transform>(i, players);
        //        RemoveListData<Vector3>(i, playersPositions);
        //    }
        //}
        //for (int i = 0; i < characterMonos.Count();) {
        //    if (characterMonos[i] != null) {
        //        fOVUnits[i].Position = characterMonos[i].transform.position;
        //        i++;
        //    } else {
        //        RemoveListData<CharacterMono>(i, characterMonos);
        //        RemoveListData<IFOVUnit>(i, fOVUnits);
        //    }

        //}
        #endregion

        #region 重构完成
        /* 
             * 更新每个逻辑层物体的坐标
             * 同时，每个单位在更新时都可能会被Remove，
             * 所以为了保证安全性，多了一个对null的判断
         */
        for (int i = 0; i < LogicalLayerObjects.Count();) {
            if (LogicalLayerObjects[i] != null) {
                LogicalLayerObjects[i].characterModel.Position = LogicalLayerObjects[i].transform.position;
                i++;
            } else {
                RemoveFOVUnit(i);
            }
        }
        #endregion

        /*
         * 如果线程已经画好了战争迷雾当前帧的贴图，
         * 那么更新贴图，并通知线程继续更新
         */ 
        if (threadStatus == FogBlendingThreadStatus.Finished) {
            texture.SetPixels32(buffer0);
            texture.Apply();
            minMapTexture.SetPixels32(minMapBuffer);
            minMapTexture.Apply();
            threadStatus = FogBlendingThreadStatus.Update;
        }
    }

    /// <summary>
    /// 负责在游戏结束时，销毁线程
    /// </summary>
    private void OnDestroy() {
        isthreadStart = false;
        // 等待线程结束
        if (thread != null)
            thread.Join();
    }

    /// <summary>
    /// 更新每个逻辑层单位在战争迷雾的逻辑可见状态，即战争迷雾的逻辑层
    /// </summary>
    private void UpdateUnitVisibleStatus() {
        #region 待重构
        //for (int i = 0; i < fOVUnits.Count();) {
        //    if (fOVUnits[i] != null) {
        //        if (IsUnitVisible(fOVUnits[i])) {
        //            fOVUnits[i].IsVisible = true;
        //        } else {
        //            fOVUnits[i].IsVisible = false;
        //        }
        //        i++;
        //    } else {
        //        RemoveListData<IFOVUnit>(i, fOVUnits);
        //    }
        //}
        #endregion

        //Debug.Log("更新逻辑层中，逻辑层Count:"+LogicalLayerObjects.Count());
        for (int i = 0; i < LogicalLayerObjects.Count();) {
            //Debug.Log("更新第"+i+"个单位：");
            if (LogicalLayerObjects[i] != null) {
                //Debug.Log("判断第"+i+"个单位是否可见");
                if (IsUnitVisible(LogicalLayerObjects[i].characterModel)) {
                    LogicalLayerObjects[i].characterModel.IsVisible = true;
                } else {
                    LogicalLayerObjects[i].characterModel.IsVisible = false;
                }
                //Debug.Log("第"+i+"个单位判断完成");
                i++;
            } else {
                RemoveFOVUnit(i);
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
        Thread.Sleep(1000);
        while (isthreadStart) {
            if (threadStatus == FogBlendingThreadStatus.Update) {
                //Debug.Log("正在更新战争迷雾");

                // 战争迷雾表示层
                ClearVisibledRegion();
                RevalMap();
                GeneratePassedRegion();
                //Debug.Log("更新表示层完成");

                // 战争迷雾逻辑层
                UpdateUnitVisibleStatus();
                //Debug.Log("更新逻辑层");

                // 更新人物在小地图上的点
                ClearMinMap();
                DrawMinMap();
                //Debug.Log("更新小地图");

                threadStatus = FogBlendingThreadStatus.Finished;
            }
            Thread.Sleep(100);
            //Debug.Log("战争迷雾休眠中");
        }
    }

    /// <summary>
    /// 每一个表示层物体都将对战争迷雾的显示层进行更新
    /// </summary>
    public void RevalMap() {
        #region 待重构
        //for (int i = 0; i < playersPositions.Count(); i++) {
        //    lock (playersPositions) {
        //        Vector3 vector3 = playersPositions[i];
        //        RevealUsingVision(vector3);
        //    }
        //}
        #endregion

        for (int i = 0; i < PresentationLayerObjects.Count(); i++) {
            lock (PresentationLayerObjects) {
                Vector3 vector3 = PresentationLayerObjects[i].Position;
                RevealUsingVision(vector3, PresentationLayerObjects[i].Radius);
            }
        }
    }

    /// <summary>
    /// 使用视野单位更新可见区域
    /// </summary>
    /// <param name="position">视野单位的坐标</param>
    /// <param name="viewRadius">视野单位的视野半径</param>
    public void RevealUsingVision(Vector3 position,float viewRadius) {

        //====================================
        // 第一步
        // 将视野单位的世界坐标转为贴图坐标
        float WorldToTex = textureSize / worldSize;
        position *= WorldToTex;

        //===================================
        // 第二步,根据视野单位的视野,设置可见范围,y轴表现在贴图坐标上是从下到上的
        float radius = viewRadius * WorldToTex;

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
            minMapBuffer[i].r = 0;
        }
    }

    //=======================================
    // 绘制小地图上单位的点
    // 此处使用的是逻辑层单位，因为地图上的点要绘制的是所有单位的
    public void DrawMinMap() {
        int width = 3;
        int height = 3;
        for (int i = 0; i < LogicalLayerObjects.Count(); i++) {
            lock (LogicalLayerObjects) {
                if (LogicalLayerObjects[i].isDying) continue;
                Vector3 position = LogicalLayerObjects[i].characterModel.Position;

                // 世界坐标转贴图坐标
                position *= textureSize / worldSize;

                if (LogicalLayerObjects[i] is HeroMono) {
                    width = 5;
                    height = 5;
                } else {
                    width = 3;
                    height = 3;
                }

                // 在当前贴图坐标下绘制一个小的绿色矩形
                int minX = Mathf.Clamp(Mathf.FloorToInt(position.x - width),0,textureSize) ;
                int minZ = Mathf.Clamp(Mathf.FloorToInt(position.z - height),0,textureSize) ;
                int maxX = Mathf.Clamp(Mathf.FloorToInt(position.x + width),0,textureSize) ;
                int maxZ = Mathf.Clamp(Mathf.FloorToInt(position.z + height),0,textureSize) ;

                for (int z=minZ;z<=maxZ;z++) {
                    int zw = z * textureSize;
                    for (int x=minX;x<=maxX;x++) {
                        if (LogicalLayerObjects[i].characterModel.unitFaction == UnitFaction.Red || LogicalLayerObjects[i].characterModel.unitFaction == UnitFaction.Neutral) {
                            minMapBuffer[zw + x].g = 255;
                            minMapBuffer[zw + x].r = 0;
                        } else if ((LogicalLayerObjects[i].characterModel.unitFaction == UnitFaction.Blue) || (LogicalLayerObjects[i].characterModel.unitFaction == UnitFaction.Hostility)) {
                            minMapBuffer[zw + x].r = 255;
                            minMapBuffer[zw + x].g = 0;
                        }
                    }
                }
            }
        }
    }
}


