using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 游戏商店的逻辑
/// </summary>
class StoreLogic {
    // 此商店贩卖的所有道具
    public List<ItemGrid> soldProps = new List<ItemGrid>();

    public StoreLogic() {
        for (int i=0;i<100;i++) {
            TestItemGrids().ItemCount = 1;
        }
    }

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
    public void Acquire(HeroMono heroMono, ItemGrid item) {
        if (item.item != null && item.ItemCount > 0) {
            heroMono.Owner.Money += Mathf.FloorToInt(item.item.price * 0.5f);
            item.ItemCount -= 1;
        }
    }

    public List<ItemGrid> FindItemsWithCommidtyType(CommditType commditType,int itemLevel=0) {
        List<ItemGrid> itemGrids = new List<ItemGrid>();
        switch (commditType) {
            case CommditType.Weapon:      // 武器
            case CommditType.Armor:       // 防具
            case CommditType.Consumable:  // 消耗品
            case CommditType.Decorative:  // 装饰品
            case CommditType.Other:       // 其他道具
                return soldProps.FindAll((ItemGrid itemGrid) => {
                    if (itemGrid.item != null) {
                        return itemGrid.item.commditType == commditType || commditType==CommditType.All;
                    }
                    return false;
                });
            default:
                return soldProps;
        }
    }

    /// <summary>
    /// 判断英雄是否有足够的金钱购买物品
    /// </summary>
    /// <returns></returns>
    public bool IsCanBuyItem(ItemGrid itemGrid,HeroMono heroMono) {
        if (itemGrid.item != null)
            return heroMono.Owner.Money >= itemGrid.item.price;
        else
            return false;
    }

    public ItemGrid TestItemGrids() {
        ItemGrid itemGrid = new ItemGrid {
            item = new Item {
                name = "测试物品"+Random.Range(0,1000),
                itemActiveSkill = new PointingSkill {
                    BaseDamage = 1000,
                    SpellDistance = 10,
                    CD = 3
                },
                itemType = ItemType.Consumed,
                maxCount = 10,
                iconPath = "00046",
                useMethodDescription = "使用：点目标",
                activeDescription = "对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害",
                passiveDescription = "+100攻击力\n+100防御力\n+10力量",
                backgroundDescription = "一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品",
                price = Random.Range(100, 10000),
                commditType = (CommditType)(Random.Range(0,6))
            }
        };
        soldProps.Add(itemGrid);
        return itemGrid;
    }
}
