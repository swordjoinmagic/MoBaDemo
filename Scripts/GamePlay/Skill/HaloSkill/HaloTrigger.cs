using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HaloTrigger : MonoBehaviour{
    // 触发器的半径
    private float tiggerRadius;
    // 符合该光环作用的目标类型
    private UnitType skillTargetType;
    // 此光环技能释放者
    private CharacterMono speller;

    // 目前此光环作用的单位列表
    private List<CharacterMono> targetList = new List<CharacterMono>();

    public float TiggerRadius {
        get {
            return tiggerRadius;
        }

        set {
            tiggerRadius = value;
        }
    }

    public UnitType SkillTargetType {
        get {
            return skillTargetType;
        }

        set {
            skillTargetType = value;
        }
    }

    public List<CharacterMono> TargetList {
        get {
            return targetList;
        }

        set {
            targetList = value;
        }
    }

    public CharacterMono Speller {
        get {
            return speller;
        }

        set {
            speller = value;
        }
    }

    public delegate void HaloSkillExecuteHandler(CharacterMono speller, CharacterMono target);
    public event HaloSkillExecuteHandler HaloSkillExecute;
    public event HaloSkillExecuteHandler HaloSkillCancelExecute;

    private void OnTriggerEnter(Collider other) {
        CharacterMono target = other.GetComponent<CharacterMono>();
        if (target != null) {
            TargetList.Add(target);

            //Debug.Log("单位进入了触发器");
            if (HaloSkillExecute != null && speller!=null) HaloSkillExecute(speller, target);
        }
    }
    private void OnTriggerExit(Collider other) {
        CharacterMono target = other.GetComponent<CharacterMono>();
        if (target != null) {
            TargetList.Remove(target);
            if (HaloSkillCancelExecute != null && speller != null) HaloSkillCancelExecute(speller,target);
        }
    }

    private void OnDisable() {
        foreach (var target in targetList) {
            if (HaloSkillCancelExecute != null) HaloSkillCancelExecute(speller, target);
        }
    }
}

