using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class InstallHPView : MonoBehaviour{

    public HPAndMPDetailView HPView;
    public HeroMono CharacterMono;

    private void Start() {
        HPView.characterMono = CharacterMono;

        
    }


}
