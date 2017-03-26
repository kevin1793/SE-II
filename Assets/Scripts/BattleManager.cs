// Author: Daniel Berg
// Date: 3/24/2017
// Description: BattleManager class used to hold load and perform battles
//	

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	// prefabs/scene-specific vars
	public Canvas BattleHUD;
	public GameObject[] monsters;
	public Slider healthbar;
	public Vector3[] spawnPoints;

	// healthbars
	private Slider playerHealth;
	private Slider monsterHealth;
	RectTransform healthTransform;

	// monster(s)
	private GameObject monster;
	private BattleMonster bmonster;

	// player animation controller
	private Animator playerAnim;

	// vars for battle loop
	private bool playerTurn = true;
	private bool isDead = false;
	private bool isAttacking = false;
	private bool isDefending = false;
	private bool tDelay = false;
	private int enemiesRemaining = 0;

	private GameObject battleUI;
	private Button attack;
	private Button defend;
	private Button run;

	// Use this for initialization
	void Start () {
		// set GameManager state to in-battle
		GameManager.instance.inBattle = true;

		// set the battleUI components (buttons)
		battleUI = GameObject.Find("BattleUI");
		attack = GameObject.Find ("Attack").GetComponent<Button> ();
		defend = GameObject.Find ("Defend").GetComponent<Button> ();
		run = GameObject.Find ("Run").GetComponent<Button> ();
		attack.onClick.AddListener (AttackListener);
		defend.onClick.AddListener (DefendListener);
		run.onClick.AddListener (RunListener);

		// set the player animator
		playerAnim = GameObject.Find("BattleHero").GetComponent<Animator> ();

		// spawn the monster(s)
		monster = Instantiate(monsters[0]) as GameObject;
		bmonster = monster.GetComponent<BattleMonster> ();

		// set their position
		SpriteRenderer monsterRender = monster.GetComponent<SpriteRenderer> ();
		monster.transform.position = spawnPoints [0];
		monsterRender.sortingLayerName = "Player";
		monsterRender.flipX = true;

		// spawn monster(s) healthbar(s)
		monsterHealth = Instantiate (healthbar) as Slider;
		monsterHealth.transform.SetParent (BattleHUD.transform, false);
		// set healthbar value to spawned monster's max health
		monsterHealth.maxValue = monster.GetComponent<BattleMonster> ().monsterHealth;
		monsterHealth.value = monster.GetComponent<BattleMonster> ().monsterHealth;

		// set monster healthbar position(s)
		healthTransform = monsterHealth.GetComponent<RectTransform> ();
		healthTransform.anchorMin = new Vector2 (1f, 0.5f);
		healthTransform.anchorMax = new Vector2 (1f, 0.5f);
		healthTransform.anchoredPosition = new Vector2 (-160f, -25f);

		// set enemiesRemaining
		enemiesRemaining = 1;

		// spawn the player healthbar
		playerHealth = Instantiate (healthbar) as Slider;
		playerHealth.transform.SetParent (BattleHUD.transform, false);
		// set the healthbar value to player's current health
		playerHealth.maxValue = GameManager.instance.playerHealth;
		playerHealth.value = GameManager.instance.playerHealth;

		// set player healthbar position
		healthTransform = playerHealth.GetComponent<RectTransform> ();
		healthTransform.anchorMin = new Vector2 (0f, 0.5f);
		healthTransform.anchorMax = new Vector2 (0f, 0.5f);
		healthTransform.anchoredPosition = new Vector2 (0, -25);
	}
	
	// Update is called once per frame
	void Update () {
		// battle logic here

		// loop while you are alive and have enemies (and nobody is attacking)
		if (enemiesRemaining > 0 && !isDead && !tDelay) {
			if (playerTurn) {
				battleUI.SetActive (true);
			} else {
				monsterAttack ();
			}
		} else if (isDead || enemiesRemaining == 0) {
			GameManager.instance.endBattle ();
		}
	}

	// button listener classes for BattleHUD
	void AttackListener () {
		playerAttack ();
		battleUI.SetActive (false);
	}

	void DefendListener () {
		playerDefend ();
		battleUI.SetActive (false);
	}

	void RunListener () {
		GameManager.instance.endBattle ();
	}

	// player attack
	void playerAttack() {
		playerAnim.SetTrigger ("onAttack");

		if (monsterHealth.value >= 75) {
			monsterHealth.value -= 75;
		} else {
			monsterHealth.value = 0;
		}

		if (monsterHealth.value == 0) {
			monsterHealth.gameObject.SetActive (false);
			monster.SetActive (false);
			enemiesRemaining = 0;
		}

		isAttacking = true;
		bmonster.monstOnHit ();
		StartCoroutine (turnDelay());
		playerTurn = false;
	}

	// player defend
	void playerDefend() {
		isDefending = true;
		playerAnim.SetBool ("isDefending", true);
		StartCoroutine (turnDelay());
		playerTurn = false;
	}

	// monster attack
	void monsterAttack() {

		if (isDefending) {
			if (playerHealth.value >= 15) {
				playerHealth.value -= 15;
			} else {
				playerHealth.value = 0;
			}
		} else {
			if (playerHealth.value >= 30) {
				playerHealth.value -= 30;
			} else {
				playerHealth.value = 0;
			}
		}

		// update player health value in gamemanager
		GameManager.instance.playerHealth = (int)playerHealth.value;

		if (playerHealth.value == 0) {
			isDead = true;
		}

		bmonster.monstAttack ();
		playerAnim.SetTrigger ("onHit");

		playerTurn = true;
		isDefending = false;
		playerAnim.SetBool ("isDefending", false);
	}

	// add a slight delay in-between actions for animations to complete
	IEnumerator turnDelay() {
		tDelay = true;
		yield return new WaitForSeconds (1.35f);
		isAttacking = false;
		tDelay = false;
	}
}
