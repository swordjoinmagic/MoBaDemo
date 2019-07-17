using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerMoney : MonoBehaviour {

    public Text text;
    private HeroMono heroMono;

    public void Init(HeroMono heroMono) {
        this.heroMono = heroMono;
    }

	// Update is called once per frame
	void Update () {
        if (heroMono == null) return;
        text.text = heroMono.Owner.Money.ToString();
	}
}
