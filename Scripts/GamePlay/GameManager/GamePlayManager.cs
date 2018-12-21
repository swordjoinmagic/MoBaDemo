using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;

/// <summary>
/// 用于管理游戏逻辑的Manager类
/// 这里指的游戏逻辑是，在我搭好技能、人物、AI、物品、装备等基本系统之后，
/// 游戏规则的逻辑，即如果是MOBA游戏，则用于管理出兵，英雄复活，玩家经济，人头数量计算等等逻辑
/// 如果是大逃杀游戏，用来管理英雄复活，玩家出生，空投等等逻辑
/// 是指游戏的高层逻辑，而技能、装备等基础系统不算在内,
/// 
/// 单例类
/// </summary>
public class GamePlayManager : MonoBehaviour{

    private static GamePlayManager instance;
    public static GamePlayManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GamePlayManager>();
            }
            return instance;
        }
    }

    // 出兵地点
    public Vector3[] placesOfDispatchRed;
    public Vector3[] placesOfDispatchBlue;
    // 塔集合
    public List<GameObject> towersRedUp;
    public List<GameObject> towersRedMiddle;
    public List<GameObject> towersRedDown;
    public List<GameObject> towersBlueUp;
    public List<GameObject> towersBlueMiddle;
    public List<GameObject> towersBlueDown;

    // 小兵的对象池
    private GameObjectPool poolObjectFactory;
    // 小兵的预设集合，即每一次出兵会出现的那一群单位
    public GameObject[] solidersPrefabs;
    // 出兵频率
    public float deltaTime;
    // 玩家队伍集合
    public UnitFaction[] playerTeams;

    // 游戏是否结束
    public bool isGameOver;

    // 产生对象的事件,用于解耦网络联机产生NPC的逻辑
    public delegate void CreateNPCGameObject(Vector3 position, GameObject soliderObject, GameObjectPool gameObjectPool);
    public event CreateNPCGameObject OnCreateNPCGameObject;

    public void Init() {

        Debug.Log("初始化GamePlayManager");

        poolObjectFactory = new GameObjectPool(50);

        StartCoroutine(DispatchSoliders());
    }

    //===========================
    // 出兵
    IEnumerator DispatchSoliders() {
        yield return new WaitForSeconds(2);
        while (!isGameOver) {

            Debug.Log("开始出兵");


            int a = 0;

            // 遍历每个出兵点，进行出兵
            foreach (var p in placesOfDispatchRed) {
                // 对每个出兵点产生一群单位
                foreach (var solider in solidersPrefabs) {
                    Vector3 position = (p + UnityEngine.Random.insideUnitSphere * 3);
                    GameObject soliderObject = poolObjectFactory.AcquireObject(position, templateObject: solider);

                    // 设置该单位的阵营
                    soliderObject.GetComponent<CharacterMono>().characterModel.unitFaction = UnitFaction.Red;

                    // 触发产生对象的事件
                    if (OnCreateNPCGameObject != null) OnCreateNPCGameObject(position,soliderObject, poolObjectFactory);

                    if (a == 0) {
                        soliderObject.GetComponent<CharacterMono>().wayPointsUnit = new WayPointsUnit(WayPointEnum.UpRoad, UnitFaction.Red);
                    } else if (a == 1) {
                        soliderObject.GetComponent<CharacterMono>().wayPointsUnit = new WayPointsUnit(WayPointEnum.MiddleRoad, UnitFaction.Red);
                    } else if (a == 2) {
                        soliderObject.GetComponent<CharacterMono>().wayPointsUnit = new WayPointsUnit(WayPointEnum.DownRoad, UnitFaction.Red);
                    }
                    // 设置战争迷雾
                    FogSystem.Instace.AddFOVUnit(soliderObject.GetComponent<CharacterMono>());

                }

                a++;
            }

            int b = 0;
            foreach (var p in placesOfDispatchBlue) {
                // 对每个出兵点产生一群单位
                foreach (var solider in solidersPrefabs) {
                    Vector3 position = (p + UnityEngine.Random.insideUnitSphere * 3);
                    GameObject soliderObject = poolObjectFactory.AcquireObject(position, templateObject: solider);

                    // 设置该单位的阵营
                    soliderObject.GetComponent<CharacterMono>().characterModel.unitFaction = UnitFaction.Blue;

                    // 触发产生对象的事件
                    if (OnCreateNPCGameObject != null) OnCreateNPCGameObject(position, soliderObject,poolObjectFactory);

                    if (b == 0) {
                        soliderObject.GetComponent<CharacterMono>().wayPointsUnit = new WayPointsUnit(WayPointEnum.UpRoad, UnitFaction.Blue);

                    } else if (b == 1) {
                        soliderObject.GetComponent<CharacterMono>().wayPointsUnit = new WayPointsUnit(WayPointEnum.MiddleRoad, UnitFaction.Blue);
                    } else if (b == 2) {
                        soliderObject.GetComponent<CharacterMono>().wayPointsUnit = new WayPointsUnit(WayPointEnum.DownRoad, UnitFaction.Blue);
                    }

                    // 设置战争迷雾
                    FogSystem.Instace.AddFOVUnit(soliderObject.GetComponent<CharacterMono>());
                }
                b++;
            }

            // 等待一段时间重新出兵
            yield return new WaitForSeconds(deltaTime);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        foreach (var position in placesOfDispatchRed) {
            Gizmos.DrawSphere(position,1f);
        }
        Gizmos.color = Color.blue;
        foreach (var position in placesOfDispatchBlue) {
            Gizmos.DrawSphere(position, 1f);
        }
    }
}

