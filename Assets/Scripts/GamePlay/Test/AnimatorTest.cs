using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        animator.SetTrigger("attack");
	}
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("attack") && nextAnimatorStateInfo.IsName("Idle")) {
            print("一次攻击操作已完成！");
        }
	}
}
