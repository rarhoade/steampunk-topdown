using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public EquippableItem ReferenceItem;
    private int storedDamage;

    void Awake(){
        storedDamage = ReferenceItem.StrengthBonus;
    }

    public void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.layer == 8){
            col.gameObject.GetComponent<Enemy>().TakeDamage((int)storedDamage, -1 * col.contacts[0].normal);
        }
    }
}
