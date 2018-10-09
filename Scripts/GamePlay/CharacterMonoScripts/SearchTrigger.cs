using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SearchTrigger : MonoBehaviour{

    public List<CharacterMono> arroundEnemies = new List<CharacterMono>();

    //======================================
    // ●对周围的敌人数量进行计数
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enermy")) {
            arroundEnemies.Add(other.GetComponent<CharacterMono>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enermy")) {
            arroundEnemies.Remove(other.GetComponent<CharacterMono>());
        }
    }
}

