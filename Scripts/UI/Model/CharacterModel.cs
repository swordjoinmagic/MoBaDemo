using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class CharacterModel {
    public int Hp;
    public int Mp;
    public int maxHp;
    public int maxMp;
    public string name;

    // 该单位的普通攻击距离
    public float attackDistance;

    // 当前角色的所有技能
    public List<BaseSkill> baseSkills = new List<BaseSkill>();
    // 当前角色的所有主动技能
    public List<ActiveSkill> activeSkills = new List<ActiveSkill>();
    // 当前角色所有被动技能
    public List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
    
}

