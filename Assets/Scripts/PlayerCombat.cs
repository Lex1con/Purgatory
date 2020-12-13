using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private GameObject main; 

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public float attackRate =2f;

    private float nextAttackTime = 0f;

    public int maxHealth = 100;
    private int currentHealth;

    public bool isBlocking = false;

    void Start() {
        main = GameObject.Find("Main"); //Access the main hidden game object running necessary scripts    
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime){
            if (Input.GetKeyDown(KeyCode.E)){
                Attack();
                nextAttackTime = Time.time + 1f/attackRate;
            }
        }

        if( Input.GetKeyDown(KeyCode.Q) ){ 
            isBlocking = true;
            animator.SetBool("IsBlocking", isBlocking);
        } else if( Input.GetKeyUp(KeyCode.Q) ){
            isBlocking = false;
            animator.SetBool("IsBlocking", isBlocking);
        } 
    
    }

    void Attack(){
        //Play animation
        animator.SetTrigger("Attack");
        //Detect enemies
        Collider2D[] enemieshit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
        foreach(Collider2D enemy in enemieshit){
            // Debug.Log(enemy.name + "Hit");
            enemy.GetComponent<EnemyCombat>().TakeDamage(50);
            if(enemy.transform.position.x < GetComponent<Rigidbody2D>().position.x){
                enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f * 5f, 0.0f);
            }else{
                enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * 5f, 0.0f);
            }
        }
    }

    public void TakeDamage (int damage){
        Debug.Log(damage);
        Debug.Log("Current Health "+currentHealth);


        if(isBlocking){ // If blocking take half damage
            if(currentHealth - damage <= 0){ //If blocking but would die reduce health to 1hp
                currentHealth = 1;
            }
            else {currentHealth -= damage/2;}    
        }
        else{ // Not blocking so take full damage
            currentHealth = currentHealth - damage;
            Debug.Log("Current Health "+currentHealth);
            //play hurt animation
            animator.SetTrigger("Hurt");
        }
        main.GetComponent<MainScript>().HealthText.text="Health: " + currentHealth;

        if(currentHealth <= 0 ){
            Die();
        }
    }

    public void Die(){
        Debug.Log("You died");
        animator.SetBool("IsDead", true);
        //disable player
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        main.GetComponent<MainScript>().playerAlive = false;
    }

    void OnDrawGizmosSelected() {

        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
