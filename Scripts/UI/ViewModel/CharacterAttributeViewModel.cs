using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using uMVVM;

public class CharacterAttributeViewModel : ViewModelBase{

    public BindableProperty<string> attack = new BindableProperty<string>();
    public BindableProperty<string> attackSpeed = new BindableProperty<string>();
    public BindableProperty<string> attackDistance = new BindableProperty<string>();
    public BindableProperty<string> moveSpeed = new BindableProperty<string>();
    public BindableProperty<string> restoreHpSpeed = new BindableProperty<string>();
    public BindableProperty<string> defense = new BindableProperty<string>();
    public BindableProperty<string> physicalResistance = new BindableProperty<string>();
    public BindableProperty<string> magicalResistance = new BindableProperty<string>();
    public BindableProperty<string> dodgeRate = new BindableProperty<string>();
    public BindableProperty<string> resotreMpSpeed = new BindableProperty<string>();
    public BindableProperty<string> forcePower = new BindableProperty<string>();
    public BindableProperty<string> agilePower = new BindableProperty<string>();
    public BindableProperty<string> intelligencePower = new BindableProperty<string>();
    public BindableProperty<HeroMainAttribute> mainAttribute = new BindableProperty<HeroMainAttribute>();

    public void Modify(HeroModel characterModel) {
        attack.Value = characterModel.attack.ToString();
        attackSpeed.Value = characterModel.attackSpeed.ToString();
        attackDistance.Value = characterModel.attackDistance.ToString();
        moveSpeed.Value = characterModel.movingSpeed.ToString();
        restoreHpSpeed.Value = characterModel.restoreHpSpeed.ToString();
        defense.Value = characterModel.defense.ToString();
        physicalResistance.Value = characterModel.physicalResistance.ToString();
        magicalResistance.Value = characterModel.magicalResistance.ToString();
        dodgeRate.Value = characterModel.dodgeRate.ToString();
        resotreMpSpeed.Value = characterModel.resotreMpSpeed.ToString();
        forcePower.Value = characterModel.forcePower.ToString();
        agilePower.Value = characterModel.agilePower.ToString();
        intelligencePower.Value = characterModel.intelligencePower.ToString();
        mainAttribute.Value = characterModel.mainAttribute;
    }
}

