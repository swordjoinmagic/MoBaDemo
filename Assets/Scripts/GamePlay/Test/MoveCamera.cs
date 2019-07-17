using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 移动摄像机
/// </summary>
class MoveCamera : MonoBehaviour{

    public float rate = 0.9f;
    public float moveSpeed = 1f;
    public Vector3 moveX = Vector3.right;
    public Vector3 moveZ = Vector3.forward;

    private void Update() {
        Vector2 mousePosition = Input.mousePosition;
        int height = Camera.main.pixelHeight;
        int width = Camera.main.pixelWidth;
        if (mousePosition.x > width * rate && mousePosition.x <= width) {
            // 右
            Camera.main.transform.Translate(moveX*moveSpeed*Time.deltaTime, Space.World);
        }
        if (mousePosition.x < width * (1 - rate) && mousePosition.x >= 0) {
            // 左
            Camera.main.transform.Translate(-moveX * moveSpeed * Time.deltaTime, Space.World);
        }
        if (mousePosition.y > height * rate && mousePosition.y <= height) {
            // 上
            Debug.Log("上："+ (moveZ * moveSpeed * Time.deltaTime));
            Camera.main.transform.Translate(moveZ * moveSpeed * Time.deltaTime,Space.World);
        }
        if (mousePosition.y < height * (1 - rate) && mousePosition.y >= 0) {
            // 下
            Camera.main.transform.Translate(-moveZ * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}

