using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;

/// <summary>
/// 简易人物窗口的ViewModel类，仅有两个属性，
/// hp及姓名，用于显示人物的血条及姓名。
/// </summary>
public class SimpleCharacterViewModel : ViewModelBase{
    public BindableProperty<int> maxHp = new BindableProperty<int>();
    public BindableProperty<int> Hp = new BindableProperty<int>();
    public BindableProperty<string> name = new BindableProperty<string>();

    /// <summary>
    /// 基于人物类来更改此ViewModel
    /// </summary>
    /// <param name="characterModel"></param>
    public void Modify(CharacterModel characterModel) {
        maxHp.Value = characterModel.maxHp;
        Hp.Value = characterModel.Hp;
        name.Value = characterModel.Name;
    }
}

