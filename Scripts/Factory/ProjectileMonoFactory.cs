using UnityEngine;

/// <summary>
/// 生成投射物的工厂
/// </summary>
public class ProjectileMonoFactory {
    // 获得一个临时GameObject对象,并为其添加ProjectileMono脚本管理其生命周期

    public static ProjectileMono AcquireObject(CharacterMono launcher, CharacterMono target, Vector3 shootPosition, Damage damage,GameObject templateObject = null) {
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
        projectile.damage = damage;
        projectile.transform.position = shootPosition;
        projectile.projectileModel = launcher.characterModel.projectileModel;

        return projectile;
    }
}

