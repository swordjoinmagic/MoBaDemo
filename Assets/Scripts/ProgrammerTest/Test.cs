using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using LitJson;

class JSONTest {
    public string name;
    public int level;
}

public class Test : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        Debug.Log("(typeof(PoisoningState)==typeof(BattleState)):" + (typeof(PointingSkill).BaseType));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
