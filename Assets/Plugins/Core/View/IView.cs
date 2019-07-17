using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {

    public interface IView <T> where T:ViewModelBase {
        /// <summary>
        /// BindingContext属性为每一个View(视图)提供一个
        /// ViewModel支持,简单来说,只要view视图类实现了
        /// IView接口,那么这个视图就跟一个ViewModel绑定了
        /// </summary>
        T BindingContext {
            get; set;
        }
    }
}
