using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Skills/ProjectileSkills/FireBall")]
public class FireBall: Projectile
{
    public override IEnumerator DoDamage(Enemy e, Collision2D col){
        e.TakeDamage((int)storedDamage, -1 * col.contacts[0].normal);
        yield return null;
    }
}
