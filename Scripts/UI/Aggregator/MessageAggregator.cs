using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate void CallBack<T, X>(T arg, X arg1);
public delegate void CallBack<T, X, Y>(T arg, X arg1, Y arg2);
public delegate void CallBack<T, X, Y, Z>(T arg, X arg1, Y arg2, Z arg3);
public delegate void CallBack<T, X, Y, Z, W>(T arg, X arg1, Y arg2, Z arg3, W arg4);

/// <summary>
/// 充当所有UI视图对象的中介者，单例类，具有两个功能
/// 
/// 1. 订阅事件：
///     向某种事件发起订阅，参数是一个delegate委托函数，
///     表示当某个事件发生后，调用该函数
/// 2. 发布事件：
///     发布事件相当于我们没有用中介者之前的的OnXXX委托，
///     在某个事件发生时，调用该中介者的发布事件方法，
///     用以调用所有订阅该事件的对象的方法
/// </summary>
public class MessageAggregator {

    // 单例类，使用饿汗模式加载，防止在多线程环境下出错
    public static readonly MessageAggregator Instance = new MessageAggregator();
    private MessageAggregator() { }

    private Dictionary<EventType, Delegate> _messages;

    public Dictionary<EventType, Delegate> Messages {
        get {
            if (_messages == null) _messages = new Dictionary<EventType, Delegate>();
            return _messages;
        }
    }

    /// <summary>
    /// 当订阅事件时调用
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callback"></param>
    private void OnListenerAdding(EventType eventType,Delegate callback) {
        //判断字典里面是否包含该事件码
        if (!Messages.ContainsKey(eventType)) {
            Messages.Add(eventType, null);
        }
        Delegate d = Messages[eventType];
        if (d != null && d.GetType() != callback.GetType()) {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托是{2}", eventType, d.GetType(), callback.GetType()));
        }
    }


    /// <summary>
    /// 当取消订阅事件时调用
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    private void OnListenerRemoving(EventType eventType,Delegate callBack) {
        if (Messages.ContainsKey(eventType)) {
            Delegate d = Messages[eventType];
            if (d == null) {
                throw new Exception(string.Format("移除监听事件错误：事件{0}没有对应的委托", eventType));
            } else if (d.GetType() != callBack.GetType()) {
                throw new Exception(string.Format("移除监听事件错误：尝试为事件{0}移除不同类型的委托，当前事件所对应的委托为{1}，要移除的委托是{2}", eventType, d.GetType(), callBack.GetType()));
            }
        } else {
            throw new Exception(string.Format("移除监听事件错误：没有事件码{0}", eventType));
        }
    }

