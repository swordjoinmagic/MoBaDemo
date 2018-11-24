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
        string content = (Resources.Load("Data\\data") as TextAsset).text;
        text.text = content;
        //Debug.Log(content);
        string s = JsonMapper.ToJson(new JSONTest { name="test",level=1});
        //Debug.Log(s);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
