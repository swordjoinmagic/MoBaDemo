using System;
using UnityEngine;
using DG.Tweening;

class MoveCurve : MonoBehaviour{
    // 抛物线目标位置
    public Transform target;    

    // 物体初始角度（以水平面作为基准）
    [Range(0,70)]
    public float angle;


    // 抛物线水平初速度
    private float horizontalSpeed;
    // 抛物线垂直初速度
    private float verticalSpeed;
    // 整个抛物过程一共持续多久
    private float duration;
    // 目标位置与当前位置的距离
    private float distance;
    // 对着目标的方向（单位向量）
    private Vector3 direction;
    // 角速度,只改变物体的z分量，即弓箭倾倒程度，
    // 物体旋转y分量决定其面朝的方向
    private float angleSpeed;

    private float tempVerticalSpeed;
    private float tempAngle;
    private float g = 10;

    private void Start() {

        distance = Vector3.Distance(transform.position,target.position);

        // 求出水平初速度
        horizontalSpeed = Mathf.Abs(Mathf.Sqrt(5 * distance / Mathf.Tan(angle * Mathf.Deg2Rad)));
        // 求出垂直初速度
        verticalSpeed = horizontalSpeed * Mathf.Tan(angle*Mathf.Deg2Rad);
        // 求出整个过程的持续时间
        duration = distance / horizontalSpeed;

        direction = Vector3.Normalize(-transform.position + target.position);

        // 求出上升所用时间
        float riseTime = verticalSpeed / g ;

        tempVerticalSpeed = verticalSpeed;
        angleSpeed = (90-angle) / riseTime;

        // 总而言之,先让物体面朝那个方向
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(angle,transform.eulerAngles.y,transform.eulerAngles.z);
        tempAngle = angle;

    }

    private void Update() {
        //if ((transform.position - target.position).magnitude < 0.5) {
        //    return;
        //}
        Move(1);
    }


    private void Move(float speed) {
        tempVerticalSpeed -= g * Time.deltaTime * speed;

        transform.Rotate(angleSpeed * Time.deltaTime * speed, 0, 0);

        Vector3 vector3 = direction * horizontalSpeed * Time.deltaTime + tempVerticalSpeed * Time.deltaTime * Vector3.up;
        vector3 *= speed;

        transform.Translate(vector3,Space.World);
        
        //// 水平移动
        //transform.Translate(direction * horizontalSpeed * Time.deltaTime, Space.World);
        //// 垂直移动
        //transform.Translate(tempVerticalSpeed * Time.deltaTime * Vector3.up, Space.World);
    }
}

