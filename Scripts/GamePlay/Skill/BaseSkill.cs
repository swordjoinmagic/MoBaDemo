using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class BaseSkill {

    public string skillName;

    // 图标地址，用地址保存，当要使用时进行加载
    public string iconPath;

    public string description;

    // 执行伤害效果
    public virtual Damage Execute() {
        return null;
    }
}
