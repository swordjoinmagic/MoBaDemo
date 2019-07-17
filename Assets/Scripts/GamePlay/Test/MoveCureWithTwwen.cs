using UnityEngine;
using DG.Tweening;

class MoveCureWithTwwen : MonoBehaviour{
    // 要达到的目标位置
    public Vector3 targetPosition;
    // 运动速率,单位m/s
    public float speed = 1;
    // 上升高度
    public float riseHeight = 5;
    // 开始的角度(基于水平面计算)
    public float angle = 30;
    // 结束时的角度(基于水平面)
    public float endAngle = 130;
    // 表示此次运动是否为弧线运动
    public bool isArcMotion = true;

    private void Start() {
        // 距离是以平方计算的,那么速度也以平方算
        speed *= speed;

        // 两者中间的位置
        Vector3 centerPosition = (transform.position + targetPosition) / 2;
        centerPosition.y += riseHeight;

        // 获得到达目标地点所需的时间
        float needTime = Vector3Util.DistanceXYSqure(transform.position,targetPosition) / speed;
        
        if (isArcMotion) {
            // 进行弧线运动

            // 当前单位指向目标地点
            transform.LookAt(targetPosition);

            // 设置初始角度
            transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);

            // 获得从当前位置到达中间位置所需时间
            float needMiddleTime = Vector3Util.DistanceXYSqure(transform.position, centerPosition) / speed;

            transform.DOLocalPath(new Vector3[] { transform.position, centerPosition, targetPosition }, needTime, PathType.CatmullRom, PathMode.Full3D, 10, Color.red);

            // 到达中点时,角度应该为90
            transform.DORotate(new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z), needMiddleTime).onComplete += () => {
                transform.DORotate(new Vector3(endAngle, transform.eulerAngles.y, transform.eulerAngles.z), needTime - needMiddleTime);
            };
        } else {
            // 进行直线运动
            transform.DOLocalPath(new Vector3[] { transform.position, targetPosition }, needTime, PathType.Linear, PathMode.Full3D, 10, Color.red);
        }
        
    }
}

