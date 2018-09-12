using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorChanged : MonoBehaviour {

    private CharacterMono characterMono;

    public void Start() {
        characterMono = GameObject.FindWithTag("Player").GetComponent<CharacterMono>();
    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        MouseIconManager.Instace.Recovery();
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Enermy")) {
                MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Attack);
            }
        }
        if (characterMono.isPrepareUseSkill) {
            MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Spell);
        }
    }
}
