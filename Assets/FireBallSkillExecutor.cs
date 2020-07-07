using UnityEngine;

[CreateAssetMenu(menuName="SkillBook/FireBall")]
public class FireBallSkillExecutor : ActiveSkill
{
    public GameObject fireBallProj;
    public override void ExecuteAction(Transform origin){
        Instantiate(fireBallProj, origin.transform.position, origin.rotation).GetComponent<Projectile>().Shoot();
    }
}
