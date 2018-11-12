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


    private CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Modify(HeroModel character) {

        //==================================================
        // 根据单位的属性来更改UI显示的结果
        attackText.text = character.TotalAttack.ToString();
        attackSpeedText.text = character.attackSpeed.ToString();
        attackDistanceText.text = character.attackDistance.ToString();
        moveSpeedText.text = character.movingSpeed.ToString();
        resotreMpSpeedText.text = character.resotreMpSpeed.ToString();
        defenseText.text = character.defense.ToString();
        physicalResistanceText.text = character.physicalResistance.ToString();
        magicalResistance.text = character.magicalResistance.ToString();
        dodgeRate.text = character.dodgeRate.ToString();
        restoreHpSpeed.text = character.restoreHpSpeed.ToString();

        //===========================================================
        // 设置单位的主属性
        HeroMainAttribute mainAttribute = character.mainAttribute;
        forcePowerMainAttributeText.gameObject.SetActive(mainAttribute == HeroMainAttribute.STR);
        agiPowerMainAttributeText.gameObject.SetActive(mainAttribute == HeroMainAttribute.AGI);
        intPowerMainAttributeText.gameObject.SetActive(mainAttribute == HeroMainAttribute.INT);

        // 设置属性面板的颜色，对于非主属性来说，它的面板颜色为黑色
        if (mainAttribute != HeroMainAttribute.STR) forcePowerImage.color = new Color(0, 0, 0, 100);
        if (mainAttribute != HeroMainAttribute.AGI) agiPowerImage.color = new Color(0, 0, 0, 100);
        if (mainAttribute != HeroMainAttribute.INT) intPowerImage.color = new Color(0, 0, 0, 100);

        //===========================================================
        // 设置力量属性的UI显示
        // 力量Text的显示变化
        forcePowerText.text = character.forcePower.ToString();

        // 力量增加 基本属性 Text的显示变化
        forcePowerDescriptionText.text = "= x点生命值 + x点生命值回复速度";     // ToDo~

        // 力量增加 主属性 Text的显示变化
        forcePowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~

        //===========================================================
        // 设置敏捷属性
        // 敏捷Text的显示变化
        agiPowerText.text = character.agilePower.ToString();

        // 敏捷增加 基本属性 Text的显示变化
        agiPowerDescriptionText.text = "= x点防御力 + x点攻击速度";     // ToDo~

        // 敏捷增加 主属性 Text的显示变化
        agiPowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~

        //===========================================================
        // 设置智力属性
        // 智力Text的显示变化
        intPowerText.text = character.intelligenceGrowthPoint.ToString();

        // 智力增加 基本属性 Text的显示变化
        intPowerDescriptionText.text = "= x点魔法值 + x点魔法回复速度";     // ToDo~

        // 智力增加 主属性 Text的显示变化
        intPowerMainAttributeText.text = "= x点攻击力（主属性加成）";   // ToDo~

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

