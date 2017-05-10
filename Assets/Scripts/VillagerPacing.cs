using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerPacing : MonoBehaviour {
    public float MovementSpeed;
    public float TimeWalking;
    public float TimeWaiting;

    public bool WalkingHorizontal;
    private bool Walking;

    public int direction;

    private float WalkingCounter;
    private float WaitingCounter;

    private Animator animator;
    private Vector3 lastPosition;

    private Rigidbody2D myRigidBody;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        WaitingCounter = TimeWaiting;
        WalkingCounter = TimeWalking;

        lastPosition = transform.position;

        if (WalkingHorizontal == true) {
            direction = 1;
        } else {
            direction = 2;
        }
        Walking = true;
    }

    void FixedUpdate()
    {
        if (Walking)
        {
            WalkingCounter -= Time.deltaTime;
            if (direction == 1) {
                myRigidBody.velocity = new Vector2(MovementSpeed, 0);
                animator.SetBool("Walking", true);
                animator.SetInteger("Direction", 1);
                animator.speed = .7f;
            }
            if (direction == -1) {
                myRigidBody.velocity = new Vector2(-MovementSpeed, 0);
                animator.SetBool("Walking", true);
                animator.SetInteger("Direction", 3);
                animator.speed = .7f;
            }
            if (direction == 2) {
                myRigidBody.velocity = new Vector2(0, MovementSpeed);
                animator.SetBool("Walking", true);
                animator.SetInteger("Direction", 0);
                animator.speed = .7f;
            }           
            if (direction == -2) {
                myRigidBody.velocity = new Vector2(0, -MovementSpeed);
                animator.SetBool("Walking", true);
                animator.SetInteger("Direction", 2);
                animator.speed = .7f;
            }
            

            if (WalkingCounter < 0) {
                Walking = false;

                animator.SetBool("Walking", false);

                WaitingCounter = TimeWaiting;
            }

        }
        else {
            WaitingCounter -= Time.deltaTime;

            myRigidBody.velocity = Vector2.zero;

            if (WaitingCounter < 0) {
                direction = direction * -1;

                Walking = true;

                WalkingCounter = TimeWalking;
            }
        }

        if (this.transform.position == lastPosition)
        {
            animator.SetBool("Walking", false);
            //MovementSpeed = 0;
            //animator.speed = 0.0f;
            //direction++;

            //animator.SetBool ("Walking", false);
            //WaitingCounter = TimeWaiting;
        }

        lastPosition = transform.position;
    }
}

