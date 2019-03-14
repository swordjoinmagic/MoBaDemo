using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 警告UI，当玩家进行某些操作但是失效时（如在mp不足时使用魔法
/// 在金钱不足时购买物品），弹出警告窗口，
/// 告诉玩家操作失败的原因
/// 
/// 对各类事件进行监听
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class WarningView : MonoBehaviour {

    // 警告文字
    public Text warnningText;

    private string warnningString;

    private CanvasGroup canvasGroup;

    public string WarnningString {
        get {
            return warnningString;
        }

        set {
            warnningString = value;
            if (warnningText != null) warnningText.text = value;
        }
    }

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();

        // 监听各类事件
        Bind();

        gameObject.SetActive(false);
    }

    private Tweener Show() {
        gameObject.SetActive(true);
        return canvasGroup.DOFade(1,1f);        
    }

    private void Hide() {
        canvasGroup.DOFade(0,0.5f).onComplete += ()=> { gameObject.SetActive(false); };
    }

    public void Bind() {

        // 监听 当玩家购买商店物品 事件
        MessageAggregator.Instance.AddListener<Player, ItemGrid>(EventType.OnPlayerPrepareBuyStoreItem,OnPlayerPrepareBuyStoreItem);

        // 监听 准备释放技能 事件
        MessageAggregator.Instance.AddListener<CharacterMono,ActiveSkill>(EventType.OnPlayerPrepareSell,OnPlyaerPrepareSpell);
    }

    /// <summary>
    /// 监听 当玩家购买商店物品 事件
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemGrid"></param>
    private void OnPlayerPrepareBuyStoreItem(Player player, ItemGrid itemGrid) {
        if (itemGrid != null && itemGrid.item != null) {
            int price = itemGrid.item.price;
            if (price > player.Money) {
                if(!isActiveAndEnabled)
                    Show().onComplete += Hide;
                WarnningString = "玩家的金钱不够!";
            }
        }
    }

    /// <summary>
    /// 监听 准备释放技能 事件
    /// </summary>
    /// <param name="characterMono"></param>
    /// <param name="activeSkill"></param>
    private void OnPlyaerPrepareSpell(CharacterMono characterMono,ActiveSkill activeSkill) {
        if (characterMono.characterModel.Mp < activeSkill.Mp) {
            if (!isActiveAndEnabled)
                Show().onComplete += Hide;
            WarnningString = "当前单位Mp值不足!";
        }
    }
}

