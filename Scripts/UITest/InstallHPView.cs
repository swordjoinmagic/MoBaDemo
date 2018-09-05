using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class InstallHPView : MonoBehaviour{

    public HPView HPView;

    private void Start() {
        CharacterModel characterModel = new CharacterModel { Hp=600,maxHp=600,maxMp=100,Mp=100 };
        HPView.BindingContext = new HPViewModel();
        HPView.BindingContext.Init(characterModel);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            HPView.BindingContext.Hp.Value -= 10;
        }
    }
}
