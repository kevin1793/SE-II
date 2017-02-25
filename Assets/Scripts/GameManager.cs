// Author: Daniel Berg
// Date: 2/24/2017
// Description: Singleton GameManager class used to hold player data and other values
//				that need to persist between scenes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// player values
	public int playerHealth;
	public Item[] playerInventory;

	public static GameManager instance = null;

	// UI goes here
	private Text playerHP;
	private Button enemyEncounter;


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
		playerHP = PersistentUI.instance.GetComponentInChildren<Text>();
		enemyEncounter = PersistentUI.instance.GetComponentInChildren<Button> ();
		enemyEncounter.gameObject.SetActive(false);

		// preserve this GameManager instance when loading new scene
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		// update player hp (to be replaced with a function call later)
		playerHP.text = "Health: " + playerHealth;
	}

	public IEnumerator EnemyEncounter () {
		enemyEncounter.gameObject.SetActive(true);
		playerHealth -= 5;
		yield return new WaitForSeconds (3);
		enemyEncounter.gameObject.SetActive (false);
	}
}
