using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 物品格子类,是物品类的包装类型，
/// 它表示了在英雄物品栏中的一个个物品格子，
/// 其拥有当前物品持有数量等属性，用来处理具体的游戏逻辑。
/// </summary>
public class ItemGrid {

    public delegate void OnValueChangeHandler<T>(T oldValue,T newValue,int index);

    //============================
    // 用于监控itemCount和ItemPath的变化
    public OnValueChangeHandler<int> OnItemCountChanged;
    public OnValueChangeHandler<string> OnIconPathChanged;

    // 此格子存储的物品
    public Item item;
    // 当前持有该物品的数量
    private int itemCount;
    
    // 使用该物品的热键
    public KeyCode hotKey;

    // 当前各自标号
    public int index;

    public int ItemCount {
        get {
            return itemCount;
        }

        set {
            int oldItemCount = ItemCount;
            if (item == null) { itemCount = 0; return; }
            // 持有物品数量不能超过该物品的最大持有数量
            itemCount = Mathf.Clamp(value,0,item.maxCount) ;
            // 如果物品使用完毕,自动将该物品设置为null
            if (ItemCount == 0)
                item = null;

            if (oldItemCount == value)
                return;
            if(OnItemCountChanged!=null)
                OnItemCountChanged(oldItemCount,ItemCount,index);
            if(item!=null && OnIconPathChanged!=null)
                OnIconPathChanged(item.iconPath,item.iconPath,index);
            else
                OnIconPathChanged(null, null,index);
        }
    }

    // 当前物品的图片路径
    public string ItemImagePath {
        get {
            if (item != null)
                return item.iconPath;
            else
                return null;
        }
    }

    public void ExecuteItemSkill(CharacterMono speller,CharacterMono target) {
        if (item == null || itemCount == 0) return;
        item.itemActiveSkill.Execute(speller,target);

        // 如果该物品是消耗品,减少物品数量
        if (item.itemType == ItemType.Consumed)
            ItemCount -= 1;
    }
    public void ExecuteItemSkill(CharacterMono speller, Vector3 position) {
        if (item == null || itemCount == 0) return;
        item.itemActiveSkill.Execute(speller, position);

        // 如果该物品是消耗品,减少物品数量
        if (item.itemType == ItemType.Consumed)
            ItemCount -= 1;
    }

    public override string ToString() {
        return item == null ? "空物品格子" : item.name;
    }
}

