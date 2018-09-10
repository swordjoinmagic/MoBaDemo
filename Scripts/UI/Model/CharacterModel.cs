using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterModel {
    public int Hp;
    public int Mp;
    public int maxHp;
    public int maxMp;
    public string name;
    // 当前角色的所有主动技能
    public List<ActiveSkill> activeSkills = new List<ActiveSkill>();
}