    #region 订阅事件的方法
    /// <summary>
    /// 无参的监听事件（即订阅事件）的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void AddListener(EventType eventType,CallBack callBack) {
        OnListenerAdding(eventType, callBack);
        Messages[eventType] = (CallBack)Messages[eventType] + callBack;
    }

    /// <summary>
    /// 1参的监听事件（即订阅事件）的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void AddListener<T>(EventType eventType, CallBack<T> callBack) {
        OnListenerAdding(eventType, callBack);
        Messages[eventType] = (CallBack<T>)Messages[eventType] + callBack;
    }

    /// <summary>
    /// 2参的监听事件（即订阅事件）的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void AddListener<T,X>(EventType eventType, CallBack<T,X> callBack) {
        OnListenerAdding(eventType, callBack);
        Messages[eventType] = (CallBack<T,X>)Messages[eventType] + callBack;
    }

    /// <summary>
    /// 3参的监听事件（即订阅事件）的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void AddListener<T,X,V>(EventType eventType, CallBack<T,X,V> callBack) {
        OnListenerAdding(eventType, callBack);
        Messages[eventType] = (CallBack<T,X,V>)Messages[eventType] + callBack;
    }

    /// <summary>
    /// 4参的监听事件（即订阅事件）的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void AddListener<T,X,Y,Z>(EventType eventType, CallBack<T,X,Y,Z> callBack) {
        OnListenerAdding(eventType, callBack);
        Messages[eventType] = (CallBack<T, X, Y, Z>)Messages[eventType] + callBack;
    }

    /// <summary>
    /// 5参的监听事件（即订阅事件）的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void AddListener<T, X, Y, Z,W>(EventType eventType, CallBack<T, X, Y, Z,W> callBack) {
        OnListenerAdding(eventType, callBack);
        Messages[eventType] = (CallBack<T, X, Y, Z,W>)Messages[eventType] + callBack;
    }

    #endregion

    #region 移除监听事件的方法

    /// <summary>
    /// 无参的移除监听事件的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RemoveListener(EventType eventType,CallBack callBack) {
        OnListenerRemoving(eventType, callBack);
        Messages[eventType] = (CallBack)Messages[eventType] - callBack;
    }

    /// <summary>
    /// 1参的移除监听事件的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RemoveListener<T>(EventType eventType, CallBack<T> callBack) {
        OnListenerRemoving(eventType, callBack);
        Messages[eventType] = (CallBack<T>)Messages[eventType] - callBack;
    }


    /// <summary>
    /// 2参的移除监听事件的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack) {
        OnListenerRemoving(eventType, callBack);
        Messages[eventType] = (CallBack<T, X>)Messages[eventType] - callBack;
    }

    /// <summary>
    /// 3参的移除监听事件的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RemoveListener<T, X, V>(EventType eventType, CallBack<T, X, V> callBack) {
        OnListenerRemoving(eventType, callBack);
        Messages[eventType] = (CallBack<T, X, V>)Messages[eventType] - callBack;
    }

    /// <summary>
    /// 4参的移除监听事件的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack) {
        OnListenerRemoving(eventType, callBack);
        Messages[eventType] = (CallBack<T, X, Y, Z>)Messages[eventType] - callBack;
    }

    /// <summary>
    /// 5参的移除监听事件的方法
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RemoveListener<T, X, Y, Z,W>(EventType eventType, CallBack<T, X, Y, Z,W> callBack) {
        OnListenerRemoving(eventType, callBack);
        Messages[eventType] = (CallBack<T, X, Y, Z,W>)Messages[eventType] - callBack;
    }

    #endregion


    #region 广播事件的方法

    /// <summary>
    /// 无参的广播监听事件
    /// </summary>
    /// <param name="eventType"></param>
    public void Broadcast(EventType eventType) {
        Delegate d;
        if (Messages.TryGetValue(eventType, out d)) {
            CallBack callBack = d as CallBack;
            if (callBack != null)
                callBack();
            else
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托有不同的类型", eventType));
        }
    }

    /// <summary>
    /// 1参的广播监听事件
    /// </summary>
    /// <param name="eventType"></param>
    public void Broadcast<T>(EventType eventType,T arg0) {
        Delegate d;
        if (Messages.TryGetValue(eventType, out d)) {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
                callBack(arg0);
            else
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托有不同的类型", eventType));
        }
    }

    /// <summary>
    /// 2参的广播监听事件
    /// </summary>
    /// <param name="eventType"></param>
    public void Broadcast<T,V>(EventType eventType, T arg0,V arg1) {
        Delegate d;
        if (Messages.TryGetValue(eventType, out d)) {
            CallBack<T,V> callBack = d as CallBack<T,V>;
            if (callBack != null)
                callBack(arg0,arg1);
            else
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托有不同的类型", eventType));
        }
    }

    /// <summary>
    /// 3参的广播监听事件
    /// </summary>
    /// <param name="eventType"></param>
    public void Broadcast<T,V,X>(EventType eventType, T arg0,V arg1,X arg2) {
        Delegate d;
        if (Messages.TryGetValue(eventType, out d)) {
            CallBack<T,V,X> callBack = d as CallBack<T,V,X>;
            if (callBack != null)
                callBack(arg0,arg1,arg2);
            else
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托有不同的类型", eventType));
        }
    }

    /// <summary>
    /// 4参的广播监听事件
    /// </summary>
    /// <param name="eventType"></param>
    public void Broadcast<T, V, X,Z>(EventType eventType, T arg0, V arg1, X arg2,Z arg3) {
        Delegate d;
        if (Messages.TryGetValue(eventType, out d)) {
            CallBack<T, V, X,Z> callBack = d as CallBack<T, V, X,Z>;
            if (callBack != null)
                callBack(arg0, arg1, arg2,arg3);
            else
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托有不同的类型", eventType));
        }
    }

    /// <summary>
    /// 5参的广播监听事件
    /// </summary>
    /// <param name="eventType"></param>
    public void Broadcast<T, V, X, Z,W>(EventType eventType, T arg0, V arg1, X arg2, Z arg3,W arg4) {
        Delegate d;
        if (Messages.TryGetValue(eventType, out d)) {
            CallBack<T, V, X, Z,W> callBack = d as CallBack<T, V, X, Z,W>;
            if (callBack != null)
                callBack(arg0, arg1, arg2, arg3,arg4);
            else
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托有不同的类型", eventType));
        }
    }


    #endregion
}

