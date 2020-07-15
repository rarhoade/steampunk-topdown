using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillWindow : MonoBehaviour
{
    // Start is called before the first frame update
    public AvailableSkillSlot[] AvailableSkills;
    public UsableSkillSlot[] SlottedSkills;
    [SerializeField] protected ActiveSkill[] startingSlottedSkills;
    [SerializeField] protected ActiveSkill[] startingSkills;
	public event Action<BaseSkillSlot> OnPointerEnterEvent;
	public event Action<BaseSkillSlot> OnPointerExitEvent;
	public event Action<BaseSkillSlot> OnRightClickEvent;
	public event Action<BaseSkillSlot> OnBeginDragEvent;
	public event Action<BaseSkillSlot> OnEndDragEvent;
	public event Action<BaseSkillSlot> OnDragEvent;
	public event Action<BaseSkillSlot> OnDropEvent;
    // Update is called once per frame
    private void OnValidate(){
        SlottedSkills = this.gameObject.GetComponentsInChildren<UsableSkillSlot>();
        AvailableSkills = this.gameObject.GetComponentsInChildren<AvailableSkillSlot>();
    }

    protected virtual void Awake() {
        foreach(UsableSkillSlot useable in SlottedSkills){
            useable.OnPointerEnterEvent += slot => EventHelper(slot, OnPointerEnterEvent);
            useable.OnPointerExitEvent += slot => EventHelper(slot, OnPointerExitEvent);
            useable.OnRightClickEvent += slot => EventHelper(slot, OnRightClickEvent);
            useable.OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
            useable.OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
            useable.OnDragEvent += slot => EventHelper(slot, OnDragEvent);
            useable.OnDropEvent += slot => EventHelper(slot, OnDropEvent);
        }
        foreach(AvailableSkillSlot available in AvailableSkills){
            available.OnPointerEnterEvent += slot => EventHelper(slot, OnPointerEnterEvent);
            available.OnPointerExitEvent += slot => EventHelper(slot, OnPointerExitEvent);
            available.OnRightClickEvent += slot => EventHelper(slot, OnRightClickEvent);
            available.OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
            available.OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
            available.OnDragEvent += slot => EventHelper(slot, OnDragEvent);
            available.OnDropEvent += slot => EventHelper(slot, OnDropEvent);
        }
    }

    public void Start(){
        SetStartingSkills();
    }

    private void EventHelper(BaseSkillSlot skillSlot, Action<BaseSkillSlot> action){
        if (action != null)
            action(skillSlot);
    }

    public void EquipSkill(ActiveSkill skill, int slotIndex, UsableSkillSlot uiElement = null){
        //if reference to uiElement exists use that
        if(uiElement){
            for(int i =0 ; i < SlottedSkills.Length; i++){
                if(uiElement == SlottedSkills[i]){
                    PlayerCombat.SkillSlots[i] -= PlayerCombat.SkillSlots[i];
                    PlayerCombat.SkillSlots[i] += skill.ExecuteAction;
                    SlottedSkills[i].Skill = skill;
                }
            }
        }
        else{
            //otherwise the slot index provided
            PlayerCombat.SkillSlots[slotIndex] -= PlayerCombat.SkillSlots[slotIndex];
            PlayerCombat.SkillSlots[slotIndex] += skill.ExecuteAction;
            SlottedSkills[slotIndex].Skill = skill;
        }
    }

    public void RemoveSkill(ActiveSkill skill){
        for(int i = 0; i < SlottedSkills.Length; i++){
            if(SlottedSkills[i].Skill == skill){
                SlottedSkills[i].Skill = null;
                PlayerCombat.SkillSlots[i] -= PlayerCombat.SkillSlots[i];
            }
        }
    }

    public void Clear()
    {
        foreach(UsableSkillSlot useable in SlottedSkills){
            if(useable.Skill != null && Application.isPlaying){
                useable.Skill.Destroy();
            }
            useable.Skill = null;
        }
        foreach(AvailableSkillSlot available in AvailableSkills){
            if(available.Skill != null && Application.isPlaying){
                available.Skill.Destroy();
            }
            available.Skill = null;
        }
    }

    private void SetStartingSkills()
    {
        Clear();
        for(int i = 0; i < startingSlottedSkills.Length; i++){
            if(startingSlottedSkills[i]){
                EquipSkill(startingSlottedSkills[i], i);
            }
        }
        for(int i = 0; i < startingSkills.Length; i++){
            if(startingSkills[i]){
                AvailableSkills[i].Skill = startingSkills[i];
            }
        }
    }
    
}
