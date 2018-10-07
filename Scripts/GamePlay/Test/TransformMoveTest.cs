using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TransformMoveTest : MonoBehaviour{
    public Transform target;

    public float speed;

    private void Update() {
        if (Vector3.Distance(transform.position,target.position) > 0) {

            // 获得从当前点指向目标地点的单位矢量
            Vector3 dir = (-transform.position + target.position).normalized;

            transform.Translate(dir * Time.deltaTime * speed);

        }
    }
}

