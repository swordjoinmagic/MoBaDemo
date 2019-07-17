using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {

    /// <summary>
    /// 传递给中介者的消息类
    /// T: T表示该消息的类型
    /// </summary>
    public class MessageArgs <T>{
        public T Item { get; set; }

        public MessageArgs(T item) {
            Item = item;
        }
    }
}
