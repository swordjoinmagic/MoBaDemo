using UnityEngine;
using uMVVM;

public class AvatarViewModel : ViewModelBase{
    // 姓名
    public BindableProperty<string> Name = new BindableProperty<string>();
    // 攻击力
    public BindableProperty<int> Attack = new BindableProperty<int>();
    // 防御力
    public BindableProperty<int> Defense = new BindableProperty<int>();
    // 移动速度
    public BindableProperty<int> MoveSpeed = new BindableProperty<int>();
    // 力量
    public BindableProperty<int> ForcePower = new BindableProperty<int>();
    // 敏捷
    public BindableProperty<int> AgilePower = new BindableProperty<int>();
    // 智力
    public BindableProperty<int> IntelligencePower = new BindableProperty<int>();
    // 单位图片
    public BindableProperty<string> AvatarImagePath = new BindableProperty<string>();
    // 英雄等级
    public BindableProperty<int> Level = new BindableProperty<int>();

    public void Modify(HeroModel hero) {
        Name.Value = hero.name;
        Attack.Value = hero.attack;
        Defense.Value = hero.defense;
        MoveSpeed.Value = hero.movingSpeed;
        ForcePower.Value = Mathf.FloorToInt(hero.forcePower); 
        AgilePower.Value = Mathf.FloorToInt(hero.agilePower);
        IntelligencePower.Value = Mathf.FloorToInt(hero.intelligencePower);
        AvatarImagePath.Value = hero.AvatarImagePath;
        Level.Value = hero.level;
    }
}

