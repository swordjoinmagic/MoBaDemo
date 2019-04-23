using uMVVM;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class AvatarView : MonoBehaviour {

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


    // 人物属性信息提示窗口prefab
    public CharacterAttributeTipsView characterAttributeTipsViewPrefab;

    private EventTrigger eventTrigger;
    private CharacterAttributeTipsView TipsViewInstance;        // 提示信息窗口实例
    private GameObject canvas;
    private RectTransform rectTransform;
    private Camera UICamera;

    // 该属性UI对应的单位的属性
    private HeroMono characterMono;

    public void Init(HeroMono heroMono) {
        this.characterMono = heroMono;
        Init();
    }

    public void Init() {
        canvas = GameObject.Find("Canvas");
        rectTransform = transform as RectTransform;
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        eventTrigger = GetComponent<EventTrigger>();

        #region 为属性UI绑定鼠标事件
        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener(eventData => {
            if (TipsViewInstance == null) {
                TipsViewInstance = Instantiate<CharacterAttributeTipsView>(characterAttributeTipsViewPrefab, canvas.transform);
            }
            TipsViewInstance.Modify(characterMono.HeroModel);

            // 获得AvaterView视图的屏幕坐标
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, rectTransform.position);
            // 获得AvaterView视图锚点在Canvas中心的Anchors坐标
            Vector2 anchorsPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, UICamera, out anchorsPos);

            // 设置该视图的RectTransform
            RectTransform tipsViewRectTransform = TipsViewInstance.transform as RectTransform;
            tipsViewRectTransform.anchoredPosition = new Vector2(
                anchorsPos.x + rectTransform.sizeDelta.x / 2,
                anchorsPos.y
            );

            TipsViewInstance.Reveal();
        });
        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener(eventData => {
            TipsViewInstance.Hide();
        });

        eventTrigger.triggers.Add(enterEvent);
        eventTrigger.triggers.Add(exitEvent);
        #endregion
    }

    /// <summary>
    /// 属性绑定
    /// </summary>
    protected void BindAttribute() {        
        characterMono.HeroModel.ExpChangedHandler += OnExpChanged;      // 经验
        characterMono.HeroModel.LevelChangedHandler += OnLevelChanged;  // 等级
        characterMono.HeroModel.ForcePowerHandler += OnForcePowerChanged;   // 力量
        characterMono.HeroModel.AgilePowerHandler += OnAgilePowerChanged;   // 敏捷
        characterMono.HeroModel.IntelligencePowerHandler += OnIntelligencePowerChanged; // 智力
    }
    
    public void OnAvatarImageChanged(string oldImagePath,string newImagePath) {
        AvatarImage.texture = Resources.Load<Texture>("UIImage/" + newImagePath);
    }
    public void OnNameChanged(string oldName, string newName) {
        //Debug.Log("姓名改版");
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
    public void OnMoveSpeedChanged(float oldMoveSpeed, float newMoveSpeed) {
        moveSpeedText.text = newMoveSpeed.ToString();
    }
    public void OnForcePowerChanged(float oldForcePower, float newForcePower) {
        // 显示时将其取整运算,直接舍去小数部分
        forcePowerText.text = ((int)newForcePower).ToString();
    }
    public void OnAgilePowerChanged(float oldAgilePower, float newAgilePower) {
        agilePowerText.text = ((int)newAgilePower).ToString();
    }
    public void OnIntelligencePowerChanged(float oldIntelligencePower, float newIntelligencePower) {
        intelligencePowerText.text = ((int)newIntelligencePower).ToString();
    }
    public void OnExpChanged(int oldExp, int newExp) {

        // 经验比率取整
        int expRate = Mathf.Clamp(Mathf.FloorToInt(((float)newExp / characterMono.HeroModel.NextLevelNeedExp) * 100), 0, 100);

        expText.text = string.Format("EXP: {0}%",expRate);
        expImage.sizeDelta = new Vector2(expImage.sizeDelta.x, expRate);
    }

}

