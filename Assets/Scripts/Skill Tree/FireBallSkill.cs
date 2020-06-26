using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Skills/ProjectileSkills/FireBall")]
public class FireBallSkill : ProjectileSkill
{
    public override IEnumerator DoDamage(Enemy e, Collision2D col){
        Debug.Log("Fire Burn");
        e.TakeDamage((int)storedDamage, -1 * col.contacts[0].normal);
        yield return null;
    }
}
