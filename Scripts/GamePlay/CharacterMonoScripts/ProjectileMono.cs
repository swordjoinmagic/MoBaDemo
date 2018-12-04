using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 投射物的MonoBehavior类
/// </summary>
public class ProjectileMono : MonoBehaviour{
    public ProjectileModel projectileModel;

    // 投射物要攻击的目标
    public CharacterMono target;

    // 该投射物的目标位置
    public Vector3 targetPosition;

    // 该投射物造成的伤害
    public Damage damage = Damage.Zero;


    /// <summary>
    /// 进行移动的函数,当移动到目标点的时候,返回true
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool Move(Vector3 position) {

        // 获得从当前点指向目标地点的单位矢量
        Vector3 dir = (-transform.position + position).normalized;

        // 一次移动的距离
        Vector3 distance = dir * Time.deltaTime * projectileModel.MovingSpeed;

        transform.Translate(distance); 

        // 两物体相距距离
        float different = Vector3.Distance(transform.position,position);
        if (different < distance.magnitude) {
            return true;
        } else {
            return false;
        }
    }

    private void Update() {
        if (projectileModel==null) return;

        // 如果targetEnermyTransform为Null,则表示该投射物为指定一个地点进行投射,
        // 当到达目标地点时,计算伤害,这种投射物一般都是范围伤害
        // 如果移动结束,执行伤害,生命周期结束
        if (Move(targetPosition)) {
            Execute();

            Destroy(gameObject);
        }
    }

    // 产生特效并执行伤害
    public void Execute() {
        GameObject targetEnemryEffect;
        GameObject targetPositionEffect;
        if (projectileModel.spherInfluence == 0) {
            // 只影响单人的投射物

            // 产生敌人身上的特效
            if (projectileModel.tartgetEnemryEffect != null) {
                targetEnemryEffect = Instantiate(projectileModel.tartgetEnemryEffect, targetPosition,Quaternion.identity);
            }

            // 产生目标位置的特效
            if (projectileModel.targetPositionEffect != null) {
                targetPositionEffect = Instantiate(projectileModel.targetPositionEffect, transform);
            }

            target.characterModel.Damaged(damage);
        } else {
            // 影响一个范围的投射物

            // 产生技能特效

            // 产生目标位置的特效
            if (projectileModel.targetPositionEffect != null) {
                targetPositionEffect = Instantiate(projectileModel.targetPositionEffect, transform.position,Quaternion.identity);
            }

            Collider[] colliders = Physics.OverlapSphere(targetPosition, projectileModel.spherInfluence);
            foreach (Collider collider in colliders) {
                CharacterMono targetMono = collider.GetComponent<CharacterMono>();
                if (targetMono == null) continue;
                // 产生敌人身上的特效
                if (projectileModel.tartgetEnemryEffect != null) {
                    targetEnemryEffect = Instantiate(projectileModel.tartgetEnemryEffect, collider.transform.position,Quaternion.identity);
                }

                targetMono.characterModel.Damaged(damage);
            }
        }
    }
}

