using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 投射物的MonoBehavior类
/// </summary>
public class ProjectileMono : MonoBehaviour{
    public ProjectileModel projectileModel;

    // 投射物要攻击的目标
    public CharacterMono target;

    // 发射投射物的单位
    public CharacterMono launcher;

    // 该投射物的目标位置
    public Vector3 targetPosition;

    // 该投射物造成的伤害的方法,是一个委托,当投射物到达目标地点后,对敌方单位执行该方法
    public Func<CharacterModel, Damage> DamageFunc;

    private void Start() {
        // 距离是以平方计算的,那么速度也以平方算
        float speed = projectileModel.movingSpeed * projectileModel.movingSpeed;

        // 两者中间的位置
        Vector3 centerPosition = (transform.position + targetPosition) / 2;
        centerPosition.y += projectileModel.riseHeight;

        // 获得到达目标地点所需的时间
        float needTime = Vector3Util.DistanceXYSqure(transform.position, targetPosition) / speed;

        if (projectileModel.isArcMotion) {
            // 进行弧线运动

            // 当前单位指向目标地点
            transform.LookAt(targetPosition);

            // 设置初始角度
            transform.eulerAngles = new Vector3(projectileModel.angle, transform.eulerAngles.y, transform.eulerAngles.z);

            // 获得从当前位置到达中间位置所需时间
            float needMiddleTime = Vector3Util.DistanceXYSqure(transform.position, centerPosition) / speed;

            var dg = transform.DOLocalPath(new Vector3[] { transform.position, centerPosition, targetPosition }, needTime, PathType.CatmullRom, PathMode.Full3D, 10, Color.red);

            // 到达中点时,角度应该为90
            transform.DORotate(new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z), needMiddleTime).onComplete += () => {
                transform.DORotate(new Vector3(projectileModel.endAngle, transform.eulerAngles.y, transform.eulerAngles.z), needTime - needMiddleTime);
            };

            // 到达目标地点后,销毁当前投射物,并执行伤害
            dg.onComplete += () => {
                Execute();
                Destroy(gameObject);
            };
        } else {
            // 进行直线运动
            var dg = transform.DOLocalPath(new Vector3[] { transform.position, targetPosition }, needTime, PathType.Linear, PathMode.Full3D, 10, Color.red);

            // 到达目标地点后,销毁当前投射物,并执行伤害
            dg.onComplete += () => {
                Execute();
                Destroy(gameObject);
            };
        }
    }

    #region 废弃方案
    /// <summary>
    /// 进行移动的函数,当移动到目标点的时候,返回true
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    //private bool Move(Vector3 position) {
    //    // 获得从当前点指向目标地点的单位矢量
    //    Vector3 dir = (-transform.position + position).normalized;

    //    // 一次移动的距离
    //    Vector3 distance = dir * Time.deltaTime * projectileModel.movingSpeed;

    //    transform.Translate(distance); 

    //    // 两物体相距距离
    //    float different = Vector3.Distance(transform.position,position);
    //    if (different < distance.magnitude) {
    //        return true;
    //    } else {
    //        return false;
    //    }
    //}

    //private void Update() {
    //    if (projectileModel==null) return;

    //    // 如果targetEnermyTransform为Null,则表示该投射物为指定一个地点进行投射,
    //    // 当到达目标地点时,计算伤害,这种投射物一般都是范围伤害
    //    // 如果移动结束,执行伤害,生命周期结束
    //    if (Move(targetPosition)) {
    //        Execute();

    //        Destroy(gameObject);
    //    }
    //}
    #endregion

    // 产生特效并执行伤害
    public void Execute() {
        if (projectileModel.spherInfluence == 0) {
            // 只影响单人的投射物

            // 产生敌人身上的特效
            if (projectileModel.tartgetEnemryEffect != null) {
                EffectsLifeCycle lifeCycle = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During,templateObject: projectileModel.tartgetEnemryEffect,during:2f);
                lifeCycle.transform.SetParent(target.transform);
                lifeCycle.transform.localPosition = Vector3.zero;
            }

            // 产生目标位置的特效
            if (projectileModel.targetPositionEffect != null) {
                EffectsLifeCycle lifeCycle = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During, templateObject: projectileModel.targetPositionEffect, during: 2f);
                lifeCycle.transform.SetParent(target.transform);
                lifeCycle.transform.localPosition = Vector3.zero;
            }

            // 计算对目标单位的伤害
            Damage damage = DamageFunc(target.characterModel);
            target.characterModel.Damaged(target, damage, launcher);
        } else {
            // 影响一个范围的投射物

            // 产生技能特效

            // 产生目标位置的特效
            if (projectileModel.targetPositionEffect != null) {
                EffectsLifeCycle lifeCycle = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During, templateObject: projectileModel.targetPositionEffect, during: 2f);
                lifeCycle.transform.SetParent(target.transform);
                lifeCycle.transform.localPosition = Vector3.zero;
            }

            Collider[] colliders = Physics.OverlapSphere(targetPosition, projectileModel.spherInfluence);
            foreach (Collider collider in colliders) {
                CharacterMono targetMono = collider.GetComponent<CharacterMono>();

                if (targetMono == null) continue;

                // 产生敌人身上的特效
                if (projectileModel.tartgetEnemryEffect != null) {
                    EffectsLifeCycle lifeCycle = TransientGameObjectFactory.AcquireObject(EffectConditonalType.During, templateObject: projectileModel.tartgetEnemryEffect, during: 2f);
                    lifeCycle.transform.SetParent(target.transform);
                    lifeCycle.transform.localPosition = Vector3.zero;
                }

                Damage damage = DamageFunc(targetMono.characterModel);
                targetMono.characterModel.Damaged(targetMono,damage,launcher);
            }
        }
    }
}

