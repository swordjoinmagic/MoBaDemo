using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uMVVM;

public class HPViewModel : ViewModelBase{
    public BindableProperty<int> Hp = new BindableProperty<int>();
    public BindableProperty<int> Mp = new BindableProperty<int>();
    public BindableProperty<int> maxHp = new BindableProperty<int>();
    public BindableProperty<int> maxMp = new BindableProperty<int>();

    public void Init(CharacterModel characterModel) {
        maxHp.Value = characterModel.maxHp;
        maxMp.Value = characterModel.maxMp;
        Hp.Value = characterModel.Hp;
        Mp.Value = characterModel.Mp;
    }
}

