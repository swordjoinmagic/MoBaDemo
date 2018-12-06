using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class InstallHPView : MonoBehaviour{

    public HPView HPView;
    public HeroMono CharacterMono;

    private void Start() {
        HPView.characterMono = CharacterMono;
        HPView.BindingContext = new HPViewModel();
        HPView.BindingContext.Init(CharacterMono.characterModel);
        
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            HPView.BindingContext.Hp.Value -= 10;
        }
    }
}
