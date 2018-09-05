using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace uMVVM {

    /// <summary>
    /// 所有View视图类的基类,继承Unity3D的MonoBehaviour类
    /// 以及实现IView接口,得以绑定一个ViewModel属性,
    /// 
    /// UnityGuiView定义了所有View视图类的生命周期
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UnityGuiView<T> : MonoBehaviour, IView<T> where T : ViewModelBase {

        protected CanvasGroup canvasGroup;

        /// <summary>
        /// 表示该view是否已初始化
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// 表示是否在该View隐藏后将其摧毁
        /// </summary>
        public bool destroyOnHide = false;

        /// <summary>
        /// 该视图绑定的ViewModel
        /// </summary>
        public readonly BindableProperty<T> viewModelProperty = new BindableProperty<T>();

        protected readonly PropertyBinder<T> binder = new PropertyBinder<T>();

        /// <summary>
        /// 实现接口,将该视图跟一个ViewModel绑定
        /// </summary>
        public T BindingContext {
            get {
                return viewModelProperty.Value;
            }
            set {

                // 如果该view还没有进行初始化, 那么进行一次初始化
                if (!_isInitialized) {
                    OnInitialize();
                    _isInitialized = true;
                }

                // 因为viewModelProperty是BinableProperty属性
                // 所以,在执行set方法时,会自动执行监听事件
                viewModelProperty.Value = value;
            }
        }

        /// <summary>
        /// 当本视图绑定的ViewModel改变时,执行的监听方法,
        /// 用virtual关键字修饰,主要是为了将来在子类中复写.
        /// 使用属性绑定器后:
        /// 无需在子类中编写成对的+=,-=方法,使用Unbind一键将
        /// 旧ViewModel中的所有委托去除,使用Bind方法将原先ViewModel中
        /// 的委托一个个加回到新ViewModel中
        /// </summary>
        /// <param name="oldViewModel"></param>
        /// <param name="newViewModel"></param>
        protected virtual void OnBindingContextChanged(T oldViewModel, T newViewModel) {
            binder.Unbind(oldViewModel);
            binder.Bind(newViewModel);
        }

        /// <summary>
        /// View的初始化方法,当BindContext(也就是这个View绑定的ViewModel)改变的时候执行.
        /// 只执行一次
        /// 
        /// 注: 在 @木宛城主 的代码中,OnInitialize方法的触发是在第一次为该View绑定的ViewModel
        /// 赋值的时候触发的,在这里注解一下,为什么不能将OnInitialize直接放在构造函数中,因为当
        /// 这个新的视图类被new出来的时候,其实他还没有绑定ViewModel,所以此时的ViewModel是Null,
        /// 如果此时触发OnInitialize方法,回应发空指针错误,所以将其放在第一次为该ViewModel赋值的时候
        /// 触发
        /// </summary>
        protected virtual void OnInitialize() {
            // 无论ViewModel的Value怎么变化,只对OnValueChanged事件监听一次
            viewModelProperty.OnValueChange += OnBindingContextChanged;


            // 初始化CanvasGroup组件
            canvasGroup = GetComponent<CanvasGroup>();

        }


        //==============================================================
        // 下列方法用于管理View的生命周期
        // 
        // 一个View视图在游戏中执行的基本顺序如下:
        //      OnInitialize ->
        //      OnAppear ->
        //      OnReveal ->
        //      OnRevealed ->
        //      OnHide ->
        //      OnHidden ->
        //      OnDisappear ->
        //      OnDestroy
        //
        //      其中OnAppear,OnReveal,OnRevealed可以归类到Reveal(显示)方法中
        //      OnHide,OnHidden,OnDisappear可以归类到Hide(隐藏)方法中
        //==============================================================

        /// <summary>
        /// 表示显示之后的回调函数
        /// </summary>
        public Action RevealedAction { get; set; }

        /// <summary>
        /// 表示隐藏之后的回调函数
        /// </summary>
        public Action HiddenAction { get; set; }

        /// <summary>
        /// View显示时执行的方法,包含了三个流程,
        /// OnAppear,OnReveal,OnRevealed
        /// 
        /// </summary>
        /// <param name="immediate">是否立刻显示View</param>
        /// <param name="action">在View显示完成后执行的回调函数</param>
        public void Reveal(bool immediate = false,Action action = null) {
            if (action != null) {
                RevealedAction += action;
            }

            OnAppear();
            OnReveal(immediate);
            OnRevealed();
        }

        /// <summary>
        /// View隐藏时执行的方法,包含了三个流程,
        /// OnHide,OnHidden,OnDisappear
        /// </summary>
        /// <param name="immediate">是否立刻隐藏</param>
        /// <param name="action">隐藏完成后调用的回调方法</param>
        public void Hide(bool immediate = false,Action action = null) {
            if (action != null) {
                HiddenAction += action;
            }

            OnHide(immediate);
            OnHidden();
            OnDisappear();
        }

        /// <summary>
        /// View准备显示时调用的方法,主要用于激活GameObject,
        /// Disable -> Enable
        /// </summary>
        public virtual void OnAppear() {
            gameObject.SetActive(true);

            // 调用ViewModel准备开始时的逻辑
            BindingContext.OnStartReveal();
        }

        /// <summary>
        /// 开始显示,如果不立即进行显示,那么
        /// 就会播放动画进行显示
        /// </summary>
        /// <param name="immediate">是否立刻显示</param>
        private void OnReveal(bool immediate) {
            if (immediate) {
                // 立即显示
                transform.localScale = Vector3.one;
                canvasGroup.alpha = 1;
            } else {
                // 播放动画
                StartAnimatedReveal();
            }
        }

        protected virtual void StartAnimatedReveal() {
            print("播放doTween动画");
            canvasGroup.interactable = false;
            transform.localScale = Vector3.one;

            // 使用DoTween动画淡入
            canvasGroup.DOFade(1, 0.2f).SetDelay(0.2f).OnComplete(() => {
                canvasGroup.interactable = true;
            });
        }

        /// <summary>
        /// 完成显示后执行的操作
        /// </summary>
        public virtual void OnRevealed() {

            // 执行完成显示后的逻辑
            BindingContext.OnFinishReveal();

            if (RevealedAction != null) {
                RevealedAction();
            }
        }

        /// <summary>
        /// 开始显示时进行的操作
        /// </summary>
        /// <param name="immediate"></param>
        public void OnHide(bool immediate) {
            BindingContext.OnStartHide();

            if (immediate) {
                // 立即隐藏
                transform.localScale = Vector3.zero;
                canvasGroup.alpha = 0;
            } else {
                // 播放动画
                StartAnimatedHide();
            }
        }

        /// <summary>
        /// 进行隐藏时播放的动画
        /// </summary>
        protected virtual void StartAnimatedHide() {
            canvasGroup.interactable = false;
            canvasGroup.DOFade(0,0.2f).SetDelay(0.2f).OnComplete(()=> {
                transform.localScale = Vector3.zero;
                canvasGroup.interactable = true;
            });
        }

        /// <summary>
        /// 隐藏完后调用的方法
        /// </summary>
        public virtual void OnHidden() {
            // 调用回调函数
            if (HiddenAction != null) {
                HiddenAction();
            }
        }

        /// <summary>
        /// View消失时调用的方法,
        /// Enable -> Disable
        /// </summary>
        public virtual void OnDisappear() {
            gameObject.SetActive(false);
            BindingContext.OnFinishHide();

            if (destroyOnHide) {
                // 销毁该View
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 当gameOject将被摧毁时,
        /// 引擎自动调用该方法
        /// </summary>
        public virtual void OnDestroy() {

            // 如果该View已经显示完毕,先隐藏,再进行销毁
            if (BindingContext.IsRevealed) {
                Hide(immediate:true);
            }

            BindingContext.OnDestory();
            // 清空该View绑定的ViewModel
            BindingContext = null;
            viewModelProperty.OnValueChange = null;
        }
    }
}
