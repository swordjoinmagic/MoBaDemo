using uMVVM;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
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

    // 人物属性信息提示窗口prefab
    public CharacterAttributeTipsView characterAttributeTipsViewPrefab;

    private EventTrigger eventTrigger;
    private CharacterAttributeTipsView TipsViewInstance;        // 提示信息窗口实例
    private GameObject canvas;
    private RectTransform rectTransform;
    private Camera UICamera;
    public HeroMono characterMono;

    private void Start() {
        canvas = GameObject.Find("Canvas");
        rectTransform = transform as RectTransform;
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        eventTrigger = GetComponent<EventTrigger>();
    }

    protected override void OnInitialize() {
        base.OnInitialize();

        //==============================================================
        // 属性绑定
        binder.Add<string>("Name",OnNameChanged);
        binder.Add<int>("Attack",OnAttackChanged);
        binder.Add<int>("Defense",OnDefenseTextChanged);
        binder.Add<float>("MoveSpeed", OnMoveSpeedChanged);
        binder.Add<int>("ForcePower", OnForcePowerChanged);
        binder.Add<int>("AgilePower", OnAgilePowerChanged);
        binder.Add<int>("IntelligencePower", OnIntelligencePowerChanged);
        binder.Add<int>("Level",OnLevelChanged);
        binder.Add<int>("ExpRate",OnExpTextChanged);
        binder.Add<string>("AvatarImagePath", OnAvatarImageChanged);


        //===============================================================
        // 设置鼠标进入、离开事件
        //if (eventTrigger==null) eventTrigger = GetComponent<EventTrigger>();
        //if (canvas == null) canvas = GameObject.Find("Canvas");
        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener(eventData=> {
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
                anchorsPos.x + rectTransform.sizeDelta.x/2,
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
        //Debug.Log("newExpRate:"+newExpRate);
        expImage.sizeDelta = new Vector2(expImage.sizeDelta.x,newExpRate);
    }
}

