using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaypointWander : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    Seeker seeker;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    Path path;
    [SerializeField] int currentWaypoint = 0;

    Rigidbody2D rb;
    public GameObject wanderNodesParent;

    [SerializeField] private Vector3[] wanderNodes;
    private int counter;
    EnemyAI aI;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        aI = GetComponent<EnemyAI>();
        aI.enabled = false;    

        wanderNodes = new Vector3[wanderNodesParent.transform.childCount + 1];
        counter = 0;
        foreach(Transform loc in wanderNodesParent.GetComponentsInChildren<Transform>()){
            wanderNodes[counter] = loc.position;
            counter++;
        }
        counter = 1;
        seeker.StartPath(rb.position, wanderNodes[counter], OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
        }
    }


    void FixedUpdate()
    {
        if(path == null)
            return;
        if(currentWaypoint >= path.vectorPath.Count)
        {
            currentWaypoint = 0;
            path = null;
            counter++; 
            if(counter >= wanderNodes.Length)
                counter = 0;
            seeker.StartPath(rb.position, wanderNodes[counter], OnPathComplete);
            return;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        fieldOfView.SetAimDirection(direction);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }
}
