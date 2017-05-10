using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerMovement : MonoBehaviour {

	public float MovementSpeed;
	public float TimeWalking;
	public float TimeWaiting;

	public bool Walking;

    public int direction;
	private Animator animator;
	private float WalkingCounter;
	private float WaitingCounter;
	private Vector3 lastPosition;

    public Collider2D walkZone;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;

    private bool hasWalkZone = false;

    private Rigidbody2D myRigidBody;

	void Start () {
		
        myRigidBody = GetComponent<Rigidbody2D>();

		animator = GetComponent<Animator>();

		WaitingCounter = TimeWaiting;
		WalkingCounter = TimeWalking;

		lastPosition = transform.position;

        ChooseDirection();

        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }
	}

	void Update (){
	}

	void FixedUpdate () {
		if (Walking) {
            WalkingCounter -= Time.deltaTime;

            if (direction == 0 ) {
                myRigidBody.velocity = new Vector2(0, MovementSpeed);
				animator.SetBool ("Walking", true);
				animator.SetInteger ("Direction", 0);
				animator.speed = .7f;
                if (hasWalkZone && transform.position.y > maxWalkPoint.y)
                {
                    Walking = false;
                    animator.SetBool("Walking", false);
                    WaitingCounter = TimeWaiting;
                }
            }
            if (direction == 1) {
                myRigidBody.velocity = new Vector2(MovementSpeed, 0);
				animator.SetBool ("Walking", true);
				animator.SetInteger ("Direction", 1);
				animator.speed = .7f;
                if (hasWalkZone && transform.position.x > maxWalkPoint.x)
                {
                    Walking = false;
                    animator.SetBool("Walking", false);
                    WaitingCounter = TimeWaiting;
                }
            }
            if (direction == 2) {
                myRigidBody.velocity = new Vector2(0, -MovementSpeed);
				animator.SetBool ("Walking", true);
				animator.SetInteger ("Direction", 2);
				animator.speed = .7f;
                if (hasWalkZone && transform.position.y < minWalkPoint.y)
                {
                    Walking = false;
                    animator.SetBool("Walking", false);
                    WaitingCounter = TimeWaiting;
                }
            }
            if (direction == 3) {
                myRigidBody.velocity = new Vector2(-MovementSpeed, 0);
				animator.SetBool ("Walking", true);
				animator.SetInteger ("Direction", 3);
				animator.speed = .7f;
                if (hasWalkZone && transform.position.x < minWalkPoint.x)
                {
                    Walking = false;
                    animator.SetBool("Walking", false);
                    WaitingCounter = TimeWaiting;
                }
            }

            if (WalkingCounter < 0) {
                Walking = false;
				animator.SetBool ("Walking", false);
                WaitingCounter = TimeWaiting;
            }

		} 

		else {
			Walking = false;
			animator.SetBool ("Walking", false);
            WaitingCounter -= Time.deltaTime;

            myRigidBody.velocity = Vector2.zero;

            if (WaitingCounter < 0) {
                ChooseDirection();
            }
        }

		if (this.transform.position == lastPosition) {
			animator.SetBool ("Walking", false);
			//MovementSpeed = 0;
			//animator.speed = 0.0f;
			//direction++;

			//animator.SetBool ("Walking", false);
			//WaitingCounter = TimeWaiting;
		}

		lastPosition = transform.position;
	}

    public void ChooseDirection() {
        direction = Random.Range(0, 4);

        Walking = true;

        WalkingCounter = TimeWalking;
    }
}
