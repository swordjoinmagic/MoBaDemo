using UnityEngine;

public class NetWorkTest1 : MonoBehaviour{

    public CharacterMono characterMono;

    private void Update() {
        if (NetWorkManager.Instance.networkPlayers.Count == 0) return;
        if (characterMono == null)
            characterMono = NetWorkManager.Instance.networkPlayers[NetWorkManager.Instance.NowPlayerID].GetComponent<CharacterMono>();
        if (Input.GetMouseButtonDown(0)) {
            characterMono.characterModel.Level += 1;
        }
    }
}

