using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorChanged : MonoBehaviour {

    private CharacterMono characterMono;
    private Projector skillCircleInflence;
    private OutLinePostEffect outLinePostEffect;

    public Projector skillCircleInflencePrefabs;


    public void Init() {
        characterMono = GameObject.FindWithTag("Player").GetComponent<CharacterMono>();
        outLinePostEffect = Camera.main.GetComponent<OutLinePostEffect>();
    }

    // Update is called once per frame
    void Update () {
        if (characterMono == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        RaycastHit hit2;
        MouseIconManager.Instace.Recovery();
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Enermy")) {
                MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Attack);

                // 为目标单位添加一个泛光描边
                outLinePostEffect.targetObject = hit.collider.gameObject;
                outLinePostEffect.outLineColor = Color.red;
                outLinePostEffect.enabled = true;
            } else {
                if(outLinePostEffect.isActiveAndEnabled)
                    outLinePostEffect.enabled = false;
            }
        }
        if (characterMono.isPrepareUseSkill) {
            MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Spell);

            ActiveSkill activeSkill = characterMono.prepareSkill;
            if (activeSkill.SkillInfluenceRadius > 0 && !activeSkill.IsMustDesignation) {
                if (Physics.Raycast(ray, out hit2, 100,layerMask:1<<11)) {
                    Vector3 position = hit2.point;
                    //Debug.Log(position);
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
