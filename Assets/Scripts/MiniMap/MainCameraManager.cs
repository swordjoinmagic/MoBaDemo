using DG.Tweening;
using UnityEngine;

/// <summary>
/// 用来负责管理摄像机移动的Mono类,
/// 当摄像机移动时,触发摄像机移动的事件,从而使小地图游标移动
/// </summary>
public class MainCameraManager : MonoBehaviour{

    public delegate void CameraMoveHandler(Vector3 oldPosition,Vector3 newPosition);
    public event CameraMoveHandler OnCameraMove;

    public float rate = 0.9f;

    // 摄像机移动速度
    public float moveSpeed = 1f;

    // 水平一次移动的间隔
    public Vector3 moveX = Vector3.right;

    // 竖直方向一次移动的间隔
    public Vector3 moveZ = Vector3.forward;

    private void Update() {
        MoveCameraByInput();
    }

    public void MoveCameraByInput() {
        Vector2 mousePosition = Input.mousePosition;

        int height = Camera.main.pixelHeight;
        int width = Camera.main.pixelWidth;

        if (mousePosition.x > width * rate && mousePosition.x <= width) {
            // 右
            TranslateCamera(moveX * moveSpeed * Time.deltaTime);
        }
        if (mousePosition.x < width * (1 - rate) && mousePosition.x >= 0) {
            // 左
            TranslateCamera(-moveX * moveSpeed * Time.deltaTime);
        }
        if (mousePosition.y > height * rate && mousePosition.y <= height) {
            // 上
            TranslateCamera(moveZ * moveSpeed * Time.deltaTime);
        }
        if (mousePosition.y < height * (1 - rate) && mousePosition.y >= 0) {
            // 下
            TranslateCamera(-moveZ * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>d 
    /// 让摄像机向目标位置移动
    /// </summary>
    /// <param name="endVector3"></param>
    public void MoveCamera(Vector3 endVector3) {

        // 获得摄像机当前位置
        Vector3 currentPosition = Camera.main.transform.position;

        // 设置摄像机新位置
        Camera.main.transform.position = endVector3;

        // 触发摄像机移动事件
        if (OnCameraMove != null) OnCameraMove(currentPosition, endVector3);        
    }

    /// <summary>
    /// 让摄像机向目标方向移动
    /// </summary>
    /// <param name="dir"></param>
    public void TranslateCamera(Vector3 dir) {

        // 获得摄像机当前位置
        Vector3 currentPosition = Camera.main.transform.position;

        // 移动摄像机
        Camera.main.transform.Translate(dir, Space.World);

        // 触发摄像机移动事件
        if (OnCameraMove != null) OnCameraMove(currentPosition, Camera.main.transform.position);
    }
}

