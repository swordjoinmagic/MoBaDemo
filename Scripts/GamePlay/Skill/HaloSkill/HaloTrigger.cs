using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HaloTrigger : MonoBehaviour{
    // 触发器的半径
    private float tiggerRadius;
    // 符合该光环作用的目标类型
    private UnitFaction unitType;
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

    public UnitFaction UnitType {
        get {
            return unitType;
        }

        set {
            unitType = value;
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

    public delegate void HaloSkillExecuteHandler(CharacterMono speller, CharacterMono target);
    public event HaloSkillExecuteHandler HaloSkillExecute;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("单位进入了触发器");

        CharacterMono target = other.GetComponent<CharacterMono>();
        if (target != null && target.characterModel.unitFaction == UnitType) {
            TargetList.Add(target);

            Debug.Log("单位进入了触发器");
            if (HaloSkillExecute != null) HaloSkillExecute(null,target);
        }
    }
    private void OnTriggerExit(Collider other) {
        CharacterMono target = other.GetComponent<CharacterMono>();
        if (target != null && target.characterModel.unitFaction == UnitType) {
            TargetList.Remove(target);
        }
    }
}

