using UnityEngine;
using System.Collections.Generic;
using Kryz.CharacterStats;

public abstract class Skill : ScriptableObject
{
    [SerializeField] public string SkillName;
    [SerializeField] public Sprite Icon;

    [SerializeField] public List<Skill> PreReqSkills;
    public virtual void Destroy(){
        Destroy(this);
    }
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
    
    public virtual void ExecuteAction(Transform origin){

    }
}


[CreateAssetMenu(menuName="Skills/PassiveSkill")]
public class PassiveSkill : Skill
{
    public virtual void ApplyPassive(){}
}
