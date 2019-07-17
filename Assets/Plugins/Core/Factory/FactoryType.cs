using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uMVVM {
    public enum FactoryType {
        Sigleton,       // 单例工厂
        Transient,      // 临时对象工厂
        Pool            // 对象池工厂
    }
}
