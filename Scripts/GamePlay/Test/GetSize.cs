using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获得单位的尺寸
/// </summary>
public class GetSize : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
        Vector3 vector3 = target.GetComponent<MeshFilter>().mesh.bounds.size;
        print("size:"+ vector3);
	}
	
}
