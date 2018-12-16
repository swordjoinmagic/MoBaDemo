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
    public BindableProperty<float> MoveSpeed = new BindableProperty<float>();
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
    // 当前经验值百分比
    public BindableProperty<int> ExpRate = new BindableProperty<int>();

    public AvatarViewModel() {
        Level.Value = -1;
        ExpRate.Value = -1;
    }

    public void Modify(HeroModel hero) {
        Name.Value = hero.Name;
        Attack.Value = hero.TotalAttack;
        Defense.Value = hero.TotalDefense;
        MoveSpeed.Value = hero.TotalMovingSpeed;
        ForcePower.Value = Mathf.FloorToInt(hero.forcePower); 
        AgilePower.Value = Mathf.FloorToInt(hero.agilePower);
        IntelligencePower.Value = Mathf.FloorToInt(hero.intelligencePower);
        AvatarImagePath.Value = hero.AvatarImagePath;
        Level.Value = hero.Level;
        ExpRate.Value = hero.Exp / hero.NextLevelNeedExp;
    }
}

