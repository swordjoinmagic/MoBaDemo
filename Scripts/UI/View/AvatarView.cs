using uMVVM;
using UnityEngine.UI;
using UnityEngine;
public class AvatarView : UnityGuiView<AvatarViewModel> {

    //==========================
    // 此View管理的UI控件
    // 头像
    public RawImage AvatarImage;
    // 名字
    public Text heroNameText;
    // 等级
    public Text levelText;
    // 攻击、防御、移动速度
    public Text attackText;
    public Text defenseText;
    public Text moveSpeedText;
    // 力量、敏捷、魔力
    public Text forcePowerText;
    public Text agilePowerText;
    public Text intelligencePowerText;
    // 经验值
    public Text expText;
    // 经验值图片
    public RectTransform expImage;

    protected override void OnInitialize() {
        base.OnInitialize();
        binder.Add<string>("Name",OnNameChanged);
        binder.Add<int>("Attack",OnAttackChanged);
        binder.Add<int>("Defense",OnDefenseTextChanged);
        binder.Add<int>("MoveSpeed", OnMoveSpeedChanged);
        binder.Add<int>("ForcePower", OnForcePowerChanged);
        binder.Add<int>("AgilePower", OnAgilePowerChanged);
        binder.Add<int>("IntelligencePower", OnIntelligencePowerChanged);
        binder.Add<int>("Level",OnLevelChanged);
        binder.Add<int>("ExpRate",OnExpTextChanged);
        binder.Add<string>("AvatarImagePath",OnAvatarImageChanged);
    }
    
    public void OnAvatarImageChanged(string oldImagePath,string newImagePath) {
        AvatarImage.texture = Resources.Load<Texture>("UIImage/" + newImagePath);
    }
    public void OnNameChanged(string oldName, string newName) {
        Debug.Log("姓名改版");
        heroNameText.text = newName;
    }
    public void OnLevelChanged(int oldLevel, int newLevel) {
        levelText.text = newLevel.ToString();
    }
    public void OnAttackChanged(int oldAttack, int newAttack) {
        attackText.text = newAttack.ToString();
    }
    public void OnDefenseTextChanged(int oldDefense, int newDefense) {
        defenseText.text = newDefense.ToString();
    }
    public void OnMoveSpeedChanged(int oldMoveSpeed, int newMoveSpeed) {
        moveSpeedText.text = newMoveSpeed.ToString();
    }
    public void OnForcePowerChanged(int oldForcePower, int newForcePower) {
        forcePowerText.text = newForcePower.ToString();
    }
    public void OnAgilePowerChanged(int oldAgilePower, int newAgilePower) {
        agilePowerText.text = newAgilePower.ToString();
    }
    public void OnIntelligencePowerChanged(int oldIntelligencePower,int newIntelligencePower) {
        intelligencePowerText.text = newIntelligencePower.ToString();
    }
    public void OnExpTextChanged(int oldExpRate,int newExpRate) {
        expText.text = "EXP:"+newExpRate + "%";
        Debug.Log("newExpRate:"+newExpRate);
        expImage.sizeDelta = new Vector2(expImage.sizeDelta.x,newExpRate);
    }
}

