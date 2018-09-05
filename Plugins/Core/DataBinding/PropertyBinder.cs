using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace uMVVM {
    
    /// <summary>
    /// 属性绑定器,存在的意义是减少代码量,
    /// 即减少对于ViewModel里委托成对的+=,-=出现
    /// T:  泛型T表示任何ViewModel的子类
    /// 
    /// 
    /// 注: 属性绑定器的使用方法
    /// 这里说一下这个属性绑定器的使用方法,
    /// 首先,在一个实际的View视图类中的OnInitiallize初始化方法中使用Binder.Add<>()方法,
    /// 将对应属性的对应监听方法(所谓监听方法也就是对应属性发生改变时,监听方法触发)
    /// 添加到属性绑定器中
    /// 这样就完成了,在所有View类的父类UnityGuiView类中,用到了bind和unbind方法,
    /// 在视图绑定的ViewModel发生改变的时候,自动将旧ViewModel里的所有委托去除,
    /// 这依赖于使用Add方法的时候,将对应+=,-=方法都加进binders,unbinders数组中,
    /// 自动将旧ViewModel的全部委托一个个去除,并将新ViewModel中的委托一个个加进去,
    /// 就是遍历那两个数组,顺序执行里面的方法
    /// 
    /// </summary>
    public class PropertyBinder <T> where T:ViewModelBase {
        
        private delegate void BindHandler(T viewModel);
        private delegate void UnBindHandler(T viewModel);

        private readonly List<BindHandler> _binders = new List<BindHandler>();
        private readonly List<UnBindHandler> _unbinders = new List<UnBindHandler>();

        /// <summary>
        /// 根据可绑定属性的名字将一个监听这个属性变化的方法绑定在
        /// 属性上,也就是监听这个新的属性是否变化啦
        /// </summary>
        /// <typeparam name="TProperty">可绑定属性的类型</typeparam>
        /// <param name="name"></param>
        /// <param name="valueChangeHandler"></param>
        public void Add <TProperty> (string name,BindableProperty<TProperty>.OnValueChangeHandler valueChangeHandler) {

            // 获得该属性绑定器对应的ViewModel的字段描述
            var fieldInfo = typeof(T).GetField(name,BindingFlags.Instance | BindingFlags.Public);

            // 如果找不到对应的字段描述,抛出异常
            if (fieldInfo == null) {
                throw new Exception(string.Format("Unable to find bindableproperty field '{0}.{1}'", typeof(T).Name, name));
            }

            // 添加绑定方法
            _binders.Add(viewModel => {
                GetPropertyValue<TProperty>(name,viewModel,fieldInfo).OnValueChange += valueChangeHandler;
            });

            _unbinders.Add(viewModel => {
                GetPropertyValue<TProperty>(name, viewModel, fieldInfo).OnValueChange -= valueChangeHandler;
            });
        }

        /// <summary>
        /// 根据属性名(name),viewModel类型,还有该ViewModel类的字段描述(fieldInfo),
        /// 获得这个ViewModel里面一个可绑定属性(BinableProperty).
        /// </summary>
        /// <typeparam name="TProperty">返回的可绑定属性的类型</typeparam>
        /// <param name="name">返回的可绑定属性的名字</param>
        /// <param name="viewModel">要去获得可绑定属性的ViewModel</param>
        /// <param name="fieldInfo">该可绑定属性的字段描述(在Add方法执行的时候已经获得)</param>
        /// <returns></returns>
        private BindableProperty<TProperty> GetPropertyValue <TProperty> (string name,T viewModel,FieldInfo fieldInfo) {

            // value即是要获得的可绑定属性
            var value = fieldInfo.GetValue(viewModel);

            BindableProperty<TProperty> bindableProperty = value as BindableProperty<TProperty>;

            // 如果没有目标属性,抛出异常
            if (bindableProperty == null) {
                throw new Exception(string.Format("Illegal bindableproperty field '{0}.{1}' ", typeof(T).Name, name));
            }
            return bindableProperty;
        }

        /// <summary>
        /// 将监听方法绑定在目标属性上
        /// </summary>
        /// <param name="viewModel"></param>
        public void Bind(T viewModel) {
            if (viewModel != null) {
                for (int i=0;i<_binders.Count;i++) {
                    _binders[i](viewModel);
                }
            }
        }

        /// <summary>
        /// 将该方法从指定的ViewModel中去除
        /// </summary>
        /// <param name="viewModel"></param>
        public void Unbind(T viewModel) {
            if (viewModel!=null) {
                for (int i=0;i<_unbinders.Count;i++) {
                    _unbinders[i](viewModel);
                }
            }
        }
    }
}
