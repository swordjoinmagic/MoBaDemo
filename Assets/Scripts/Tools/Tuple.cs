using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 自定义元组类型
public class Tuple<K,V> {
    K first;
    V second;

    public K First {
        get {
            return first;
        }

        set {
            first = value;
        }
    }

    public V Second {
        get {
            return second;
        }

        set {
            second = value;
        }
    }
}

