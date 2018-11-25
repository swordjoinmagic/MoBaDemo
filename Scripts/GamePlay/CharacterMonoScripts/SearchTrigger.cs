using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SearchTrigger : MonoBehaviour{

    public List<CharacterMono> arroundEnemies = new List<CharacterMono>();

    public List<CharacterMono> arroundFriends = new List<CharacterMono>();


    private CharacterMono characterMono;

    public void Awake() {
        characterMono = transform.parent.GetComponent<CharacterMono>();
    }

    //======================================
    // ●对周围的敌人数量进行计数
    private void OnTriggerEnter(Collider other) {
        CharacterMono target = other.gameObject.GetComponent<CharacterMono>();
        // 当目标碰撞体是"普通单位"且与自己不属于同个阵营时,敌人数加1
        if (target != null && target.IsCanBeAttack()) {
            if (!characterMono.CompareOwner(target))
                arroundEnemies.Add(other.GetComponent<CharacterMono>());
            else
                arroundFriends.Add(other.GetComponent<CharacterMono>());
        }
    }

    private void OnTriggerExit(Collider other) {
        CharacterMono target = other.gameObject.GetComponent<CharacterMono>();
        if (target != null) {
            if(!characterMono.CompareOwner(target))
                arroundEnemies.Remove(other.GetComponent<CharacterMono>());
            else
                arroundFriends.Add(other.GetComponent<CharacterMono>());

        }
    }
}

