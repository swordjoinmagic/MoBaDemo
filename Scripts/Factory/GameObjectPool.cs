using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using uMVVM;

/// <summary>
/// 游戏对象的对象池工厂
/// </summary>
public class GameObjectPool{

    private class PoolData {
        public bool InUse { get; set; }
        public GameObject Obj { get; set; }
    }

    private List<PoolData> pools;

    private int max;
    public GameObjectPool(int max) {
        this.max = max;
        pools = new List<PoolData>(max);
    }

    public GameObject GetGameObjectWithPool(Vector3 position,GameObject templateObject=null) {
        lock (pools) {
            lock (pools) {
                // 判断要获取的对象的类型在池中是否有
                // 如果要获取的对象,在对象池中还有剩余
                // 返回这个剩余的对象
                for (int i = 0; i < pools.Count; i++) {
                    PoolData p = pools[i];
                    if (!p.InUse) {
                        p.InUse = true;
                        return p.Obj;
                    }
                }

                // 如果对象池中找不到要获取的对象,或者要获取的对象全部都在使用中,
                // 那么新创建一个对象(从模板对象处进行创建)
                if (templateObject == null) return null;        // 如果没有模板对象，那么返回null
                GameObject obj = GameObject.Instantiate(templateObject, position, Quaternion.identity);
                PoolData p1 = new PoolData { InUse = true, Obj = obj };
                pools.Add(p1);
                return obj;
            }
        }
    }

    /// <summary>
    /// 在池中找到跟指定对象obj一样的对象,
    /// 主要是为了在后面对PoolData的InUse属性
    /// 赋值的时候,要找到该PoolData
    /// 
    /// 备注:
    ///     这一段代码加锁的原因是,防止在多线程环境下,
    ///     _pool临时又增加了对象,导致代码的不确定性(?),
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private PoolData GetPoolData(GameObject obj) {
        lock (pools) {
            foreach (PoolData p in pools) {
                if (p.Obj == obj) {
                    return p;
                }
            }
        }
        return null;
    }


    private void PutObject(GameObject obj) {
        // 获得这个obj对象在池中的PoolData
        PoolData p = GetPoolData(obj);
        // 将该PoolData设为未使用状态
        if (p != null) {
            p.InUse = false;
        }
    }

    /// <summary>
    /// 释放对象,在对象池中,如果对象池已满,
    /// 那么会真正的释放掉一个对象,如果对象池
    /// 未满,那么仅仅是将该对象的使用状态标记为
    /// 未使用而已
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseObject(GameObject obj) {
        if (pools.Count > max) {
            obj.SetActive(false);
            GameObject.Destroy(obj);
            var p = GetPoolData(obj);
            lock (pools) {
                pools.Remove(p);
            }
            return;
        }
        // 如果对象池未满,将要释放的对象
        // 在对象池中标记为未使用
        PutObject(obj);
    }


    public GameObject AcquireObject(Vector3 position,GameObject templateObject=null) {
        return GetGameObjectWithPool(position, templateObject);
    }
}

