using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Item {
    // 物品名
    public string name;
    // 物品价格
    public int price;
    // 物品被动效果描述
    public string passiveDescription;
    // 物品使用方式说明
    public string useMethodDescription;
    // 物品主动效果描述
    public string activeDescription;
    // 物品背景描述
    public string backgroundDescription;
    // 物品图片地址
    public string iconPath;
    // 单个物品可持有的最大数量
    public int maxCount;
    // 物品类型
    public ItemType itemType;
    // 物品在商店里的类型
    public CommditType commditType;
    // 物品购买间隔
    public float itemPayInteral;
    // 物品特技
    public ActiveSkill itemActiveSkill;
    // 物品被动特技(可用于增益属性,增加特效等等)
    public List<PassiveSkill> itemPassiveSkills = new List<PassiveSkill>();
}

