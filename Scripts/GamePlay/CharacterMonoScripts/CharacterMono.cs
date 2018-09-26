using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterMono : MonoBehaviour {

    private SimpleCharacterViewModel simpleCharacterViewModel;

    public CharacterModel characterModel;

    public int Hp;
    public int Mp;
    public int maxHp;
    public int maxMp;
    public string characterName;

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

    private void Awake() {
        characterModel = new CharacterModel {
            maxHp = maxHp,
            Hp = Hp,
            maxMp = maxMp,
            Mp = Mp,
            
            name = characterName,
            attackDistance = 1.5f,
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
}

