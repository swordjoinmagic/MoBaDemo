using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace uMVVM {

    /// <summary>
    /// 拦截器接口，定义了AOP中拦截器的
    /// 拦截方法，以及方法完成前后需要额外
    /// 进行的操作
    /// </summary>
    public interface IInvocationHandler {
        /// <summary>
        /// 在方法执行前，执行的操作
        /// </summary>
        void PreProcess();

        /// <summary>
        /// 执行被代理的方法
        /// </summary>
        /// <param name="target">被拦截的目标对象</param>
        /// <param name="method">被拦截的方法</param>
        /// <param name="args">方法的参数</param>
        /// <returns></returns>
        object Invoke(object target,MethodInfo method,object[] args);

        /// <summary>
        /// 方法执行后,执行的操作
        /// </summary>
        void PostProcess();
    }
}
