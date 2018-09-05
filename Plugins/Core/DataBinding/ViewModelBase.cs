using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {
    /// <summary>
    /// 所有ViewModel的基类,
    /// 定义了ViewModel的生命周期,
    /// ViewModel的生命周期是和View联系在一起的,
    /// 在View进入某个阶段时(显示,隐藏,运行中),
    /// 由View调用ViewModel的各个方法(OnHide,OnFinishHide等等)
    /// </summary>
    public class ViewModelBase {
        // 该ViewModel是否已被初始化
        private bool _isInitialized;

        /// <summary>
        /// 父节点的ViewModel
        /// </summary>
        public ViewModelBase ParentViewModel {
            get; set;
        }

        public bool IsRevealed {
            get; private set;
        }

        /// <summary>
        /// 表示该ViewModel是否正在执行显示中的逻辑
        /// </summary>
        public bool IsRevealInProgress {
            get; private set;
        }

        /// <summary>
        /// 表示该ViewModel是否正在执行隐藏中的逻辑
        /// </summary>
        public bool IsHideInProgress {
            get; private set;
        }

        protected virtual void OnInitialize() { }

        /// <summary>
        /// 在View开始显示时,执行的方法
        /// </summary>
        public virtual void OnStartReveal() {
            IsRevealInProgress = true;
            // 在一开始显示的时候,执行初始化方法
            if (!_isInitialized) {
                OnInitialize();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// 在View显示完成后,ViewModel要额外执行的逻辑
        /// </summary>
        public virtual void OnFinishReveal() {
            IsRevealInProgress = false;
            IsRevealed = true;
        }

        /// <summary>
        /// 开始隐藏View时,ViewModel执行的方法
        /// </summary>
        public virtual void OnStartHide() {
            IsHideInProgress = true;
        }

        /// <summary>
        /// View隐藏完成后,ViewModel执行的方法
        /// </summary>
        public virtual void OnFinishHide() {
            IsHideInProgress = false;
            IsRevealed = false;
        }

        public virtual void OnDestory() { }
    }
}
