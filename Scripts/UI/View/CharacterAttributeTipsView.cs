using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using uMVVM;
using UnityEngine.EventSystems;

public class CharacterAttributeTipsView : UnityGuiView<CharacterAttributeViewModel> {

    //==========================================
    // 此View管理的UI控件
    public Text attackSpeedText;
    public Text attackText;
    public Text attackDistanceText;
    public Text moveSpeedText;
    public Text resotreMpSpeedText;
    public Text defenseText;
    public Text physicalResistanceText;
    public Text magicalResistance;
    public Text dodgeRate;
    public Text restoreHpSpeed;
    public Text forcePowerText;
    public Text agiPowerText;
    public Text intPowerText;
    public Text forcePowerDescriptionText;
    public Text agiPowerDescriptionText;
    public Text intPowerDescriptionText;
    public Image forcePowerImage;
    public Image agiPowerImage;
    public Image intPowerImage;
    public Text forcePowerMainAttributeText;
    public Text agiPowerMainAttributeText;
    public Text intPowerMainAttributeText;

    protected override void OnInitialize() {
        base.OnInitialize();

        //==================================================
        // 属性绑定
        binder.Add<string>("attack",OnAttackChanged);
        binder.Add<string>("attackSpeed",OnAttackSpeedChanged);
        binder.Add<string>("attackDistance",OnAttackDistanceChanged);
        binder.Add<string>("moveSpeed",OnMoveSpeedChanged);
        binder.Add<string>("restoreHpSpeed",OnRestoreHpSpeedChanged);
        binder.Add<string>("defense",OnDefenseChanged);
        binder.Add<string>("physicalResistance",OnPhysicalResistanceChanged);
        binder.Add<string>("magicalResistance",OnMagicalResistanceChanged);
        binder.Add<string>("dodgeRate",OnDodgeRateChanged);
        binder.Add<string>("resotreMpSpeed",OnResotreMpSpeedChanged);
        binder.Add<string>("forcePower",OnForcePowerChanged);
        binder.Add<string>("agilePower",OnAgiPowerChanged);
        binder.Add<string>("intelligencePower",OnIntPowerChanged);
        binder.Add<HeroMainAttribute>("mainAttribute",OnHeroMainAttributeChanged);

    }

    private void OnAttackChanged(string oldAttack, string newAttack) {
        attackText.text = newAttack;
    }
    private void OnAttackSpeedChanged(string oldSpeed, string newSpeed) {
        attackSpeedText.text = newSpeed;
    }
    private void OnAttackDistanceChanged(string oldDistance, string newDistance) {
        attackDistanceText.text = newDistance;
    }
    private void OnMoveSpeedChanged(string oldSpeed, string newSpeed) {
        moveSpeedText.text = newSpeed;
    }
    private void OnResotreMpSpeedChanged(string oldSpedd, string newSpeed) {
        resotreMpSpeedText.text = newSpeed;
    }
    private void OnDefenseChanged(string oldDefense, string newDefense) {
        defenseText.text = newDefense;
    }
    private void OnPhysicalResistanceChanged(string oldValue, string newValue) {
        physicalResistanceText.text = newValue;
    }
    private void OnMagicalResistanceChanged(string oldValue, string newValue) {
        magicalResistance.text = newValue;
    }
    private void OnDodgeRateChanged(string oldRate,string newRate) {
        dodgeRate.text = newRate;
    }
    private void OnRestoreHpSpeedChanged(string oldSpeed,string newSpeep) {
        restoreHpSpeed.text = newSpeep;
    }
    private void OnHeroMainAttributeChanged(HeroMainAttribute oldValue, HeroMainAttribute newValue) {

        // 设置主属性
        forcePowerMainAttributeText.gameObject.SetActive(newValue==HeroMainAttribute.STR);
        agiPowerMainAttributeText.gameObject.SetActive(newValue == HeroMainAttribute.AGI);
        intPowerMainAttributeText.gameObject.SetActive(newValue == HeroMainAttribute.INT);

        // 设置属性面板的颜色，对于非主属性来说，它的面板颜色为黑色
        if (newValue != HeroMainAttribute.STR) forcePowerImage.color = new Color(0,0,0,100);
        if (newValue != HeroMainAttribute.AGI) agiPowerImage.color = new Color(0,0,0,100);
        if (newValue != HeroMainAttribute.INT) intPowerImage.color = new Color(0,0,0,100);
    }
    private void OnForcePowerChanged(string oldPower,string newPower) {

        // 力量Text的显示变化
        forcePowerText.text = newPower;

        // 力量增加 基本属性 Text的显示变化
        forcePowerDescriptionText.text = "= x点生命值 + x点生命值回复速度";     // ToDo~

        // 力量增加 主属性 Text的显示变化
        forcePowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~
    }
    private void OnAgiPowerChanged(string oldPower,string newPower) {
        // 敏捷Text的显示变化
        agiPowerText.text = newPower;

        // 敏捷增加 基本属性 Text的显示变化
        agiPowerDescriptionText.text = "= x点防御力 + x点攻击速度";     // ToDo~

        // 敏捷增加 主属性 Text的显示变化
        agiPowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~
    }
    private void OnIntPowerChanged(string oldPower,string newPower) {
        // 智力Text的显示变化
        intPowerText.text = newPower;

        // 智力增加 基本属性 Text的显示变化
        intPowerDescriptionText.text = "= x点魔法值 + x点魔法回复速度";     // ToDo~

        // 智力增加 主属性 Text的显示变化
        intPowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~
    }

}

