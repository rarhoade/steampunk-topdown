using System.Collections; using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator weaponAnimator;
    public Rigidbody2D rb;
    public bool canAttack = true;
    public float attackMomentum = 0.1f;
    public GameObject sampleSkill;
    public Transform ShotOrigin;
    public Character c;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Character>();
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack){
            Attack();
        }       
        if(Input.GetMouseButtonDown(1) && canAttack){
            Instantiate(sampleSkill, ShotOrigin.transform.position, ShotOrigin.rotation).
                GetComponent<ProjectileSkill>().
                Shoot(c.GetPlayerNumber());
        }
    }

    // Update is called once per frame
    void Attack(){
        weaponAnimator.SetTrigger("Attack");
    }

    public void SetAnimator(Animator newAnimator){
        weaponAnimator = newAnimator;
    }
}
