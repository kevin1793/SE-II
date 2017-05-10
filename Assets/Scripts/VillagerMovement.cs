using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerMovement : MonoBehaviour {

	public float MovementSpeed;
	public float TimeWalking;
	public float TimeWaiting;

	public bool Walking;

    private int direction;

	private float WalkingCounter;
	private float WaitingCounter;

    private Rigidbody2D myRigidBody;

	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();

		WaitingCounter = TimeWaiting;
		WalkingCounter = TimeWalking;

        ChooseDirection();
	}

	void Update () {
        if (Walking) {
            WalkingCounter -= Time.deltaTime;

            if (direction == 0) {
                myRigidBody.velocity = new Vector2(0, MovementSpeed);
            }
            if (direction == 1) {
                myRigidBody.velocity = new Vector2(MovementSpeed, 0);
            }
            if (direction == 2) {
                myRigidBody.velocity = new Vector2(0, -MovementSpeed);
            }
            if (direction == 3) {
                myRigidBody.velocity = new Vector2(-MovementSpeed, 0);
            }

            if (WalkingCounter < 0) {
                Walking = false;

                WaitingCounter = TimeWaiting;
            }

        } else {
            WaitingCounter -= Time.deltaTime;

            myRigidBody.velocity = Vector2.zero;

            if (WaitingCounter < 0) {
                ChooseDirection();
            }
        }		
	}

    public void ChooseDirection() {
        direction = Random.Range(0, 4);

        Walking = true;

        WalkingCounter = TimeWalking;
    }
}
