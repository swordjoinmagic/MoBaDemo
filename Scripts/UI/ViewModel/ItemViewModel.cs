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

    // 根据ItemGrid类来修改ViewModel
    public void Init(ItemGrid itemGrid) {
        Item item = itemGrid.item;
        if (item != null) {
            iconPath.Value = item.iconPath;
            itemCount.Value = itemGrid.ItemCount;
        } else {
            iconPath.Value = null;
            itemCount.Value = 0;
        }
        
    }
}

