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
    Vector3 originalScale;
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
    Vector3 target;

    [SerializeField] private Vector3[] wanderNodes;
    private int waypointCounter;
    [SerializeField] private int investigationCounter;
    EnemyAI aI;
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //Delete later;
        aI = GetComponent<EnemyAI>();
        aI.enabled = false;    

        wanderNodes = new Vector3[wanderNodesParent.transform.childCount + 1];
        waypointCounter = 0;
        investigationCounter = 0;
        foreach(Transform loc in wanderNodesParent.GetComponentsInChildren<Transform>()){
            wanderNodes[waypointCounter] = loc.position;
            waypointCounter++;
        }
        waypointCounter = 0;
        target = wanderNodes[waypointCounter];
        seeker.StartPath(rb.position, target, OnPathComplete);
        //Field of View Instantiation
        fieldOfView = Instantiate(fovPf, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
        state = State.Roaming;
    }

    void FixedUpdate(){
        currentAwareness -= AwarenessDecrement;
        if(currentAwareness < 0)
            currentAwareness = 0;
        
        switch(state) {
            case State.Roaming:
                WalkTowardsTarget();
                break;
            case State.ChasingTarget:
                WalkTowardsTarget();
                break;
            case State.Investigating:
                if(investigationCounter == 0){
                    CalculateNextTarget();
                    seeker.StartPath(transform.position, target, OnPathComplete);
                }
                WalkTowardsTarget();
                break;
            default:
                WalkTowardsTarget();
                break;
        }
    }

    //targetTransform is meant to be a way insert a chasingTarget transform
    //It is done through CalculateNextTarget because this will scrub for the right state
    void CalculateNextTarget(Transform targetTransform=null){
        switch(state) {
            case State.Roaming:
                if(wanderNodes.Length > 0){
                    waypointCounter++;
                    if(waypointCounter >= wanderNodes.Length)
                        waypointCounter = 0;
                    target = wanderNodes[waypointCounter];
                }
                else{
                    target = GetRoamingPosition();
                }
                break;
            case State.ChasingTarget:
                target = targetTransform.position;
                break;
            case State.Investigating:
                if(investigationCounter < 3){
                    StartCoroutine("Investigate");
                    target = GetRoamingPosition();   
                    investigationCounter++;
                }
                else{
                    investigationCounter = 0;
                    state = State.Roaming;
                }
                break;
            default:
                target = GetRoamingPosition();
                break;
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
        }
    }

    void ViewCone(Vector2 direction){
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(direction);
        if(direction.x > 0){
            transform.localScale = originalScale;
        }
        else {
            transform.localScale = Vector3.Scale(originalScale, new Vector3(-1 , 1, 1));
        }
        FindTargetPlayer(direction, 5f);
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
        if(canMove){
            if(path == null)
                return;
            if(currentWaypoint >= path.vectorPath.Count)
            {
                currentWaypoint = 0;
                path = null;
                CalculateNextTarget();
                seeker.StartPath(rb.position, target, OnPathComplete);
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
        else{
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator Investigate(){
        canMove = false;
        yield return new WaitForSeconds(1f);
        ViewCone(GetRandomDir());
        yield return new WaitForSeconds(1f);
        ViewCone(GetRandomDir());
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    public static Vector3 GetRandomDir() {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private Vector3 GetRoamingPosition() {
        return transform.position + GetRandomDir() * Random.Range(5f, 10f);
    }

    private bool FindTargetPlayer(Vector2 direction, float awarenessIncrement=0f){
        //Debug.Log(Vector3.Distance(transform.position, GameHandler.Instance.GetPlayerTransform()));
        float distToPlayer = Vector3.Distance(transform.position, GameHandler.Instance.GetPlayerTransform());
        if(distToPlayer < viewDistance){
            Vector3 dirToPlayer = (GameHandler.Instance.GetPlayerTransform() - transform.position).normalized;
            if(Vector3.Angle(direction, dirToPlayer) < fov / 2f){
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, layerMask);
                if(raycastHit2D.collider && raycastHit2D.collider.gameObject.tag == "Player"){
                    AddAwareness(awarenessIncrement * ((viewDistance - distToPlayer)/viewDistance));
                    return true;
                }
            }
        }
        return false;
    }
}
