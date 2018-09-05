using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {

    /// <summary>
    /// 所有工厂公共的接口
    /// </summary>
    public interface IObjectFactory {
        /// <summary>
        /// 根据类名获得指定对象的实例
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        object AcquireObject(string className);
        /// <summary>
        /// 根据Type，对象的类型，来获得指定对象的实例
        /// </summary>
        /// <param name="type">指定要生成的对象的类型</param>
        /// <returns></returns>
        object AcquireObject(Type type);

        /// <summary>
        /// 使用泛型获得指定对象的实例,值得注意的是
        /// 这个泛型必须是引用类型,且拥有一个无参构造函数
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <returns></returns>
        TInstance AcquireObject<TInstance>() where TInstance : class, new();

        /// <summary>
        /// 工厂用于释放对象的方法,一般来说,
        /// 都是等待GC自动释放内存的
        /// </summary>
        /// <param name="obj"></param>
        void ReleaseObject(object obj);

    }
}
