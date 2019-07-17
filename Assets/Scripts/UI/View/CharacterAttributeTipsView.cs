using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using uMVVM;
using UnityEngine.EventSystems;

public class CharacterAttributeTipsView : MonoBehaviour {

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
    public Text forcePowerGrowthText;
    public Text agiPowerGrowthText;
    public Text intPowerGrowthText;

    private CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Modify(HeroModel character) {

        #region 基本属性与其Plus属性
        //==================================================
        // 根据单位的属性来更改UI显示的结果
        string str = "{0} {1} <color=#00ff25>{2}</color>";

        // 计算基本值与附加值
        if (character.AttackPlus == 0)
            attackText.text = character.Attack.ToString();
        else
            attackText.text = string.Format(str, character.Attack, character.AttackPlus > 0 ? "+" : "-", character.AttackPlus);
        
        attackSpeedText.text = character.AttackSpeed.ToString();

        if(character.AttackDistancePlus == 0)
            attackDistanceText.text = character.attackDistance.ToString();
        else
            attackDistanceText.text = string.Format(str, character.attackDistance, character.AttackDistancePlus > 0 ? "+" : "-", character.AttackDistancePlus); 

        if (character.MovingSpeedPlus == 0)
            moveSpeedText.text = character.MovingSpeed.ToString();
        else
            moveSpeedText.text = string.Format(str, character.MovingSpeed, character.MovingSpeedPlus > 0 ? "+" : "-", character.MovingSpeedPlus);

        if (character.ResotreMpSpeedPlus == 0)
            resotreMpSpeedText.text = character.ResotreMpSpeed.ToString();
        else
            resotreMpSpeedText.text = string.Format(str, character.ResotreMpSpeed, character.ResotreMpSpeedPlus > 0 ? "+" : "-", character.ResotreMpSpeedPlus);


        // 计算基本属性与附加值
        if (character.DefensePlus == 0) 
            defenseText.text = character.Defense.ToString();
        else
            defenseText.text = string.Format(str, character.Defense, character.DefensePlus > 0 ? "+" : "-", character.DefensePlus);

        // 计算基本属性与附加值
        if (character.PhysicalResistancePlus == 0)
            physicalResistanceText.text = character.PhysicalResistance.ToString();
        else
            physicalResistanceText.text = string.Format(str, character.PhysicalResistance, character.PhysicalResistancePlus > 0 ? "+" : "-", character.PhysicalResistancePlus);

        // 计算基本属性与附加值
        if (character.MagicalResistancePlus == 0)
            magicalResistance.text = character.MagicalResistance.ToString();
        else
            magicalResistance.text = string.Format(str, character.MagicalResistance, character.MagicalResistancePlus > 0 ? "+" : "-", character.MagicalResistancePlus);

        // 计算基本属性与附加值
        if (character.DodgeRatePlus == 0)
            dodgeRate.text = character.DodgeRate.ToString();
        else
            dodgeRate.text = string.Format(str, character.DodgeRate, character.DodgeRatePlus > 0 ? "+" : "-", character.DodgeRatePlus);

        // 计算基本属性与附加值
        if (character.RestoreHpSpeedPlus == 0)
            restoreHpSpeed.text = character.RestoreHpSpeed.ToString();
        else
            restoreHpSpeed.text = string.Format(str, character.RestoreHpSpeed, character.RestoreHpSpeedPlus> 0 ? "+" : "-", character.RestoreHpSpeedPlus);

        #endregion

        #region 主属性
        //===========================================================
        // 设置单位的主属性
        HeroMainAttribute mainAttribute = character.MainAttribute;
        forcePowerMainAttributeText.gameObject.SetActive(mainAttribute == HeroMainAttribute.STR);
        agiPowerMainAttributeText.gameObject.SetActive(mainAttribute == HeroMainAttribute.AGI);
        intPowerMainAttributeText.gameObject.SetActive(mainAttribute == HeroMainAttribute.INT);

        // 将所有面板初始化为对应颜色
        forcePowerImage.color = new Color(1, 0, 0, 100f/255);
        agiPowerImage.color = new Color(0, 1, 0, 100f / 255);
        intPowerImage.color = new Color(0, 0, 1, 100f / 255);

        // 设置属性面板的颜色，对于非主属性来说，它的面板颜色为黑色
        if (mainAttribute != HeroMainAttribute.STR) forcePowerImage.color = new Color(0, 0, 0, 100f / 255);
        if (mainAttribute != HeroMainAttribute.AGI) agiPowerImage.color = new Color(0, 0, 0, 100f / 255);
        if (mainAttribute != HeroMainAttribute.INT) intPowerImage.color = new Color(0, 0, 0, 100f / 255);

        //===========================================================
        // 设置力量属性的UI显示
        // 力量Text的显示变化
        forcePowerText.text = character.ForcePower.ToString();

        // 力量增加 基本属性 Text的显示变化
        forcePowerDescriptionText.text = "= x点生命值 + x点生命值回复速度";     // ToDo~

        // 力量增加 主属性 Text的显示变化
        forcePowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~

        // 力量成长点
        forcePowerGrowthText.text = string.Format("(每级获得{0}点)",character.ForcePowerGrowthPoint);

        //===========================================================
        // 设置敏捷属性
        // 敏捷Text的显示变化
        agiPowerText.text = character.AgilePower.ToString();

        // 敏捷增加 基本属性 Text的显示变化
        agiPowerDescriptionText.text = "= x点防御力 + x点攻击速度";     // ToDo~

        // 敏捷增加 主属性 Text的显示变化
        agiPowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~

        agiPowerGrowthText.text = string.Format("(每级获得{0}点)", character.AgilePowerGrowthPoint);

        //===========================================================
        // 设置智力属性
        // 智力Text的显示变化
        intPowerText.text = character.IntelligenceGrowthPoint.ToString();

        // 智力增加 基本属性 Text的显示变化
        intPowerDescriptionText.text = "= x点魔法值 + x点魔法回复速度";     // ToDo~

        // 智力增加 主属性 Text的显示变化
        intPowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~

        intPowerGrowthText.text = string.Format("(每级获得{0}点)", character.IntelligenceGrowthPoint);
        #endregion
    }

    public void Reveal() {
        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1;

    }
    public void Hide() {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;
    }
}

