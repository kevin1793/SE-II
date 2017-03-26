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

	// player values
	public int playerHealth;
	public Item[] playerInventory;

	public static GameManager instance = null;

	// UI goes here
	private Text playerHP;
	private Button enemyEncounter;

	public bool inBattle = false;

	// current scene (used to load back to the correct scene from battle scenes)
	private Scene sourceScene;
	private string sourceName;

	// current loc (used to load back to the correct location from other scenes)
	private Vector3 currentPos;
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

		// set player hp gui object
		playerHP = PersistentUI.instance.GetComponentInChildren<Text> ();
		enemyEncounter = PersistentUI.instance.GetComponentInChildren<Button> ();

		enemyEncounter.gameObject.SetActive (false);

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

		// update player pos if needed
		if (playerNeedsUpdate) {
			// update player pos
			GameObject player = GameObject.Find ("Hero");
			player.transform.position = currentPos;
			playerNeedsUpdate = false;
		}
	}

	public IEnumerator EnemyEncounter () {
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
		SceneManager.LoadScene("Battle Scene (Daniel)");
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
	}

	// public function to flag a position update to given position
	public void flagPosUpdate(Vector3 tPos) {
		currentPos = tPos;
		playerNeedsUpdate = true;
	}
}
