using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed = 200f;

    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    public Transform EnemyGFX;
    private Vector3 initScale;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //TODO: switch it so all orc graphics have a parent object
        //that will end up being enemygfx
        EnemyGFX = GetComponent<Transform>();
        initScale = EnemyGFX.localScale;

        InvokeRepeating("UpdatePath", 0f, .5f);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }


    void UpdatePath(){
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
            return;        

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }
        
        //switching graphic scale if velocity switches horizontally

        if(rb.velocity.x >= 0.01f){
            EnemyGFX.localScale = initScale;
        }
        else if(rb.velocity.x <= -0.01f){
            EnemyGFX.localScale = new Vector3(
                -initScale.x,
                initScale.y,
                initScale.z
            );
        }
    }
}
