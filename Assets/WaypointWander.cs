using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaypointWander : MonoBehaviour
{
    //State control variables
    private enum State {
        Roaming,
        ChasingTarget,
        Investigating,
    }
    [SerializeField] private State state;
    public float AwarenessLimit = 100f;
    public float AwarenessDecrement = 4f;

    //View Cone varialbes
    [SerializeField] private float currentAwareness = 0f;
    [SerializeField] private float fov;
    [SerializeField] private float viewDistance;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private GameObject fovPf;
    
    [SerializeField] private LayerMask layerMask;
    
    //Pathfinding Variables
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
        //Delete later;
        aI = GetComponent<EnemyAI>();
        aI.enabled = false;    

        wanderNodes = new Vector3[wanderNodesParent.transform.childCount + 1];
        counter = 0;
        foreach(Transform loc in wanderNodesParent.GetComponentsInChildren<Transform>()){
            wanderNodes[counter] = loc.position;
            counter++;
        }
        counter = 0;
        seeker.StartPath(rb.position, wanderNodes[counter], OnPathComplete);
        //Field of View Instantiation
        fieldOfView = Instantiate(fovPf, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
        state = State.Roaming;
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
        }
    }


    void FixedUpdate(){
        currentAwareness -= AwarenessDecrement;
        switch(state) {
            case State.Roaming:
                WalkTowardsTarget();
                break;
            case State.ChasingTarget:
                WalkTowardsTarget();
                break;
            case State.Investigating:
                Investigate();
                state = State.Roaming;
                break;
            default:
                WalkTowardsTarget();
                break;
        }
    }

    void ViewCone(Vector2 direction){
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(direction);
        FindTargetPlayer(direction);
    }

    public void AddAwareness(float a, Transform source=null){
        if(source){
            //check if source is behind enemy
            //if so half the amount
        }
        currentAwareness += a;
        if(a >= AwarenessLimit){
            currentAwareness = AwarenessLimit;
            //time to chase target
            state = State.ChasingTarget;
        }
        else if(currentAwareness > AwarenessLimit * 0.7){
            //time to investigate
            state = State.Investigating;
        }
    }

    void WalkTowardsTarget()
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
        
        //viewcone
        ViewCone(direction);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    void Investigate(){
        seeker.StartPath(transform.position, GetRoamingPosition(), OnPathComplete);
    }

    public static Vector3 GetRandomDir() {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private Vector3 GetRoamingPosition() {
        return transform.position + GetRandomDir() * Random.Range(5f, 10f);
    }

    private void FindTargetPlayer(Vector2 direction){
        //Debug.Log(Vector3.Distance(transform.position, GameHandler.Instance.GetPlayerTransform()));
        float distToPlayer = Vector3.Distance(transform.position, GameHandler.Instance.GetPlayerTransform());
        if(distToPlayer < viewDistance){
            Vector3 dirToPlayer = (GameHandler.Instance.GetPlayerTransform() - transform.position).normalized;
            if(Vector3.Angle(direction, dirToPlayer) < fov / 2f){
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, layerMask);
                if(raycastHit2D.collider && raycastHit2D.collider.gameObject.tag == "Player"){
                    AddAwareness(viewDistance - distToPlayer);
                }
            }
        }
    }
}
