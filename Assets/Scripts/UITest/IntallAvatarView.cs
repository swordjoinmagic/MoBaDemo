using UnityEngine;

public class IntallAvatarView : MonoBehaviour{

    public AvatarView avatarView;
    public HeroMono character;

    private void Start() {
        character = GameObject.Find("sjm").GetComponent<HeroMono>();
    }
    private void Update() {
        if (Input.GetKey(KeyCode.X)) {
            character.HeroModel.Exp += 10;
            Debug.Log("经验值+10，现在经验是："+ character.HeroModel.Exp); 
        }
    }
}

