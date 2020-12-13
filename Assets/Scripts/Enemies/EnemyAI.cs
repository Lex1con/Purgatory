using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 5f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physicis")]
    public float speed = 600f;
    public float nextWaypointDistance = 3f;
    public float jumNodeHeightRequirement = 0.8f;

    public float jumpModifier = 0.2f;
    public float jumpCheckOffset = 0.1f;

    [Header("CustomBehavior")]
    private GameObject main; 
    public bool followEnabled = true;
    public bool jumpEnabled = true;

    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rigidbody;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Main"); //Access the main hidden game object running necessary scripts
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    void OnDrawGizmosSelected() {

        if(activateDistance == null || rigidbody == null){
            return;
        }
        Gizmos.DrawWireSphere(rigidbody.position, activateDistance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TargetInDistance() && followEnabled){
            PathFollow();
        }else {
            animator.SetBool("isMoving", false);
        }


    }

    // public bool TargetInAttackDistance(){
    //     return Vector2.Distance(transform.position, target.transform.position) < attackDistance;
    // }

    public void Stop(){
        target = this.transform;
        followEnabled = false;
        animator.SetBool("isMoving", false);
        animator.SetBool("IsDead", true);
        this.enabled = false;
        main.GetComponent<MainScript>().score += 1;
        if(main.GetComponent<MainScript>().score > PlayerPrefs.GetInt("highscore")){
            PlayerPrefs.SetInt("highscore", main.GetComponent<MainScript>().score);
        }
        GetComponent<EnemyCombat>().Die();
    }
    
    void UpdatePath(){
        if (followEnabled && TargetInDistance() && seeker.IsDone()){
            seeker.StartPath(rigidbody.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow(){
        if (path == null){
            return;
        }

        //End Of Path
        if(currentWaypoint >= path.vectorPath.Count){
            return;
        }

        //Check for collision with anything
        
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);
        if(isGrounded){
            animator.SetBool("IsJumping", false);
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(rigidbody.velocity.x != 0f){
            animator.SetBool("isMoving", true);
        }else{
            animator.SetBool("isMoving", false);
        }


        //Jump
        if(jumpEnabled && isGrounded){
            if (direction.y > jumNodeHeightRequirement){
                rigidbody.AddForce(Vector2.up * speed * jumpModifier);
                animator.SetBool("IsJumping", true);
            }
        }

        rigidbody.AddForce(force);
        

        //Next Waypoint
        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance){
            currentWaypoint++;
        }

        //Direction Graphic Handling
        if(directionLookEnabled){
            if(rigidbody.velocity.x > 0.05f){
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } else if (rigidbody.velocity.x < -0.05f){
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

    }

    private bool TargetInDistance(){
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p){
        if ( !p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
}
