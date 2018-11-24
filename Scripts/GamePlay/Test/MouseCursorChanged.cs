using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorChanged : MonoBehaviour {

    private CharacterMono characterMono;
    private Projector skillCircleInflence;

    public Projector skillCircleInflencePrefabs;


    public void Start() {
        characterMono = GameObject.FindWithTag("Player").GetComponent<CharacterMono>();
    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        RaycastHit hit2;
        MouseIconManager.Instace.Recovery();
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Enermy")) {
                MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Attack);
            }
        }
        if (characterMono.isPrepareUseSkill) {
            MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Spell);

            ActiveSkill activeSkill = characterMono.prepareSkill;
            if (activeSkill.SkillInfluenceRadius > 0) {
                if (Physics.Raycast(ray, out hit2, 100,layerMask:1<<11)) {
                    Vector3 position = hit2.point;
                    Debug.Log(position);
                    position.y = 3f;
                    if (skillCircleInflence == null) {
                        skillCircleInflence = GameObject.Instantiate<Projector>(skillCircleInflencePrefabs, position, skillCircleInflencePrefabs.transform.rotation);
                    } else {
                        skillCircleInflence.gameObject.SetActive(true);
                        skillCircleInflence.transform.position = position;
                    }
                    skillCircleInflence.orthographicSize = activeSkill.SkillInfluenceRadius;

                }
            }
        } else {
            if (skillCircleInflence != null) {
                skillCircleInflence.gameObject.SetActive(false);
            }
        }
    }
}
