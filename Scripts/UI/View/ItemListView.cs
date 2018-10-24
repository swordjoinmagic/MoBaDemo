using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 物品栏视图
/// </summary>
public class ItemListView : MonoBehaviour{

    // 和单位的ItemGrids数组一一对应
    public ItemView[] itemViews = new ItemView[6];
    public Image[] maskImage;

    public CharacterMono characterMono;
    private List<ItemGrid> itemGrids;

    private void Start() {
        itemGrids = characterMono.characterModel.itemGrids;
        for (int i=0;i<6;i++) {
            itemViews[i].BindingContext = new ItemViewModel();
            itemViews[i].BindingContext.Init(itemGrids[i]);
            characterMono.ItemViewModels.Add(itemViews[i].BindingContext);
        }
    }

    private void Update() {
        for (int i= 0;i<6;i++) {

            ItemGrid itemGrid = itemGrids[i];

            Item item = itemGrid.item;

            if (item != null) {
                ActiveSkill activeSkill = item.itemActiveSkill;
                if (activeSkill == null) return;

                float coolDown = activeSkill.CD;
                float finalSpellTime = activeSkill.FinalSpellTime;

                float different = Time.time - finalSpellTime;

                float rate = 1 - Mathf.Clamp01(different / coolDown);

                maskImage[i].fillAmount = rate;
            }
        }
    }
}

