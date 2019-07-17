using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {
    /// <summary>
    /// 基于【服务定位】模式实现IOC
    /// 在内部维护一个字典，字典的值是Func，
    /// 一个匿名函数，通过该匿名函数实现对象的
    /// 注入，每次要获得目标对象的时候，使用如下方式：
    ///     container[type]()
    /// 即可获得一个对象
    /// </summary>
    public class ServiceLocator {
        // 单例工厂
        private static SingletonObjectFactory _singletonObjectFactory = new SingletonObjectFactory();
        // 临时对象工厂
        private static TransientObjectFactory _transientObjectFactory = new TransientObjectFactory();

        /// <summary>
        /// 服务定位模式的核心对象,用于以对象的类型为键保存一个被注入的对象实例
        /// </summary>
        private static readonly Dictionary<Type, Func<object>> Container = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// 通过工厂模式来获得一个具体的实例对象,
        /// 这里的Lazy函数主要用于返回一段根据工厂
        /// 创建实例对象的匿名函数
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="factoryType"></param>
        /// <returns></returns>
        private static Func<object> Lazy<TInstance>(FactoryType factoryType) where TInstance : class, new(){
            return () => {
                switch (factoryType) {
                    case FactoryType.Sigleton:
                        return _singletonObjectFactory.AcquireObject<TInstance>();
                    default:
                        return _transientObjectFactory.AcquireObject<TInstance>();
                }
            };
        }

        /// <summary>
        /// 在IOC容器中注册一个单例类,
        /// 对每一个请求,只返回唯一的实例,
        /// 该重载函数添加一个以某个接口作为引用类型的
        /// 实例类
        /// 
        /// 备注:
        ///     要创建的对象必须含有一个无参的构造方法
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterSingleton<TInterface, TInstance>() where TInstance : class, new() {
            Container.Add(typeof(TInterface),Lazy<TInstance>(FactoryType.Sigleton));
        }

        /// <summary>
        /// 单例类注册函数,重载函数2:
        ///     该重载函数向IOC容器添加一个以该对象类型
        ///     作为引用类型的实例类
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterSingleton<TInstance>() where TInstance:class,new() {
            Container.Add(typeof(TInstance),Lazy<TInstance>(FactoryType.Sigleton));
        }

        /// <summary>
        /// 单例类注册函数,重载函数3:
        ///     向该IOC容器添加一个返回TInstance类型的匿名函数
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="func"></param>
        public static void RegisterSingleton<TInstance>(Func<object> func) where TInstance : class, new() {
            Container.Add(typeof(TInstance),func);
        }

        /// <summary>
        /// 临时对象注册函数:
        ///     该重载函数向IOC容器添加一个返回TInstance类型的匿名函数
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterTransient<TInstance>() where TInstance:class,new(){
            Container.Add(typeof(TInstance),Lazy<TInstance>(FactoryType.Sigleton));
        }

        /// <summary>
        /// 临时对象注册函数:
        ///     该重载函数向IOC容器添加一个返回以TInterface为引用类型,TInstance为实例的
        ///     对象的匿名函数
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterTransient<TInterface,TInstance>() where TInstance:class,new(){
            Container.Add(typeof(TInterface),Lazy<TInstance>(FactoryType.Transient));
        }

        /// <summary>
        /// 向该IOC容器添加一个返回TInstance类型的匿名函数
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="func"></param>
        public static void RegisterTransient <TInstance> (Func<object> func) where TInstance:class,new() {
            Container.Add(typeof(TInstance),func);
        }

        /// <summary>
        /// 清空容器
        /// </summary>
        public static void Clear() {
            Container.Clear();
        }

        /// <summary>
        /// 从IOC容器中获取一个对象实例,
        /// 只有当真正要获取一个对象的时候,
        /// 才会调用工厂模式进行对象的创建
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Resolve<TInterface>() where TInterface:class {
            return Resolve(typeof(TInterface)) as TInterface;
        }

        private static object Resolve(Type type) {
            if (!Container.ContainsKey(type)) {
                return null;
            }
            return Container[type]();
        }

    }
}
