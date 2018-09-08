using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CharacterMono : MonoBehaviour {

    private SimpleCharacterViewModel simpleCharacterViewModel;

    public CharacterModel characterModel;
    public int Hp;
    public int Mp;
    public int maxHp;
    public int maxMp;
    public string characterName;

    private bool isStart = false;

    public SimpleCharacterViewModel SimpleCharacterViewModel {
        get {
            return simpleCharacterViewModel;
        }

        set {
            simpleCharacterViewModel = value;
        }
    }

    private void Start() {
        isStart = true;
        characterModel = new CharacterModel {
            Hp = Hp,
            maxHp = maxHp,
            Mp = Mp,
            maxMp = maxMp,
            name = characterName
        };
    }
}

