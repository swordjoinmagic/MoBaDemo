using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {
    public static class ViewModelBaseExtensions {

        /// <summary>
        /// 对于ViewModel的扩展方法,返回当前ViewModel对象的一个指定的父节点,
        /// 这里用了一个小技巧,对ViewModel的所有Parent对象都进行指定类型强转,
        /// 如果强转失败,那么会返回null,所以最后会返回指定类型的ViewModel(当前ViewModel的祖先节点)
        /// </summary>
        /// <typeparam name="T">要找的祖先ViewModel的类型</typeparam>
        /// <param name="origin">当前ViewModel</param>
        /// <returns></returns>
        public static IEnumerable<T> Ancestors<T>(this ViewModelBase origin) where T : ViewModelBase {

            // 如果当前对象为空,那么终止迭代
            if (origin == null) {
                yield break;
            }
            ViewModelBase parentViewModel = origin.ParentViewModel;

            while (parentViewModel != null) {
                var castedViewModel = parentViewModel as T;
                if (castedViewModel != null) {
                    yield return castedViewModel;
                }
                parentViewModel = parentViewModel.ParentViewModel;
            }

        }
    }
}
