using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 物品合成系统,绑定人物的GetItem方法,
/// 当人物得到物品的时候,判断是否可以使用新得到的物品和已有物品进行合成
/// </summary>
public class ItemCompositeSystem {

    /// <summary>
    /// 合成装备,此方法监听getItem事件
    /// </summary>
    /// <param name="characterMono">得到新物品的单位</param>
    /// <param name="itemGrid">新的物品</param>
    public void Composite(CharacterMono characterMono, ItemGrid itemGrid) {
        // 物品非空
        if (itemGrid.item != null && itemGrid.ItemCount != 0) {

            // 遍历新得到的物品的后缀装备列表
            foreach (var itemID in itemGrid.item.SuffixEquipments) {

                // 遍历该装备的前缀列表,查看是否满足合成条件

            }

        }
    }
}

