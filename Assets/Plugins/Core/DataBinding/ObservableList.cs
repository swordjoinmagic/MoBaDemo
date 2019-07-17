using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM{
    /// <summary>
    /// 动态数据集合,当集合添加、删除数据的时候,
    /// 提供一种通知机制,告诉UI动态更新界面.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableList<T> : IList<T> {

        /// <summary>
        /// 当列表被初始化或者重置的时候，
        /// 触发该方法，通知UI界面动态更新
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public delegate void ValueChangeHandler(List<T> oldValue, List<T> newValue);
        public ValueChangeHandler OnValueChanged;

        private List<T> _value = new List<T>();

        public T this[int index] {
            get { return _value[index]; }
            set { _value[index] = value; }
        }

        public List<T> Value {
            get { return _value; }
            set {
                if (!Equals(_value, value)) {
                    List<T> old = _value;
                    _value = value;
                    ValueChanged(old, _value);
                }
            }
        }

        private void ValueChanged(List<T> oldValue, List<T> newValue) {
            if (OnValueChanged != null) {
                OnValueChanged(oldValue, newValue);
            }
        }

        /// <summary>
        /// 在该List添加数据的时候触发的事件
        /// </summary>
        /// <param name="instance"></param>
        public delegate void AddHandler(T instance);
        public AddHandler OnAdd;

        /// <summary>
        /// 在该List插入数据的时候触发的事件
        /// </summary>
        /// <param name="instance"></param>
        public delegate void InsertHandler(int index, T instance);
        public InsertHandler OnInsert;

        /// <summary>
        /// 在该List删除数据的时候触发的事件
        /// </summary>
        /// <param name="instance"></param>
        public delegate void RemoveHandler(T instance);
        public RemoveHandler OnRemove;

        /// <summary>
        /// 添加数据的方法
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item) {
            _value.Add(item);

            if (OnAdd != null) {
                OnAdd(item);
            }
        }

        public bool Remove(T item) {
            if (_value.Remove(item)) {
                if (OnRemove != null) {
                    OnRemove(item);
                }
                return true;
            }
            return false;
        }
        public void RemoveAt(int index) {
            _value.RemoveAt(index);
        }

        public void Insert(int index, T item) {
            _value.Insert(index, item);

            if (OnInsert != null) {
                OnInsert(index, item);
            }
        }

        public int IndexOf(T item) {
            return _value.IndexOf(item);
        }

        public bool IsReadOnly {
            get; private set;
        }

        public int Count {
            get { return _value.Count; }
        }

        public void Clear() {
            _value.Clear();
        }

        public bool Contains(T item) {
            return _value.Contains(item);
        }

        //================
        // 迭代器
        //================
        public IEnumerator<T> GetEnumerator() {
            return _value.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        //===============
        // 复制
        //==============
        public void CopyTo(T[] array,int arrayIndex) {
            _value.CopyTo(array,arrayIndex);
        }
    }
}
