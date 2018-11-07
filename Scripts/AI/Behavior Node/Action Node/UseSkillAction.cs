using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UseSkillAction : Action{

    public SharedGameObject enermy;
    public GameObject enermyParticleSystem;

    private CharacterMono enermyCharacterMono;
    private CharacterMono characterMono;
    private PointingSkill pointingSkill;
    private Animator animator;

    public override void OnStart() {
        animator = GetComponent<Animator>();
        characterMono = GetComponent<CharacterMono>();
        enermyCharacterMono = enermy.Value.GetComponent<CharacterMono>();
        if (characterMono.prepareSkill != null) {
            Debug.Log("prepareSkill:"+ characterMono.prepareSkill);
            pointingSkill = characterMono.prepareSkill as PointingSkill;
            Debug.Log("pointingSkill:"+pointingSkill);
            //pointingSkill.target = enermy.Value;
            pointingSkill.TargetEffect = enermyParticleSystem;
        }
    }

    public override TaskStatus OnUpdate() {
        animator.SetTrigger("attack");
        Damage damage = pointingSkill.CalculateDamage();
        CharacterModel enermyCharacter = enermyCharacterMono.characterModel;
        enermyCharacter.Hp -= damage.TotalDamage;
        enermyCharacterMono.SimpleCharacterViewModel.Modify(enermyCharacter);
        return TaskStatus.Success;
    }
}

