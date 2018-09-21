using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class BaseSkill {

    public string skillName;
    public Image icon;
    public string description;

    // 执行伤害效果
    public virtual Damage Execute() {
        return null;
    }
}
