using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFrameRate : MonoBehaviour {

    private Text text;

    float time = 0f;
    int frameCount = 0;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = string.Format("帧率：{0:00.0} FPS", 1/Time.deltaTime);

        //time += Time.deltaTime;
        //frameCount += 1;
        //if (time >= 1f) {
        //    text.text = string.Format("帧率：{0} FPS",frameCount);
        //    time = 0f;
        //    frameCount = 0;
        //}
	}
}
