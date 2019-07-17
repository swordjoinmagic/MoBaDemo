using UnityEngine;

/// <summary>
/// 生成投射物的工厂
/// </summary>
public class ProjectileMonoFactory {
    // 获得一个临时GameObject对象,并为其添加ProjectileMono脚本管理其生命周期

    /// <summary>
    /// 
    /// </summary>
    /// <param name="launcher">发射投射物的单位</param>
    /// <param name="target">投射物的目标单位</param>
    /// <param name="shootPosition">发射的位置</param>
    /// <param name="damage">投射物此次造成的伤害</param>
    /// <param name="templateObject">投射物模板单位</param>
    /// <returns></returns>
    public static ProjectileMono AcquireObject(CharacterMono launcher, CharacterMono target, Vector3 shootPosition,GameObject templateObject = null) {
        GameObject result = null;

        if (templateObject == null) {
            result = new GameObject();
        } else {
            result = GameObject.Instantiate(templateObject);
        }

        ProjectileMono projectile = result.AddComponent<ProjectileMono>();
        projectile.targetPosition = target.transform.position;
        projectile.target = target;
        projectile.launcher = launcher;
        projectile.DamageFunc += launcher.characterModel.GetDamage;
        projectile.transform.position = shootPosition;
        projectile.projectileModel = launcher.characterModel.projectileModel;

        return projectile;
    }
}

