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
        for (int i=0;i<500;i++) {
            TestItemGrids().ItemCount = 1;
        }
    }

    /// <summary>
    /// 更新商品复原情况
    /// </summary>
    public void UpdateCommodityRecoverySituation() {
        foreach (var itemGrid in soldProps) {
            // 如果当前物品的数量小于最大数量，那么对其回复进度进行刷新
            if (itemGrid.item != null && itemGrid.ItemCount < itemGrid.item.maxCount) {
                itemGrid.TimeProgressRate += Time.smoothDeltaTime;

                // 如果回复进度大于商品回复间隔，那么商品数量+1，同时回复进度置0
                if (itemGrid.TimeProgressRate >= itemGrid.item.itemPayInteral) {
                    // 如果该商品处于冷却状态(也就是被卖空了),那么将其冷却状态回复,同时不增加它的数量
                    if (itemGrid.IsCoolDowning)
                        itemGrid.IsCoolDowning = false;
                    else
                        // 商品不处于卖空的状态时,回复时间到,商品数量增加
                        itemGrid.ItemCount += 1;

                    // 重置回复进度
                    itemGrid.TimeProgressRate = 0;
                }
            } else if (itemGrid.item != null && itemGrid.ItemCount >= itemGrid.item.maxCount) {
                // 重置回复进度
                itemGrid.TimeProgressRate = 0;
            }
        }
    }

    /// <summary>
    /// 贩卖商品
    /// </summary>
    /// <param name="heroMono"></param>
    /// <param name="item"></param>
    public void Sell(HeroMono heroMono, ItemGrid item) {

        // 当玩家准备购买商品事件
        MessageAggregator.Instance.Broadcast<Player,ItemGrid>(EventType.OnPlayerPrepareBuyStoreItem,heroMono.Owner,item);

        // 判断价格是否足够,物品是否已空
        if (item.item != null && heroMono.Owner.Money >= item.item.price) {

            // 出售商品给单位
            heroMono.GetItem(item);
            heroMono.Owner.Money -= item.item.price;

            // 如果该物品的数量<=1,说明再减一就要为0了,在这里限制物品数量为1,当物品被买光后,设置此物品为正在冷却状态(isCoolDowning)
            if (item.ItemCount <= 1) {
                item.IsCoolDowning = true;
            } else
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
        int price = Random.Range(100, 500);
        ItemGrid itemGrid = new ItemGrid {
            item = new Item {
                name = "测试物品 价格:"+ price,
                itemActiveSkill = new PointingSkill(new SkillModel(new Tuple<string, object>[] {
                    new Tuple<string,object>{ First="Damage",Second=new Damage(1000,200) }
                }) {
                    SpellDistance = 10,
                    Cooldown = 3
                }),
                itemType = ItemType.Consumed,
                maxCount = 2,
                iconPath = "00046",
                useMethodDescription = "使用：点目标",
                activeDescription = "对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害对一个目标进行投掷，造成伤害",
                passiveDescription = "+100攻击力\n+100防御力\n+10力量",
                backgroundDescription = "一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品一个用来测试的物品",
                price = price,
                commditType = (CommditType)(Random.Range(0,6)),
                itemPayInteral = 5f,
                ItemId = 1
            }
        };
        soldProps.Add(itemGrid);
        return itemGrid;
    }
}
