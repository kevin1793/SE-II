// Author: Daniel Berg
// Date: 2/24/2017
// Description: Singleton GameManager class used to hold player data and other values
//				that need to persist between scenes
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// vars for scene switching
	public string targetScene;
	public Vector3 targetPos;

	// dialogue
	private TextBoxManager dialogueManager;

	// player values
	public int playerHealth;
	public Item[] playerInventory;

	public static GameManager instance = null;

	// UI goes here
	private Text playerHP;
	private Button enemyEncounter;
	public GameObject dBox;

	// flags
	public bool inBattle = false;
	public bool allowMovement = true;
	public bool inConversation = false;

	// current scene (used to load back to the correct scene from battle scenes)
	private Scene sourceScene;
	private string sourceName;

	// current loc (used to load back to the correct location from other scenes)
	public Vector3 currentPos;
	private bool playerNeedsUpdate = false;


	// Use this for initialization
	void Awake () {

		// if no GameManager exists, set this as the GameManager
		if (instance == null) {
			instance = this;
		}
		// if this GameManager is not the initial instance, destroy it
		else if (instance != this) {
			Destroy (gameObject);
		}

		// set gui and dialogue
		playerHP = PersistentUI.instance.GetComponentInChildren<Text> ();
		enemyEncounter = PersistentUI.instance.GetComponentInChildren<Button> ();
		dBox = GameObject.Find ("dialogueBox");
		dialogueManager = FindObjectOfType<TextBoxManager> ();

		enemyEncounter.gameObject.SetActive (false);

		// toggle health ui on/off if in a battle
		if (inBattle) {
			playerHP.gameObject.SetActive (false);
		} else {
			playerHP.gameObject.SetActive (true);
		}

		// init scene info
		sourceScene = SceneManager.GetActiveScene();
		sourceName = sourceScene.name;

		// preserve this GameManager instance when loading new scene
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {

		// update player hp (to be replaced with a function call later)
		playerHP.text = "Health: " + playerHealth;

		// if in battle, make sure you aren't displaying health UI
		if(inBattle) {
			playerHP.gameObject.SetActive(false);
		}

		// if dialogue manager isn't set, try to set it
		if (dialogueManager == null) {
			dialogueManager = FindObjectOfType<TextBoxManager> ();
		}

		// if dbox wasn't set, set it
		if (dBox == null) {
			dBox = GameObject.Find ("dialogueBox");
		}

		// if not talking, set dBox to inactive
		if (!inConversation) {
			dBox.gameObject.SetActive (false);
		}

		// update player pos if needed
		if (playerNeedsUpdate) {
			GameObject player = GameObject.Find ("Hero");
			// if we can find the player, update
			if (player != null) {
				// update player pos
				player.transform.position = currentPos;
				// if we successfully updated
				if (GameObject.Find ("Hero").transform.position == currentPos) {
					playerNeedsUpdate = false;
				}
			}
			// else try again next frame
		}
	}

	// function for encountering an enemy
	public IEnumerator EnemyEncounter () {
		allowMovement = false;
		// flash enemy encounter UI element
		enemyEncounter.gameObject.SetActive(true);
		yield return new WaitForSeconds (1.5f);
		enemyEncounter.gameObject.SetActive (false);

		// update source scene
		sourceScene = SceneManager.GetActiveScene();
		sourceName = sourceScene.name;

		// update player pos
		currentPos = GameObject.Find("Hero").transform.position;

		// move to battle scene
		SceneManager.LoadScene("Battle Scene");
	}

	// function to enter a conversation
	public void enterConversation(GameObject t) {
		//inConversation = true;
		dBox.gameObject.SetActive (true);
		inConversation = true;
		dialogueManager.setDialogue (t.GetComponent<DialogueText> ().dialogue);
	}

	// function used to move the player from a battle scene back to original scene
	public void endBattle () {
		// update inbattle bool
		inBattle = false;

		// load the scene
		SceneManager.LoadScene(sourceName);

		// reactivate health ui
		playerHP.gameObject.SetActive(true);

		// flag gamemanager to update player pos
		playerNeedsUpdate = true;

		allowMovement = true;
	}

	// function to reset the game on player death
	public void resetGame() {
		inBattle = false;

		// switch to the main menu scene
		SceneManager.LoadScene ("MainMenu");

		// reset the player health, UI, and location
		playerHP.gameObject.SetActive (true);
		playerHealth = 200;
		currentPos = new Vector3 (3.71f, 3.03f, 0);
		allowMovement = true;
	}

	// public function to flag a position update to given position
	public void flagPosUpdate(Vector3 tPos) {
		currentPos = tPos;
		playerNeedsUpdate = true;
	}
}
