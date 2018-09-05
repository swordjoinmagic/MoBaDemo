using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uMVVM {

    /// <summary>
    /// 可绑定属性，当该属性值发生改变时，
    /// 触发监听事件（也就是一个委托）
    /// </summary>
    /// <typeparam name="T">该可绑定属性的类型</typeparam>
    public class BindableProperty <T>{

        public delegate void OnValueChangeHandler(T oldValue,T newValue);

        T _value;
        /// <summary>
        /// 触发的监听方法，由外部进行赋值
        /// </summary>
        public OnValueChangeHandler OnValueChange;

        public T Value {
            get {
                return _value;
            }

            set {
                // 当值发生改变时，触发监听事件
                if (!Equals(_value, value)) {
                    T oldValue = _value;
                    _value = value;
                    if (OnValueChange != null)
                        OnValueChange(oldValue,_value);
                }
            }
        }

        public override string ToString() {
            if (_value != null) {
                return "BindableProperty Value : " + this._value.ToString();
            } else {
                return "BindableProperty Value : null";
            }
        }
    }
}
