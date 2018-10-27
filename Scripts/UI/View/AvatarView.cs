using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;
using UnityEngine.UI;

public class AvatarView : UnityGuiView<AvatarViewModel>{

    //==========================
    // 此View管理的UI控件

    // 头像
    public RawImage AvatarImage;
    // 名字
    public string heroName;
    // 等级
    public int level;
    // 
}

