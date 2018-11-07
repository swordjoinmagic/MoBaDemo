using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;

public class ItemViewModel : ViewModelBase{
    // 物品图片地址
    public BindableProperty<string> iconPath = new BindableProperty<string>();
    // 物品剩余数量
    public BindableProperty<int> itemCount = new BindableProperty<int>();
    // 物品名
    public BindableProperty<string> name = new BindableProperty<string>();
    // 物品被动效果描述
    public BindableProperty<string> passiveDescription = new BindableProperty<string>();
    // 物品使用方式描述
    public BindableProperty<string> useMethodDescription = new BindableProperty<string>();
    // 物品主动效果描述
    public BindableProperty<string> activeDescription = new BindableProperty<string>();
    // 物品背景描述
    public BindableProperty<string> backgroundDescription = new BindableProperty<string>();
    // 物品主动技能的冷却时间
    public BindableProperty<string> itemCD = new BindableProperty<string>();

    // 根据ItemGrid类来修改ViewModel
    public void Modify(ItemGrid itemGrid) {
        Item item = itemGrid.item;
        if (item != null) {
            iconPath.Value = item.iconPath;
            itemCount.Value = itemGrid.ItemCount;
            name.Value = item.name;
            passiveDescription.Value = item.passiveDescription;
            useMethodDescription.Value = item.useMethodDescription;
            activeDescription.Value = item.activeDescription;
            backgroundDescription.Value = item.backgroundDescription;
            if (item.itemActiveSkill != null)
                itemCD.Value = item.itemActiveSkill.CD.ToString();
        } else {
            iconPath.Value = null;
            itemCount.Value = 0;
        }
        
    }
}

