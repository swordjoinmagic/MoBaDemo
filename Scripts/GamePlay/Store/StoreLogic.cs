using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 游戏商店的逻辑
/// </summary>
class StoreLogic {
    // 此商店贩卖的所有道具
    public List<ItemGrid> soldProps;

    /// <summary>
    /// 贩卖商品
    /// </summary>
    /// <param name="heroMono"></param>
    /// <param name="item"></param>
    public void Sell(HeroMono heroMono, ItemGrid item) {
        // 判断价格是否足够,物品是否已空
        if (item.item != null && heroMono.Owner.Money >= item.item.price) {
            // 出售商品给单位
            heroMono.GetItem(item);
            heroMono.Owner.Money -= item.item.price;
            item.ItemCount -= 1;
        }
    }

    /// <summary>
    /// 收购商品
    /// </summary>
    /// <param name="heroMono"></param>
    /// <param name="item"></param>
    public void Acquire(HeroMono heroMono,ItemGrid item) {
        if (item.item != null && item.ItemCount>0) {
            heroMono.Owner.Money += Mathf.FloorToInt(item.item.price * 0.5f);
            item.ItemCount -= 1;
        }
    }

}
