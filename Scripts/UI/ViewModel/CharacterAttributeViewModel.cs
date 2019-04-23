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
        string str = "{0} {1} <color=#00ff25>{2}</color>";
        if (characterModel.AttackPlus == 0)
            attack.Value = characterModel.Attack.ToString();
        else
            attack.Value = string.Format(str, characterModel.Attack, characterModel.AttackPlus>0?"+":"-",characterModel.AttackPlus);
        attackSpeed.Value = characterModel.AttackSpeed.ToString();
        attackDistance.Value = characterModel.attackDistance.ToString();
        moveSpeed.Value = characterModel.MovingSpeed.ToString();
        restoreHpSpeed.Value = characterModel.RestoreHpSpeed.ToString();
        defense.Value = characterModel.Defense.ToString();
        physicalResistance.Value = characterModel.PhysicalResistance.ToString();
        magicalResistance.Value = characterModel.MagicalResistance.ToString();
        dodgeRate.Value = characterModel.DodgeRate.ToString();
        resotreMpSpeed.Value = characterModel.ResotreMpSpeed.ToString();
        forcePower.Value = characterModel.ForcePower.ToString();
        agilePower.Value = characterModel.AgilePower.ToString();
        intelligencePower.Value = characterModel.IntelligencePower.ToString();
        mainAttribute.Value = characterModel.MainAttribute;
    }
}

