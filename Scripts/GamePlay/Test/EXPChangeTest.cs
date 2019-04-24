using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EXPChangeTest : MonoBehaviour{

    HeroMono hero;

    private void Start() {
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (hero == null) hero = GameObject.Find("sjm").GetComponent<HeroMono>();
            hero.HeroModel.Exp += 6666;
        }
    }
}

