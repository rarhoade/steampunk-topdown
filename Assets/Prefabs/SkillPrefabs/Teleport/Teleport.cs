using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName="SkillBook/Teleport")]
public class Teleport : ActiveSkill
{
    public ActiveSkill referenceSkill;
    public float distance;

    public override void ExecuteAction(Transform origin) {
        Rigidbody2D rb2d = origin.transform.root.gameObject.GetComponent<Rigidbody2D>();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2 (
            mousePosition.x - origin.transform.position.x,
            mousePosition.y - origin.transform.position.y
        );

        Vector2 newPos = (Vector2)origin.transform.position + (distance * direction.normalized);
        rb2d.position = newPos;
    }
}
