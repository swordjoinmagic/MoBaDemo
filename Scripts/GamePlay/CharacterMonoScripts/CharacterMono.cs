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
            //    spherInfluence = 2.5f,
            //    targetPositionEffect = targetPositionEffect,
            //    tartgetEnemryEffect = targetEnemryEffect,
            //    movingSpeed = 1,
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

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
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
    /// 处理人物普通攻击的逻辑
    /// </summary>
    public void Attack(CharacterModel target) {

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

