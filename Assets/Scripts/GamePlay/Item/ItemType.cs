using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[GameType]
public enum ItemType {
    // 用于消耗的物品,这种物品类型,在每次释放其主动技能后,
    // 物品数量会-1
    Consumed,

    // 永久的,此种物品除非卖掉,不然不会消失
    Permanent,

    // 装备类型
    Equipment
}

