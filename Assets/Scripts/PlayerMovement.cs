using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour 
{
	#region Member Variables

	// timer for enemy encounters
	private float timeSinceLastBattle = 0.0f;
	private float encounterChance = 0.005f;

	private int health;

	// flag used for triggering enemy encounters
	public bool isMoving = false;

	//int for running
	//private int enableRun = 0;

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

	private bool isNPC = false;
	private GameObject currNPC;
	#endregion

	// Use this for initialization
	void Start ()
	{
		// get the local reference
		animator = GetComponent<Animator>();

		// set initial position
		lastPosition = transform.position;
		CheckPointPosition = transform.position;

		if (SceneManager.GetActiveScene ().name == "Prologue") {
			animator.SetInteger ("isDown", 1);
			movementSpeed = 0f;
		}
	}

	void Update(){
		if (isNPC == true && !GameManager.instance.inConversation && SceneManager.GetActiveScene ().name == "Tutorial scene") {
			// if not in conversation, enter one
			GameManager.instance.enterConversation (currNPC);
		} else if (Input.GetKeyDown (KeyCode.F) && isNPC == true && !GameManager.instance.inConversation && !GameManager.instance.inShop && SceneManager.GetActiveScene ().name != "Tutorial scene") {
			GameManager.instance.enterConversation (currNPC);
		}
	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		// enemy encounter logic
		// if enough time has passed
		if (timeSinceLastBattle >= 500f && SceneManager.GetActiveScene().name != "First Town (Kevin)" && SceneManager.GetActiveScene().name != "Port" && SceneManager.GetActiveScene().name != "Inn" && SceneManager.GetActiveScene().name != "Potion Shop" && SceneManager.GetActiveScene().name != "Path" && SceneManager.GetActiveScene().name != "Tutorial scene" && SceneManager.GetActiveScene().name != "RichHouse(Amar)"){
			// check for random encounter and not in safeZone ie.) Towns
			if (Random.Range (0.0f, 1.0f) <= encounterChance && isMoving && !GameManager.instance.inConversation) {
				StartCoroutine(GameManager.instance.EnemyEncounter());
				timeSinceLastBattle = 0f;
			}
		}


		if (isMoving == false) {
			animator.SetInteger("Direction",4);
		}

		// check for player exiting the game
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		float vertical = 0.0f;
		float horizontal = 0.0f;

		// get the input this frame if we can move
		if (GameManager.instance.allowMovement) {
			vertical = Input.GetAxis ("Vertical");
			horizontal = Input.GetAxis ("Horizontal");
		}

		// if there is no input then stop the animation
		if((horizontal == 0.0f)&&(vertical == 0.0f))
		{
			isMoving = false;
			animator.speed = 0.0f;
		}

		// reset the velocity each frame
		GetComponent<Rigidbody2D>().velocity =    new Vector2(0, 0);

		//check if "left shift" button is held down for run
		if (Input.GetKey (KeyCode.LeftShift) && isMoving == true) {
			animator.SetInteger ("isRunning", 1);
			movementSpeed = 155f;
		} else {
			animator.SetInteger("isRunning",0);
			movementSpeed = 75.0f;
		}

		// horizontal movement, left or right, set animation type and speed 
		if (horizontal > 0)
		{
			isMoving = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * Time.deltaTime,0);
			animator.SetInteger("Direction", 1);
			animator.speed = .7f;
			if (animator.GetInteger ("isRunning") == 1) {
				animator.speed = 1.5f;
			}
		}
		else if (horizontal < 0)
		{
			isMoving = true;
			GetComponent<Rigidbody2D>().velocity =    new Vector2(-movementSpeed * Time.deltaTime, 0);
			animator.SetInteger("Direction", 3);
			animator.speed = .7f;
			if (animator.GetInteger ("isRunning") == 1) {
				animator.speed = 1.5f;
			}
		}

		// vertical movement, up or down, set animation type and speed 
		if (vertical > 0) {
			isMoving = true;
			//transform.Translate(0, movementSpeed * 0.9f * Time.deltaTime, 0);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, movementSpeed * Time.deltaTime);
			animator.SetInteger ("Direction", 0);
			animator.speed = 1.5f;
			if (animator.GetInteger ("isRunning") == 1) {
				animator.speed = 1.1f;
			}
		} else if (vertical < 0) {
			isMoving = true;
			//transform.Translate(0, -movementSpeed *  0.9f * Time.deltaTime, 0);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -movementSpeed * Time.deltaTime);
			animator.SetInteger ("Direction", 2);
			animator.speed = 1.5f;
			if (animator.GetInteger ("isRunning") == 1) {
				animator.speed = 1.1f;
			}
		}

		//compare this position to the last known one, are we moving?
		if(this.transform.position == lastPosition)
		{
			isMoving = false;
			// we aren't moving so make sure we dont animate
			animator.speed = 0.0f;
			animator.SetInteger ("Direction", 4);
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
			// update target variables
			GameManager.instance.targetScene = collider.gameObject.GetComponent<TileChanger>().targetScene;
			GameManager.instance.targetPos = collider.gameObject.GetComponent<TileChanger>().targetPos;
			// heal the player to full if he enters the inn and trigger flag
			if (GameManager.instance.targetScene == "Inn") {
				GameManager.instance.setPlayerHP(GameManager.instance.playerMaxHP);
				if (GameManager.instance.hasVisitedInn == false) {
					GameManager.instance.hasVisitedInn = true;
				}
			}
			GameObject.Find("FadePanel").GetComponent<FadeScript>().FadeOut();
			isDead = true;
		}

		if(collider.gameObject.tag == "NPC")
		{
			isNPC = true;
			currNPC = collider.gameObject;
		}

	}


	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "NPC") {
			isNPC = false;
			currNPC = null;
			GameManager.instance.inConversation = false;
			GameManager.instance.shopOnEnd = false;
			if (GameManager.instance.inShop) {
				GameManager.instance.closeShop ();
			}
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
