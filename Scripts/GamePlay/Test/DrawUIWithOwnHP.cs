using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 给摄像机内所有拥有HP属性的单位画UI
/// </summary>
public class DrawUIWithOwnHP : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
        Vector3 position = Camera.main.WorldToScreenPoint(target.position);
        print("position:"+position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
