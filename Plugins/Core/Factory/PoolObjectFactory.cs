using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {
    /// <summary>
    /// 对象池,拥有一个最大对象数量_max,
    /// 每次有请求申请一个实例对象时,先去
    /// 池中查看是否有对象未在使用中,如果
    /// 有,则返回池中未被使用的对象,如果没有
    /// 则查看当前池中对象数量是否大于_max,
    /// 如果不大于,新创建一个对象,如果小于,等待
    /// </summary>
    public class PoolObjectFactory : IObjectFactory{

        private class PoolData {
            public bool InUse { get; set; }
            public object Obj { get; set; }
        }

        private readonly List<PoolData> _pool;
        /// <summary>
        /// 对象池中对象最大存放数量
        /// </summary>
        private readonly int _max;

        /// <summary>
        /// limit表示,如果对象池中的对象超过了最大限制
        /// max,是否限制再有新对象加入对象池
        /// </summary>
        private readonly bool _limit;

        public PoolObjectFactory(int max,bool limit) {
            _max = max;
            _limit = limit;
            _pool = new List<PoolData>();
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
        private PoolData GetPoolData(object obj) {
            lock (_pool) {
                foreach (PoolData p in _pool) {
                    if (p.Obj == obj) {
                        return p;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取对象池中的对象
        /// </summary>
        /// <param name="type">对象的Type</param>
        /// <returns></returns>
        private object GetObject(Type type) {
            lock (_pool) {
                // 判断要获取的对象的类型在池中是否有,
                // 需要注意的一点是,某一个对象池只保持
                // 同一对象的类型,所以在这里进行判断
                if (_pool.Count > 0) {
                    if (_pool[0].Obj.GetType() != type) {
                        throw new Exception(string.Format("the Pool Factory only for Type :{0}", _pool[0].Obj.GetType().Name));
                    }
                }

                // 如果要获取的对象,在对象池中还有剩余
                // 返回这个剩余的对象
                for (int i = 0; i < _pool.Count; i++) {
                    PoolData p = _pool[i];
                    if (!p.InUse) {
                        p.InUse = true;
                        return p.Obj;
                    }
                }

                if (_pool.Count >= _max && _limit) {
                    throw new Exception("max limit is arrived.");
                }

                // 如果对象池中找不到要获取的对象,或者要获取的对象全部都在使用中,
                // 那么新创建一个对象
                object obj = Activator.CreateInstance(type,false);
                PoolData p1 = new PoolData { InUse=true,Obj=obj };
                _pool.Add(p1);
                return obj;
            }
        }

        private void PutObject(object obj) {
            // 获得这个obj对象在池中的PoolData
            PoolData p = GetPoolData(obj);
            // 将该PoolData设为未使用状态
            if (p != null) {
                p.InUse = false;
            }
        }

        public object AcquireObject(string className) {
            return GetObject(TypeFinder.ResolveType(className));
        }

        public object AcquireObject(Type type) {
            return GetObject(type);
        }

        public TInstance AcquireObject<TInstance>() where TInstance : class, new() {
            return AcquireObject(typeof(TInstance)) as TInstance;
        }

        /// <summary>
        /// 释放对象,在对象池中,如果对象池已满,
        /// 那么会真正的释放掉一个对象,如果对象池
        /// 未满,那么仅仅是将该对象的使用状态标记为
        /// 未使用而已
        /// </summary>
        /// <param name="obj"></param>
        public void ReleaseObject(object obj) {
            if (_pool.Count > _max) {
                if (obj is IDisposable) {
                    ((IDisposable)obj).Dispose();
                }
                var p = GetPoolData(obj);
                lock (_pool) {
                    _pool.Remove(p);
                }
                return;
            }
            // 如果对象池未满,将要释放的对象
            // 在对象池中标记为未使用
            PutObject(obj);
        }
    }
}
