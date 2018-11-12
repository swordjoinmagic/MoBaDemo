using System;
using System.Linq;
using System.Text;
using UnityEngine;
using BehaviorDesigner;
using System.Collections;
using UnityEngine.AI;

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
    public Transform[] placesOfDispatch;
    // 小兵的对象池
    private GameObjectPool poolObjectFactory;
    // 小兵的预设集合，即每一次出兵会出现的那一群单位
    public GameObject[] solidersPrefabs;
    // 出兵频率
    public float deltaTime;

    // 游戏是否结束
    public bool isGameOver;

    public void Start() {
        poolObjectFactory = new GameObjectPool(50);

        StartCoroutine(DispatchSoliders());
    }

    //===========================
    // 出兵
    IEnumerator DispatchSoliders() {
        while (!isGameOver) {

            // 遍历每个出兵点，进行出兵
            foreach (var p in placesOfDispatch) {
                // 对每个出兵点产生一群单位
                foreach (var solider in solidersPrefabs) {
                    Vector3 position = (p.position + UnityEngine.Random.insideUnitSphere * 3);
                    GameObject soliderObject = poolObjectFactory.AcquireObject(position,templateObject:solider);
                    //FogSystem.Instace.AddListData<Transform>(soliderObject.transform,FogSystem.Instace.players);
                    //soliderObject.GetComponent<NavMeshAgent>().enabled = true;
                }
            }

            // 等待一段时间重新出兵
            yield return new WaitForSeconds(deltaTime);
        }
    }
}

