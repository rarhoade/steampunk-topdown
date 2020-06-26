using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileSkill : MonoBehaviour
{
    public float BaseDamage = 0f;
    public float ProjectileVelocity;
    public float storedDamage;
    public ActiveSkill ReferenceSkill;
    public float DefaultDamage;
    private int playerNumber;

    
    public virtual IEnumerator DoDamage(Enemy e, Collision2D col){
        GetComponent<Enemy>().TakeDamage((int)storedDamage, -1 * col.contacts[0].normal);
        yield return null;
    }

    public virtual void Shoot(int playerIdx){
        playerNumber = playerIdx;
        storedDamage = ReferenceSkill.BaseDamage + 
            GameHandler.Instance.FetchCharStat(playerNumber, ReferenceSkill.GetStatIdx()) 
            * ReferenceSkill.ScaleValue;
        GetComponent<Rigidbody2D>().AddForce(transform.up * ProjectileVelocity);
    }

    public void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.layer == 8){
            StartCoroutine(DoDamage(col.gameObject.GetComponent<Enemy>(), col));
        }
        Destroy(this.gameObject);
    }
}
