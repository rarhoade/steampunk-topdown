using UnityEngine;
using System.Collections.Generic;
using Kryz.CharacterStats;

public abstract class Skill : ScriptableObject
{
    [SerializeField] string SkillName;
    [SerializeField] Sprite Icon;

    [SerializeField] List<Skill> PreReqSkills;
}

[CreateAssetMenu(menuName="Skills/ActiveSkill")]
public class ActiveSkill : Skill
{
    public float ScaleValue;
    public float BaseDamage;
    public enum StatType
    {
        Strength,
        Agility,
        Intelligence,
        Vitality,
        None
    }
    public StatType dropDown = StatType.None;

    public int GetStatIdx(){
        return (int)dropDown;
    }
}

[CreateAssetMenu(menuName="Skills/PassiveSkill")]
public class PassiveSkill : Skill
{
    public virtual void ApplyPassive(){}
}
