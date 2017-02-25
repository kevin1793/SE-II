using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	#region Member Variables
	// timer for enemy encounters
	private float timeSinceLastBattle = 0.0f;
	private float encounterChance = 0.15f;

	private int health;

	/// <summary>
	/// Player movement speed
	/// </summary>
	public float movementSpeed = 75.0f;

	/// <summary>
	/// Animation state machine local reference
	/// </summary>
	private Animator animator;

	/// <summary>
	/// The last position of the player in previous frame
	/// </summary>
	private Vector3 lastPosition;

	/// <summary>
	/// The last checkpoint position that we have saved
	/// </summary>
	private Vector3 CheckPointPosition;

	/// <summary>
	/// Is the player dead?
	/// </summary>
	private bool isDead = false;
	#endregion

	// Use this for initialization
	void Start ()
	{
		// get the local reference
		animator = GetComponent<Animator>();

		// set initial position
		lastPosition = transform.position;
		CheckPointPosition = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		// enemy encounter logic
		// if enough time has passed
		if (timeSinceLastBattle >= 500f) {
			// check for random encounter
			if (Random.Range (0.0f, 1.0f) <= encounterChance) {
				StartCoroutine(GameManager.instance.EnemyEncounter());
				timeSinceLastBattle = 0f;
			}
		}


		// check for player exiting the game
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		// get the input this frame
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		// if there is no input then stop the animation
		if((horizontal == 0.0f)&&(vertical == 0.0f))
		{
			animator.speed = 0.0f;
		}

		// reset the velocity each frame
		GetComponent<Rigidbody2D>().velocity =    new Vector2(0, 0);

		// horizontal movement, left or right, set animation type and speed 
		if (horizontal > 0)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * Time.deltaTime,0);
			animator.SetInteger("Direction", 1);
			animator.speed = .7f;
		}
		else if (horizontal < 0)
		{
			GetComponent<Rigidbody2D>().velocity =    new Vector2(-movementSpeed * Time.deltaTime, 0);
			animator.SetInteger("Direction", 3);
			animator.speed = .7f;
		}

		// vertical movement, up or down, set animation type and speed 
		if (vertical > 0)
		{
			//transform.Translate(0, movementSpeed * 0.9f * Time.deltaTime, 0);
			GetComponent<Rigidbody2D>().velocity =    new Vector2(0, movementSpeed * Time.deltaTime);
			animator.SetInteger("Direction", 0);
			animator.speed = 1.5f;
		}
		else if (vertical < 0)
		{
			//transform.Translate(0, -movementSpeed *  0.9f * Time.deltaTime, 0);
			GetComponent<Rigidbody2D>().velocity =    new Vector2(0, -movementSpeed * Time.deltaTime);
			animator.SetInteger("Direction", 2);
			animator.speed = 1.5f;
		}

		//compare this position to the last known one, are we moving?
		if(this.transform.position == lastPosition)
		{
			// we aren't moving so make sure we dont animate
			animator.speed = 0.0f;
		}

		// get the last known position
		lastPosition = transform.position;

		//increment time
		timeSinceLastBattle++;

		// if we are dead do not move anymore
		if(isDead == true)
		{
			GetComponent<Rigidbody2D>().velocity =    new Vector2(0.0f, 0.0f);
			animator.speed = 0.0f;
		}

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "DangerousTile")
		{
			GameManager.instance.playerHealth -= 100;
		}
		else if(collider.gameObject.tag == "LevelChanger")
		{
			GameObject.Find("FadePanel").GetComponent<FadeScript>().FadeOut();
			isDead = true;
		}
	}

	/// <summary>
	/// Respawns the player at checkpoint.
	/// </summary>
	public void RespawnPlayerAtCheckpoint()
	{
		// if we hit a dangerous tile then we are dead so go to the checkpoint position that was last saved
		transform.position = CheckPointPosition;
		isDead = false;
	}

}
