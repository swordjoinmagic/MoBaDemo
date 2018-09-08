using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CharacterMono : MonoBehaviour{
    public CharacterModel characterModel;
    public int Hp;
    public int Mp;
    public int maxHp;
    public int maxMp;
    public string characterName;

    private void Start() {
        characterModel = new CharacterModel();
        characterModel.Hp = Hp;
        characterModel.maxHp = maxHp;
        characterModel.Mp = Mp;
        characterModel.maxMp = maxMp;
        characterModel.name = characterName;
    }

}

