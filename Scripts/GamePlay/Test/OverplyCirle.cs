using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverplyCirle : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            Collider[] cols = Physics.OverlapSphere(new Vector3(0, 0, 0), 5);
            print("创建的碰撞体集合长度为：" + cols.Length);
            foreach (Collider collider in cols) {
                print("碰撞体为：" + collider.gameObject.name);
            }
        }
        
	}
}
