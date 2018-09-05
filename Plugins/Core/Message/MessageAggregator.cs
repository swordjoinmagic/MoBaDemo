using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace uMVVM {

    /// <summary>
    /// 处理消息来时的委托，即所有处理方法的原型
    /// </summary>
    /// <typeparam name="T">消息参数的类型</typeparam>
    /// <param name="sender">消息的发送者</param>
    /// <param name="args">消息参数</param>
    public delegate void MessageHandler<T>(object sender,MessageArgs<T> args);

    /// <summary>
    /// 负责转发消息和订阅消息的中介者类，可以
    /// 降低各个对象之间的耦合度，让各个对象不显式
    /// 引用对方，而是都通过中介者来进行。
    /// 
    /// 使用方法:
    ///     在要订阅消息的ViewModel中的构造方法里订阅消息.
    ///     在要发布消息的ViewModel中编写发布消息的方法,
    ///     然后当要发布消息的时候,调用发布消息的方法
    /// </summary>
    public class MessageAggregator <T> {
        /// <summary>
        /// 中介者的核心,
        /// 键是消息ID,唯一.
        /// 值是消息来时的处理函数,
        /// 由Subscribe进行订阅
        /// </summary>
        private readonly Dictionary<string, MessageHandler<T>> _messages = new Dictionary<string, MessageHandler<T>>();

        // 使用饿汉模式构造单例类
        public static readonly MessageAggregator<T> Instance = new MessageAggregator<T>();

        // 私有化构造方法
        private MessageAggregator() { }

        /// <summary>
        /// 订阅消息,可以这么理解:
        /// 当某个消息(根据消息ID判断)来的时候,
        /// 告诉中介者这个消息应该有订阅这个消息
        /// 的类来处理这样子.
        /// </summary>
        public void Subscribe(string name, MessageHandler<T> handler) {
            if (!_messages.ContainsKey(name)) {
                _messages.Add(name, handler);
            } else {
                _messages[name] += handler;
            }
        }

        /// <summary>
        /// 发布消息,一般用于在某个对象的某些状态
        /// 改变时,为了告诉另一个对象(但是又不想显式
        /// 调用),于是将这个状态改变的消息发送给中介者,
        /// 中介者接收到消息后,将消息转发给被通知的那个
        /// 对象处理.
        /// 在这里表现为执行处理函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Publish(string name, object sender, MessageArgs<T> args) {
            if (_messages.ContainsKey(name) && _messages[name]!=null) {

                Debug.Log("Publish方法被执行");

                // 转发
                _messages[name](sender,args);
            }
        }
    }
}
