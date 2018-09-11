using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour {

    public Transform target;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A)) {
            //transform.rotation = Quaternion.LookRotation(target.position);/*Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(target.position),Time.deltaTime*5);*/
            transform.LookAt(target.position,transform.up);
        }
	}
}
