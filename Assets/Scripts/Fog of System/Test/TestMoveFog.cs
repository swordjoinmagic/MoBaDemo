using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveFog : MonoBehaviour {

    public float speed = 1;
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h,0, v) * Time.deltaTime * speed);
    }
}
