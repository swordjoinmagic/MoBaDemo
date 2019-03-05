using UnityEngine;

static class Vector3Util {
    /// <summary>
    /// 获得a向量和b向量的距离的平方(只在xy分量上)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float DistanceXYSqure(Vector3 a,Vector3 b) {
        return (a.x - b.x)* (a.x - b.x) + (a.z - b.z)* (a.z - b.z);
    }
}

