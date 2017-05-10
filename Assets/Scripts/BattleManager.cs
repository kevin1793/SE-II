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
	private Slider[] monsterHealth = new Slider[3];
	//private Slider monsterHealth2;
	//private Slider monsterHealth3;
	RectTransform healthTransform;
	HealthbarText batHealth;

	// monster(s) max of 3
	private GameObject[] monster = new GameObject[3];
	private BattleMonster[] bmonster = new BattleMonster[3];

	// player animation controller
	private Animator playerAnim;

	// vars for battle loop
	private bool playerTurn = true;
	private bool isDead = false;
	private bool isAttacking = false;
	private bool isDefending = false;
	private bool tDelay = false;
	private int enemiesRemaining = 0;

	// UI
	private GameObject battleUI;
	private Button attack;
	private Button defend;
	private Button run;
	private GameObject attackText;

	private AudioSource[] BattleFX;
	private AudioSource swordSlice;
	private AudioSource monsterAtt;

	private AudioSource[] BattleMusic;
	private AudioSource Fanfare;
	private AudioSource Music;

	public static bool pause = false;
	private bool battleEnding = false;

	// Use this for initialization
	void Start () {
		// getting audio clip for attack
		AudioSource[] BattleMusic = GameObject.Find ("Battle Music").GetComponents<AudioSource> ();
		Music = BattleMusic [0];
		Fanfare = BattleMusic[1];

		AudioSource[] BattleFX = GameObject.Find ("BattleFX").GetComponents<AudioSource> ();
		swordSlice = BattleFX [0];
		monsterAtt = BattleFX[1];

		// set GameManager state to in-battle
		GameManager.instance.inBattle = true;

		// set the battleUI components (buttons)
		battleUI = GameObject.Find ("BattleUI");
		attack = GameObject.Find ("Attack").GetComponent<Button> ();
		defend = GameObject.Find ("Defend").GetComponent<Button> ();
		run = GameObject.Find ("Run").GetComponent<Button> ();
		attackText = GameObject.Find ("InstructionText");
		attack.onClick.AddListener (AttackListener);
		defend.onClick.AddListener (DefendListener);
		run.onClick.AddListener (RunListener);
		attackText.SetActive (false);

		// set the player animator
		playerAnim = GameObject.Find("BattleHero").GetComponent<Animator> ();

		// spawn the monster(s)

		// generate random amount of enemies to spawn (1-3)
		int randomEnemies = Random.Range(1, 4);

		// loop and spawn the monsters
		for (int x = 0; x < randomEnemies; x++) {
			// generate a random monster to spawn from monsters array
			int randomMonster = Random.Range(0, monsters.Length);
			monster[x] = Instantiate(monsters[randomMonster]) as GameObject;
			bmonster[x] = monster[x].GetComponent<BattleMonster> ();

			// set their position
			SpriteRenderer monsterRender = monster[x].GetComponent<SpriteRenderer> ();
			monster[x].transform.position = spawnPoints [x];
			monsterRender.sortingLayerName = "Player";
			monsterRender.flipX = true;

			// spawn monster(s) healthbar(s)
			monsterHealth[x] = Instantiate (healthbar) as Slider;
			monsterHealth[x].transform.SetParent (BattleHUD.transform, false);
			// set healthbar value to spawned monster's max health
			monsterHealth[x].maxValue = monster[x].GetComponent<BattleMonster> ().monsterHealth;
			monsterHealth[x].value = monster[x].GetComponent<BattleMonster> ().monsterHealth;
			// set monster id and healthbar id to match
			batHealth = monsterHealth[x].GetComponent<HealthbarText>();
			batHealth.setId(x);
			bmonster [x].monsterID = x;

			// set monster healthbar position(s)
			healthTransform = monsterHealth[x].GetComponent<RectTransform> ();
			if (x == 0) {
				healthTransform.anchorMin = new Vector2 (1f, 0.5f);
				healthTransform.anchorMax = new Vector2 (1f, 0.5f);
				healthTransform.anchoredPosition = new Vector2 (-160f, -25f);
			} else if (x == 1) {
				healthTransform.anchorMin = new Vector2 (1f, 0.75f);
				healthTransform.anchorMax = new Vector2 (1f, 0.75f);
				healthTransform.anchoredPosition = new Vector2 (-160f, -37.5f);
			} else if (x == 2) {
				healthTransform.anchorMin = new Vector2 (1f, 0.25f);
				healthTransform.anchorMax = new Vector2 (1f, 0.25f);
				healthTransform.anchoredPosition = new Vector2 (-160f, -12.5f);
			}
		}

		// set enemiesRemaining to amount of enemies spawned
		enemiesRemaining = randomEnemies;

		// spawn the player healthbar
		playerHealth = Instantiate (healthbar) as Slider;
		playerHealth.transform.SetParent (BattleHUD.transform, false);
		// set the healthbar value to player's current health
		playerHealth.maxValue = 200;
		playerHealth.value = GameManager.instance.playerHealth;

		// set player healthbar position
		healthTransform = playerHealth.GetComponent<RectTransform> ();
		healthTransform.anchorMin = new Vector2 (0f, 0.5f);
		healthTransform.anchorMax = new Vector2 (0f, 0.5f);
		healthTransform.anchoredPosition = new Vector2 (0, -25);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// battle logic here

		// loop while you are alive and have enemies (and nobody is attacking)
		if (enemiesRemaining > 0 && !isDead && !tDelay && !isAttacking) {
			if (playerTurn) {
				battleUI.SetActive (true);
				attack.gameObject.SetActive (true);
				defend.gameObject.SetActive (true);
				run.gameObject.SetActive (true);
			} else {
				monsterAttack ();
			}
		} else if (isAttacking) {
			// if waiting for the player to select a target, check for a target
			checkForAttack ();
		} else if (enemiesRemaining == 0 && !isAttacking && !battleEnding) {
			battleEnding = true;
			Music.Stop ();
			Fanfare.Play ();
			StartCoroutine("endBattle");
		} else if (isDead) {
			GameManager.instance.resetGame ();
		}
	}

	// button listener classes for BattleHUD
	void AttackListener () {
		isAttacking = true;
		attack.gameObject.SetActive (false);
		defend.gameObject.SetActive (false);
		run.gameObject.SetActive (false);
		attackText.SetActive (true);
	}

	void DefendListener () {
		playerDefend ();
		battleUI.SetActive (false);
	}

	void RunListener () {
		GameManager.instance.endBattle ();
	}

	// function for detecting a player attack
	void checkForAttack() {
		// on mouse down
		if(Input.GetMouseButtonDown(0)) {
			// check if one of the monsters was clicked
			RaycastHit2D hit = Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0);

			// if so, perform attack on the monster
			if(hit.collider.gameObject.tag == "BattleMonster") {
				BattleMonster m = hit.collider.gameObject.GetComponent<BattleMonster>();
				playerAnim.SetTrigger ("onAttack");
				attackText.SetActive (false);
				battleUI.SetActive (false);
				StartCoroutine (attackDelay (m.monsterID));
			}
		}
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
		
		// loop through all the monsters and have each attack
		foreach (BattleMonster monst in bmonster) {
			// make sure the monster exists
			if (monst != null) {
				// make sure it isn't a dead monster
				if (monster [monst.monsterID].gameObject.activeSelf) {
					monsterAtt.Play ();

					if (isDefending) {
						if (playerHealth.value >= monst.monsterStrength / 2) {
							playerHealth.value -= monst.monsterStrength / 2;
						} else {
							playerHealth.value = 0;
						}
					} else {
						if (playerHealth.value >= monst.monsterStrength) {
							playerHealth.value -= monst.monsterStrength;
						} else {
							playerHealth.value = 0;
						}
					}

					// update player health value in gamemanager
					GameManager.instance.playerHealth = (int)playerHealth.value;

					if (playerHealth.value == 0) {
						isDead = true;

					}

					monst.monstAttack ();
					playerAnim.SetTrigger ("onHit");
				}
			}
		}
	
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

	IEnumerator attackDelay(int targetMonster){

		if (monsterHealth[targetMonster].value >= 75) {
			yield return new WaitForSeconds (0.3f);
			swordSlice.Play ();
			yield return new WaitForSeconds (0.2f);
			bmonster[targetMonster].monstOnHit ();
			monsterHealth[targetMonster].value -= 75;
			yield return new WaitForSeconds (1.0f);
		} else if (monsterHealth[targetMonster].value <= 75) {
			enemiesRemaining--;
			yield return new WaitForSeconds (0.3f); // time needed for hit Sound to be in
			swordSlice.Play ();
			yield return new WaitForSeconds (0.2f);
			bmonster[targetMonster].monstOnHit ();
			monsterHealth[targetMonster].value = 0;
			yield return new WaitForSeconds (1.0f); //for last hit to make contact
			monsterHealth[targetMonster].gameObject.SetActive (false);
			monster[targetMonster].SetActive (false);
		}

		playerTurn = false;
		isAttacking = false;
	}

	IEnumerator endBattle() {
		yield return new WaitForSeconds (5f); //time for victory music
		GameManager.instance.endBattle ();
	}
}