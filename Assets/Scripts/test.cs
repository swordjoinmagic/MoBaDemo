
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.EventSystems;

public class test : MonoBehaviour {

    public Texture2D texture;

	// Use this for initialization
	void Start () {

    }

    private void Update() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            Debug.Log("当前触摸在UI上");
        } else {
            Debug.Log("当前没有触摸在UI上");
        }
    }
}
 