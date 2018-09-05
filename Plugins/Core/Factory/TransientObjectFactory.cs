using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {
    /// <summary>
    /// 创建临时对象的工厂,每次请求都返回一个
    /// 不同的对象给调用者
    /// </summary>
    public class TransientObjectFactory : IObjectFactory {

        public object AcquireObject(string className) {
            return AcquireObject(TypeFinder.ResolveType(className));
        }

        public object AcquireObject(Type type) {
            var obj = Activator.CreateInstance(type,false);
            return obj;
        }

        public TInstance AcquireObject<TInstance>() where TInstance : class, new() {
            TInstance instance = new TInstance();
            return instance;
        }

        public void ReleaseObject(object obj) {
            // 等待GC自动回收
        }
    }
}
