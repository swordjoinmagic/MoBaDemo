using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour {

    // 死亡时间,经过这个时间后,该事物消失
    public float deadTime = 0.4f;

    public float nowTime = 0;

    public void Update() {
        nowTime += Time.deltaTime;
        if (nowTime > deadTime) {
            Destroy(gameObject);
        }
    }
}
