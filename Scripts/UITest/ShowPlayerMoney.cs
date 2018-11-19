using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerMoney : MonoBehaviour {

    public Text text;
    public HeroMono heroMono;

	// Update is called once per frame
	void Update () {
        text.text = heroMono.Owner.Money.ToString();
	}
}
