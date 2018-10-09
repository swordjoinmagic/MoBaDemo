using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 用于管理一个单位在游戏中的逻辑,如播放动画,播放音效,进行攻击等等操作.
/// </summary>
[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class CharacterMono : MonoBehaviour {
    private SimpleCharacterViewModel simpleCharacterViewModel;

    // 当前人物的动画组件以及寻路组件
    private Animator animator;
    private NavMeshAgent agent;

    /// <summary>
    /// 表示当前单位的一些基本属性,如:hp,mp,攻击力等等
    /// </summary>
    public CharacterModel characterModel;

    // 当前准备释放的技能
    public ActiveSkill prepareSkill = null;

    // 表示是否准备释放法术
    public bool isPrepareUseSkill = false;

    // 周围的敌人
    public List<CharacterMono> arroundEnemies;

    public SimpleCharacterViewModel SimpleCharacterViewModel {
        get {
            return simpleCharacterViewModel;
        }

        set {
            simpleCharacterViewModel = value;
        }
    }

    //================================================
    // ●测试用
    public GameObject targetPositionEffect;
    public GameObject targetEnemryEffect;
    public ProjectileMono projectile;
    public int Hp;
    public int Mp;
    public int maxHp;
    public int maxMp;
    public string characterName;
    public void Install() {
        characterModel = new CharacterModel {
            maxHp = maxHp,
            Hp = Hp,
            maxMp = maxMp,
            Mp = Mp,
            name = characterName,
            attackDistance = 2f,
            //projectileModel = new ProjectileModel {
            //    spherInfluence = 2f,
            //    targetPositionEffect = targetPositionEffect,
            //    tartgetEnemryEffect = targetEnemryEffect,
            //    movingSpeed = 7,
            //    turningSpeed = 1
            //},
            //projectile = projectile,
            activeSkills = new List<ActiveSkill> {
                new PointingSkill{
                    BaseDamage = 10,
                    KeyCode = KeyCode.E,
                    Mp = 10,
                    PlusDamage = 200,
                    self = gameObject,
                    selfEffect = null,
                    target = null,
                    targetEffect = null,
                    SpellDistance = 4f,
                    CD = 2f,
                    skillName = "E技能",
                    iconPath = "00046",
                    description = "one skill Description",
                },
                new PointingSkill{
                    BaseDamage = 10,
                    KeyCode = KeyCode.W,
                    Mp = 220,
                    PlusDamage = 200,
                    self = gameObject,
                    selfEffect = null,
                    target = null,
                    targetEffect = null,
                    SpellDistance = 4f,
                    CD = 5f,
                    skillName = "W技能",
                    iconPath = "00041",
                    description = "用于测试，这是一个技能描述，比较长的测试，用来观察富文本框的长度会产生怎样的变化",
                    skillLevel = 6
                }
            }
        };
    }
    //================================================

    private void Awake() {
        Install();

        //============================
        // 与ViewModel双向绑定
        Bind();

        // 获得该单位身上绑定的组件
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 获得该单位周围的敌人
        arroundEnemies = transform.Find("SearchTrigger").GetComponent<SearchTrigger>().arroundEnemies;
    }

    /// <summary>
    /// 处理人物追击的逻辑
    /// 当人物追击完成(也就是移动到了目标单位面前)返回true,否则返回false 
    /// <para></para>
    /// 追击部分
    /// 当移动到小于攻击距离时，自动停止移动,
    /// 否则继续移动,直到追上敌人,或者敌人消失在视野中
    /// </summary>
    /// <param name="targetTransform">要追击的单位的位置</param>
    /// <returns></returns>
    public bool Chasing(Transform targetTransform) {
        // 获得当前单位与目标单位的距离
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(targetTransform.position.x, targetTransform.position.z)
        );

        if (!agent.pathPending && distance <= characterModel.attackDistance) {
            animator.SetBool("isRun", false);
            agent.isStopped = true;

            return true;
        } else {
            animator.SetBool("isRun", true);
            agent.isStopped = false;
            agent.SetDestination(targetTransform.position);

            return false;
        }
    }

    /// <summary>
    /// 处理人物攻击的函数,完成一次攻击返回True，否则返回False
    /// </summary>
    /// <param name="isAttackFinish">本次攻击是否完成</param>
    /// <param name="targetTransform">目标敌人的Transform</param>
    /// <param name="target">目标敌人的Mono对象</param>
    public bool Attack(ref bool isAttackFinish, Transform targetTransform, CharacterMono target) {
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);

        //======================================
        // 播放攻击动画
        // 如果准备开始攻击,那么播放动画
        if (!currentAnimatorStateInfo.IsName("attack")) {
            animator.SetTrigger("attack");
            isAttackFinish = false;
        }


        //======================================
        // 伤害判断
        if (currentAnimatorStateInfo.IsName("attack") &&
            nextAnimatorStateInfo.IsName("Idle") &&
            !isAttackFinish) {

            if (characterModel.projectileModel == null) {
                target.characterModel.Hp -= 50;
            } else {
                Transform shotPosition = transform.Find("shootPosition");
                ProjectileMono projectileMono = Instantiate(characterModel.projectile, shotPosition.position, Quaternion.identity);
                projectileMono.targetPosition = targetTransform.position;
                projectileMono.target = target;
                projectileMono.damage = new Damage() { BaseDamage = 100 };
                projectileMono.projectileModel = characterModel.projectileModel;
            }
            isAttackFinish = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 重置人物的攻击动画
    /// </summary>
    public void ResetAttackStateAnimator() {
        animator.ResetTrigger("attack");
    }

    /// <summary>
    /// 重置目前单位的所有动画,如攻击动画、移动动画、施法动画。
    /// </summary>
    public void ResetAllStateAnimator() {
        animator.ResetTrigger("spell");
        animator.ResetTrigger("attack");
        animator.SetBool("isRun", false);
    }

    /// <summary>
    /// 移动到指定地点,移动结束返回False,移动尚未结束返回True
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Move(Vector3 position) {
        ResetAttackStateAnimator();
        animator.SetBool("isRun", true);
        agent.isStopped = false;
        agent.SetDestination(position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            animator.SetBool("isRun", false);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 判断人物当前准备释放的技能是不是立即释放技能,如果是,那么返回True,反之返回False
    /// </summary>
    /// <returns></returns>
    public bool IsImmediatelySpell() {
        return prepareSkill.SpellDistance == 0;
    }
     
    /// <summary>
    /// 释放技能的函数,施法结束返回True,施法失败或施法未完成返回False
    /// </summary>
    public bool Spell(CharacterMono enemryMono,Transform enermyTransform) {
        // 获得当前动画和下一个动画状态
        AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(0);
        CharacterModel enemryModel = enemryMono.characterModel;

        if (IsImmediatelySpell()) {
            // 原地释放技能,此时直接释放技能

            // 播放释放技能的动画
            if (!currentAnimatorStateInfo.IsName("Spell"))
                animator.SetTrigger("spell");

            // 如果技能释放结束,那么产生特效,计算伤害
            if (currentAnimatorStateInfo.IsName("Spell") &&
                nextAnimatorStateInfo.IsName("Idle")) {

                // 计算伤害
                Damage damage = prepareSkill.Execute();
                enemryModel.Damaged(damage);

                isPrepareUseSkill = false;
                prepareSkill = null;
                return true;
            }
        } else {
            // 指向型技能

            PointingSkill pointingSkill = prepareSkill as PointingSkill;

            // 当前距离敌人 > 施法距离,进行追击
            if (Chasing(enermyTransform)) {
                //======================================
                // 播放施法动画
                // 如果准备开始施法,那么播放动画
                if (!currentAnimatorStateInfo.IsName("Spell")) {
                    animator.SetTrigger("spell");
                }

                // 如果技能释放结束,那么产生特效,计算伤害
                if (currentAnimatorStateInfo.IsName("Spell") &&
                    nextAnimatorStateInfo.IsName("Idle")) {

                    pointingSkill.target = enemryMono.gameObject;

                    Damage damage = pointingSkill.Execute();
                    enemryModel.Hp -= damage.TotalDamage;

                    Debug.Log("释放技能");

                    isPrepareUseSkill = false;
                    prepareSkill = null;
                    return true;
                }
            }
        }
        return false;
    }




    //======================================
    // ●绑定Model中的各项属性到ViewModel中
    public void Bind() {
        characterModel.HpValueChangedHandler += OnHpValueChanged;
    }
    public void OnHpValueChanged(int oldHp,int newHp) {
        simpleCharacterViewModel.Hp.Value = newHp;
    }

}

