using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController2D : MonoBehaviour {
    
    [SerializeField]
    float speed = 6.0f; //Move Speed

    private Vector2 moveDir = Vector3.zero; // Move Direction
    private Animator animator; //Animator Componenet
    
    private void Start() //Runs on Initilisation
    {
        //Assigns Animator Component
        animator = GetComponentInChildren<Animator>();
    }
    private void Update() //Runs every Frame
    {
    }
    private void FixedUpdate() //Runs every Physics pass
    {
        //Gets movement direction from Input
        moveDir = Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical");
        moveDir = moveDir.normalized;
        //If there is input set walking animation and rotate player model in the direction of movement
        if (moveDir != Vector2.zero)
        {
            //animator.SetBool("Walking", true);
        }
        //Else disable walk animation
        else
        {
            //animator.SetBool("Walking", false);
        }
        moveDir *= speed * 0.4f * Time.deltaTime;
        //Add movement to rigidbody component
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity += moveDir;
    }
    


}
