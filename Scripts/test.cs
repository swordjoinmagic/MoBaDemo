using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        Camera.main.transform.Rotate(new Vector3(0,h,0)*Time.deltaTime*5);
        
	}
}
