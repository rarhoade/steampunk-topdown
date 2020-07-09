using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    public Animator weaponAnimator;
    public Rigidbody2D rb;
    public bool canAttack = true;
    public float attackMomentum = 0.1f;
    public GameObject sampleSkill;
    public Transform ShotOrigin;
    public Character c;
    public GameObject Appendages;
    public delegate void SkillAction(Transform shot);
    public static List<SkillAction> SkillSlots;

    private int skillSlotSize = 4;
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Character>();
        SkillSlots = new List<SkillAction>(skillSlotSize);
        for(int i = 0; i < skillSlotSize; i++){
            SkillSlots.Add(null);
        }
    }
    // Start is called before the first frame update
    void Update()
    {
        if(canAttack){
            if (Input.GetMouseButtonDown(0)){
                Attack();
            }       
            if(Input.GetMouseButtonDown(1)){
                Debug.Log("alt weapon");
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1)){
                SkillSlots[0](ShotOrigin);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)){
                SkillSlots[1](ShotOrigin);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)){
                SkillSlots[2](ShotOrigin);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4)){
                SkillSlots[3](ShotOrigin);
            }
        }
        
    }

    public void HideArms(){

    }

    public void ShowArms(){

    }

    // Update is called once per frame
    void Attack(){
        weaponAnimator.SetTrigger("Attack");
    }

    public void SetAnimator(Animator newAnimator){
        weaponAnimator = newAnimator;
    }

    public void RegisterSkill(SkillAction NewAction, int slot){
        //get rid of old skill
        SkillSlots[slot] -= SkillSlots[slot];
        //register new skill
        SkillSlots[slot] += NewAction;
    }
}
