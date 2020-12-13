using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    public CharacterController2D controller;

    public float runSpeed= 40f;

    float horizontalMove = 0f;

    bool jump = false;

    bool block = false;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if(Input.GetButtonDown("Jump")){
             jump = true;
             animator.SetBool("IsJumping", true); 
        }
        
        if( Input.GetKeyDown(KeyCode.Q) ){ 
            block = true;
            runSpeed = 0f;
        } else if( Input.GetKeyUp(KeyCode.Q) ){
            block = false;
            runSpeed= 40f;
        } 

    }

    public void OnLanding(){
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
        



    }
    
}
