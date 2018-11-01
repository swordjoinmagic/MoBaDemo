using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Type type = Type.GetType("PointingSkill");
        PointingSkill pointingSkill = type.Assembly.CreateInstance("PointingSkill") as PointingSkill;
        Debug.Log("反射测试："+pointingSkill.GetType());
        pointingSkill.Mp = 100;
        pointingSkill.BaseDamage = 100;
        Debug.Log("反射测试：SkillMP:"+pointingSkill.Mp);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
