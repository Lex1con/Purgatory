using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Variables")]
    
    public Animator animator;

    public int maxHealth = 100;
    private int currentHealth;

    public Transform attackPoint;

    private Rigidbody2D rigidbody;

    private Transform target;

    public LayerMask PlayerLayers;

    public float attackRate =10f;

    private float nextAttackTime = 0f;

    public float attackDistance = 3f; //range for AI attack

    bool canAttack = true;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
        target = GetComponent<EnemyAI>().target;

    }

    private void FixedUpdate() {
        // Debug.Log(TargetInAttackDistance());
        if(Time.time < nextAttackTime){
            canAttack = false;
        }else{
            canAttack = true;
        }
        // Debug.Log("Can Attack: "+canAttack);
        if(TargetInAttackDistance() && canAttack){
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack(){
        //Play animation
        animator.SetTrigger("Attack");
        //Detect Player
        Collider2D[] Players = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, PlayerLayers);
        //Damage them
        Collider2D player =  Players[1];

        Debug.Log(player.name + "Hit");
        player.GetComponent<PlayerCombat>().TakeDamage(10);
        if(target.transform.position.x < rigidbody.position.x){
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f * 5f, 0.0f);
        }else{
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * 5f, 0.0f);
        }
    }

    public void TakeDamage (int damage){
        currentHealth -= damage;

        //play hurt animation
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0 ){
            GetComponent<EnemyAI>().Stop();
            Die();
        } 
    }

    public bool TargetInAttackDistance(){
        return Vector2.Distance(transform.position, target.transform.position) < attackDistance;
    }

    public void Die(){
        Debug.Log("Enemy died");
        GetComponent<Rigidbody2D>().gravityScale = 0;
        //disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void OnDrawGizmosSelected() {

        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}

