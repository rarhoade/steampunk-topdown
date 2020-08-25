using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BaseAI : MonoBehaviour
{
    //State control variables
    private enum State {
        Roaming,
        ChasingTarget,
        Investigating,
        LookingAround,
    }
    Vector3 originalScale;
    [SerializeField] private State state;
    public float AwarenessLimit = 100f;
    public float AwarenessDecrement = 4f;

    //View Cone variables
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
    Coroutine investigationCoroutine;

    Rigidbody2D rb;
    public GameObject wanderNodesParent;
    Vector3 target;
    Transform chaseTarget;

    [SerializeField] private Vector3[] wanderNodes;
    private int waypointCounter;
    [SerializeField] private int investigationCounter;
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        chaseTarget = null;
        originalScale = transform.localScale;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        wanderNodes = new Vector3[wanderNodesParent.transform.childCount + 1];
        waypointCounter = 0;
        investigationCounter = 0;
        foreach(Transform loc in wanderNodesParent.GetComponentsInChildren<Transform>()){
            wanderNodes[waypointCounter] = loc.position;
            waypointCounter++;
        }
        waypointCounter = 0;
        target = wanderNodes[waypointCounter];
        CalculatePath();
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

        if(currentAwareness > AwarenessLimit)
            currentAwareness = AwarenessLimit;
        
        switch(state) {
            case State.Roaming:
                WalkTowardsTarget();
                break;
            case State.ChasingTarget:
                WalkTowardsTarget();
                break;
            case State.Investigating:
                WalkTowardsTarget();
                break;
            case State.LookingAround:
                break;
            default:
                WalkTowardsTarget();
                break;
        }
    }

    void UpdatePath(){
        if(chaseTarget && seeker.IsDone())
            seeker.StartPath(rb.position, chaseTarget.position, OnPathComplete);
    }

    void ToggleChasing(Transform source=null){
        if(!source){
            chaseTarget = null;
            CancelInvoke();
        }
        else{
            chaseTarget = source;
            InvokeRepeating("UpdatePath", 0f, .5f);
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
                CalculatePath();
                break;
            case State.ChasingTarget:
                break;
            case State.Investigating:
                if(investigationCounter < 3){
                    if(targetTransform)
                        target = targetTransform.position;
                    else
                        target = GetRoamingPosition();   
                    CalculatePath();
                }
                else{
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
        if(state != State.ChasingTarget){
            if(a >= AwarenessLimit){
                currentAwareness = AwarenessLimit;
                //time to chase target
                state = State.ChasingTarget;
            }
            else if(currentAwareness > AwarenessLimit * 0.7){
                //time to investigate
                investigationCounter = 0;
                state = State.Investigating;
                if(source){
                    CalculateNextTarget(source);
                }
                
            }
        }
        
    }

    void WalkTowardsTarget()
    {
        if(canMove){
            if(CheckPathing())
                return;
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

    IEnumerator LookingAround(){
        //looking around
        rb.velocity = Vector2.zero;
        canMove = false;
        float time = 2f;
        float counter = 0f;
        for(int i = 0; i < 3; i++){
            Vector3 newDirection = GetRandomDir();
            while(counter < time){
                counter += Time.deltaTime;
                ViewCone(newDirection);
                yield return null;
            }
            counter = 0;
        }
        investigationCounter++;
        //Go back to investigating
        if(investigationCounter < 3){
            state = State.Investigating;
        }
        else {
        //If we've hit the cap on investigating go back to roaming
            investigationCounter = 0;
            state = State.Roaming;
        }
        CalculateNextTarget();
        canMove = true;
    }

    bool CheckPathing(){
        if(path == null)
            return true;
        if(currentWaypoint >= path.vectorPath.Count)
        {
            currentWaypoint = 0;
            path = null;
            if(state == State.Investigating){
                state = State.LookingAround;
                investigationCoroutine = StartCoroutine("LookingAround");
            }
            else{
                CalculateNextTarget();
                CalculatePath();
            }
            return true;
        }
        return false;
    }

    void CalculatePath(){
        seeker.StartPath(rb.position, target, OnPathComplete);
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
                    if(state == State.LookingAround || state == State.Investigating){
                        Debug.Log("This is being called");
                        state = State.ChasingTarget;
                        canMove = true;
                        ToggleChasing(raycastHit2D.collider.gameObject.transform);
                        StopCoroutine(investigationCoroutine);
                    }
                    else{
                        AddAwareness(awarenessIncrement * ((viewDistance - distToPlayer)/viewDistance));
                    }
                    return true;
                }
            }
        }
        return false;
    }
}
