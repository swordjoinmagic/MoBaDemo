using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM{

    /// <summary>
    /// 动态代理对象
    /// </summary>
    public class Proxy {
        public static Proxy Instance = new Proxy();

        // 拦截器
        private IInvocationHandler invocationHandler;
        // 拦截对象
        private object target;
        // 拦截方法
        private string method;
        // 拦截方法的参数
        private object[] args;

        private Proxy() { }

        /// <summary>
        /// 设置代理的拦截器
        /// </summary>
        /// <param name="invocationHandler"></param>
        /// <returns></returns>
        public Proxy SetInvocationHandler(IInvocationHandler invocationHandler) {
            this.invocationHandler = invocationHandler;
            return this;
        }

        /// <summary>
        /// 设置代理的拦截对象
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Proxy SetTarger(object target) {
            this.target = target;
            return this;
        }

        /// <summary>
        /// 设置代理拦截的方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public Proxy SetMethod(string method) {
            this.method = method;
            return this;
        }

        /// <summary>
        /// 设置代理拦截的方法的参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Proxy SetArgs(object[] args) {
            this.args = args;
            return this;
        }

        /// <summary>
        /// 执行经过代理包装后的方法
        /// </summary>
        /// <returns></returns>
        public object Invoke() {
            var methodInfo = target.GetType().GetMethod(method);
            return invocationHandler.Invoke(target,methodInfo,args);
        }
    }
}
