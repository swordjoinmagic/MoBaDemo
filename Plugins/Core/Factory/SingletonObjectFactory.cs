using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {


    /// <summary>
    /// 单例模式工厂,所有对象只生成一次,且一直保持,
    /// 下次从工厂中获得对象依然是之前的那个
    /// </summary>
    public class SingletonObjectFactory : IObjectFactory {

        /// <summary>
        /// 该单例模式工厂类的核心属性,用于保存一个单例对象的字典,
        /// 其中该单例对象的Type是他的键
        /// </summary>
        private static Dictionary<Type, object> _cachedObjects = null;

        // 锁
        private static readonly object _lock = new object();

        private Dictionary<Type, object> CachedObjects {
            get {
                // 使用双重检查锁创建单例工厂
                if (_cachedObjects == null) {
                    lock (_lock) {
                        if (_cachedObjects == null) {
                            _cachedObjects = new Dictionary<Type, object>();
                        }
                    }
                }
                return _cachedObjects;
            }
        }
        public object AcquireObject(string className) {
            return AcquireObject(TypeFinder.ResolveType(className));
        }

        public object AcquireObject(Type type) {
            // 如果已经创建该单例对象，那么直接返回
            if (CachedObjects.ContainsKey(type)) {
                return CachedObjects[type];
            }
            /// 如果没有，则懒汉模式加载一个单例对象
            lock (_lock) {
                CachedObjects.Add(type,Activator.CreateInstance(type,false));
                return CachedObjects[type];
            }
        }

        public TInstance AcquireObject<TInstance>() where TInstance : class, new() {
            var type = typeof(TInstance);

            //如果已有该对象，返回
            if (CachedObjects.ContainsKey(type)) {
                return CachedObjects[type] as TInstance;
            }

            lock (_lock) {
                TInstance instance = new TInstance();
                CachedObjects.Add(type,instance);
                return CachedObjects[type] as TInstance;
            }
        }

        public void ReleaseObject(object obj) {
            // 等待GC自动回收
        }
    }
}
