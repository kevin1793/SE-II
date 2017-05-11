// Author: Daniel Berg
// Date: 2/24/2017
// Description: Singleton GameManager class used to hold player data and other values
//				that need to persist between scenes, and for performing game tasks
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
	public int playerMaxHP;
	public int playerG;
	public InventorySlot selectedItem;
	public int equipWeaponDamage = 75;
	public string equipWeaponName = "";

	public static GameManager instance = null;

	// UI goes here
	private Text playerHP;
	private Button enemyEncounter;
	private Text playerGold;
	private GameObject inventoryObj;
	private GameObject[] playerInventory = new GameObject[20];
	private GameObject[] tempInv = new GameObject[20];
	private GameObject itemSelect;
	private Button useButton;
	private Button dropButton;
	private Button equipButton;
	public GameObject dBox;
	private Text currentItem;
	private Text equippedWeapon;
	private Toggle battleToggler;
	public GameObject fnotify;

	// other vars
	private GameObject currentDialogue;
	private Shop currShop;
	private ObtainableItem currItem;
	private GameObject swordsman;

	// flags
	public bool inBattle = false;
	public bool allowMovement = true;
	public bool allowBattle = false;
	public bool battleToggleOverride = true;
	public bool inConversation = false;
	public bool swordsmanBattle = false;
	public bool shopOnEnd = false;
	public bool inShop = false;
	public bool getItemOnEnd = false;
	public bool dsSword = false;
	public bool firstConvo = true;
	public bool hasVisitedInn = false;
	public bool NPCJoinOnEnd = false;
	public bool inTutorial = false;
	public bool inPrologue = false;
	public bool finalBattleTriggered = false;
	public bool finalBossDefeated = false;

	// current scene (used to load back to the correct scene from battle scenes)
	private Scene sourceScene;
	private string sourceName;

	// current loc (used to load back to the correct location from other scenes)
	public Vector3 currentPos;
	private bool playerNeedsUpdate = false;

	// -
	private bool oneTimeEvents = true;


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
		battleToggler = PersistentUI.instance.transform.Find("battleToggler").gameObject.GetComponent<Toggle>();
		playerHP = PersistentUI.instance.transform.Find("playerHealth").gameObject.GetComponent<Text>();
		playerHP.text = "Health: " + playerHealth + "/" + playerMaxHP;
		equippedWeapon = PersistentUI.instance.transform.Find("playerWeapon").gameObject.GetComponent<Text>();
		equippedWeapon.text = "Weapon: None +" + equipWeaponDamage;
		enemyEncounter = PersistentUI.instance.transform.Find("enemyEncounter").gameObject.GetComponent<Button>();
		inventoryObj = PersistentUI.instance.transform.Find ("playerInventory").gameObject;
		itemSelect = PersistentUI.instance.transform.Find ("itemSelect").gameObject;
		fnotify = PersistentUI.instance.transform.Find ("FNotify").gameObject;
		currentItem = itemSelect.transform.Find ("selectedItem").gameObject.GetComponent<Text> ();
		useButton = itemSelect.transform.Find ("itemInteract/useButton").gameObject.GetComponent<Button> ();
		useButton.onClick.AddListener (useListener);
		dropButton = itemSelect.transform.Find ("itemInteract/dropButton").gameObject.GetComponent<Button> ();
		dropButton.onClick.AddListener (dropListener);
		equipButton = itemSelect.transform.Find ("itemInteract/equipButton").gameObject.GetComponent<Button> ();
		equipButton.onClick.AddListener (equipListener);
		playerGold = inventoryObj.GetComponentInChildren<Text> ();
		playerG = 0;
		tempInv = GameObject.FindGameObjectsWithTag ("InventorySlot");
		// put slots in proper places in playerinventory
		foreach (GameObject g in tempInv) {
			playerInventory [g.GetComponent<InventorySlot> ().slotIndex] = g;
		}

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
		if (SceneManager.GetActiveScene ().name == "Prologue" || SceneManager.GetActiveScene ().name == "Prologue2" || SceneManager.GetActiveScene ().name == "Prologue3" || SceneManager.GetActiveScene ().name == "PreFinalBattle") {
			inPrologue = true;
			allowMovement = false;
		} else {
			inPrologue = false;
		}
		if (SceneManager.GetActiveScene ().name == "Tutorial scene") {
			inTutorial = true;
			if (firstConvo) {
				allowMovement = true;
			} else {
				allowMovement = false;
			}
		} else {
			inTutorial = false;
		}

		if (inPrologue || inTutorial || inBattle || SceneManager.GetActiveScene ().name == "Exodus" || SceneManager.GetActiveScene ().name == "Cave 2" || SceneManager.GetActiveScene ().name == "First Town (Kevin)" || SceneManager.GetActiveScene ().name == "Port" || SceneManager.GetActiveScene ().name == "Inn" || SceneManager.GetActiveScene ().name == "Potion Shop" || SceneManager.GetActiveScene ().name == "Path" || SceneManager.GetActiveScene ().name == "Tutorial scene" || SceneManager.GetActiveScene ().name == "RichHouse(Amar)" || SceneManager.GetActiveScene ().name == "PoorHouse(Amar)" || SceneManager.GetActiveScene ().name == "Library(Amar)") {
			allowBattle = false;
			battleToggler.gameObject.SetActive (false);
		} else {
			allowBattle = true;
			battleToggler.gameObject.SetActive (true);
		}

		if (oneTimeEvents) {
			inventoryObj.SetActive (false);
			itemSelect.SetActive (false);
			fnotify.SetActive (false);
			oneTimeEvents = false;
		}

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

		// if inventory is closed, make sure item selection is closed
		if (inventoryObj.activeSelf == false && itemSelect.activeSelf) {
			itemSelect.SetActive (false);
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

		// toggle inventory visibility on 'I' when not in battle or conversation
		if (Input.GetKeyUp (KeyCode.I) && !inBattle && !inConversation) {
			inventoryObj.SetActive (!inventoryObj.activeSelf);
		}
	}

	// function for encountering an enemy
	public IEnumerator EnemyEncounter ()
	{
		allowMovement = false;
		// flash enemy encounter UI element
		enemyEncounter.gameObject.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		enemyEncounter.gameObject.SetActive (false);

		// disable inventory
		inventoryObj.SetActive (false);

		// update source scene
		sourceScene = SceneManager.GetActiveScene ();
		sourceName = sourceScene.name;

		// update player pos
		currentPos = GameObject.Find ("Hero").transform.position;

		// move to battle scene
		if (sourceName == "Island" || sourceName == "Island 2") {
			SceneManager.LoadScene ("Battle Scene Island");
		} else if (sourceName == "Cave" || sourceName == "Cave1") {
			SceneManager.LoadScene ("Battle Scene Cave");
		} else if (sourceName == "Cave 2") {
			SceneManager.LoadScene ("Final Battle Scene");
		}
		else if (sourceName == "Tresmuertes") {
			SceneManager.LoadScene ("Deserted Battle Scene");
		}else {
			SceneManager.LoadScene ("Battle Scene");
		}

		battleToggler.gameObject.SetActive (false);
	}

	// function to enter a conversation
	public void enterConversation(GameObject t) {
		if (inTutorial) {
			currentDialogue = t;
			firstConvo = false;
		}
		dBox.gameObject.SetActive (true);
		inConversation = true;
		dialogueManager.setDialogue (t.GetComponent<DialogueText> ().dialogue);
		if (t.GetComponent<Shop> ()) {
			currShop = t.GetComponent<Shop> ();
			shopOnEnd = true;
		}
		if (t.GetComponent<ObtainableItem> ()) {
			currItem = t.GetComponent<ObtainableItem> ();
			if (dsSword) {
				string[] newDial = t.GetComponent<ObtainableItem> ().altDialogue.text.Split ('\n');
				dialogueManager.setDialogue (newDial);
			} else {
				getItemOnEnd = true;
			}
		}
		if (t.GetComponent<PartyMember> () && hasVisitedInn) {
			if (swordsmanBattle) {
				inConversation = false;
				dBox.gameObject.SetActive (false);
			} else {
				string[] newDial = t.GetComponent<PartyMember> ().joinText.text.Split ('\n');
				dialogueManager.setDialogue (newDial);
				swordsman = t;
				NPCJoinOnEnd = true;
			}
		}
	}

	// function used to move the player from a battle scene back to original scene
	public void endBattle () {
		// update inbattle bool
		inBattle = false;

		// load the scene
		SceneManager.LoadScene(sourceName);

		// update player hp
		playerHP.text = "Health: " + playerHealth + "/" + playerMaxHP;

		// reactivate health ui
		playerHP.gameObject.SetActive(true);

		// reactive other ui
		battleToggler.gameObject.SetActive (true);

		// flag gamemanager to update player pos
		playerNeedsUpdate = true;

		allowMovement = true;

		inConversation = false;
	}

	// function to reset the game on player death
	public void resetGame() {
		inBattle = false;

		// switch to the main menu scene
		SceneManager.LoadScene ("MainMenu");

		// reset the player health, UI, and location
		playerHP.gameObject.SetActive (true);
		playerHealth = 250;
		playerHP.text = "Health: " + playerHealth + "/" + playerMaxHP;
		currentPos = new Vector3 (3.71f, 3.03f, 0);
		allowMovement = true;

		// reset player inventory
		foreach (GameObject item in playerInventory) {
			item.GetComponent<InventorySlot> ().clearSlot ();
		}
	}

	// public function to flag a position update to given position
	public void flagPosUpdate(Vector3 tPos) {
		currentPos = tPos;
		playerNeedsUpdate = true;
		allowMovement = true;
	}

	// add item i to player inventory
	public void addItemToInventory(Item i) {
		InventorySlot slot;

		// loop through inventory
		for (int x = 0; x < playerInventory.Length; x++) {
			slot = playerInventory [x].GetComponent<InventorySlot> ();

			// add item to the first empty slot
			if (slot.isEmpty) {
				slot.setItem (i);
				return;
			}
			// else if item is stackable and we found the matching item
			else if (i.stackable && slot.getId () == i.itemId) {
				// add 1 to the slot stack
				slot.addStackable();
				return;
			}
		}
	}

	// add amount of gold to player inventory
	public void addGoldToInventory(int amount) {
		playerG += amount;
		playerGold.text = "" + playerG;
	}

	// set player HP to a given value
	public void setPlayerHP(int hp) {
		playerHealth = hp;
		playerHP.text = "Health: " + playerHealth + "/" + playerMaxHP;
	}

	void useListener() {
		itemSelect.SetActive (false);
		// if health potion
		if (selectedItem.getId () == 0 || selectedItem.getId() == 6) {
			playerHealth += selectedItem.getSlotItem ().itemValue;

			// make sure player doesn't go over maxhp
			if (playerHealth > playerMaxHP) {
				playerHealth = playerMaxHP;
			}

			// update player hp text
			playerHP.text = "Health: " + playerHealth + "/" + playerMaxHP;

			// check if that was the last usable
			if (selectedItem.getAmount () <= 1) {
				// if so, clear the slot
				selectedItem.clearSlot ();
				updateInventory (selectedItem.slotIndex);
			} else {
				// else reduce the amount by 1
				selectedItem.removeStackable ();
			}
		} 
		// if mana potion (heal half for now)
		else if (selectedItem.getId () == 1) {
			playerHealth += selectedItem.getSlotItem ().itemValue / 2;

			// make sure player doesn't go over maxhp
			if (playerHealth > playerMaxHP) {
				playerHealth = playerMaxHP;
			}

			// update player hp text
			playerHP.text = "Health: " + playerHealth + "/" + playerMaxHP;

			// check if that was the last usable
			if (selectedItem.getAmount () <= 1) {
				// if so, clear the slot
				selectedItem.clearSlot ();
				updateInventory (selectedItem.slotIndex);
			} else {
				// else reduce the amount by 1
				selectedItem.removeStackable ();
			}
		}
	}

	void dropListener() {
		itemSelect.SetActive (false);
		selectedItem.clearSlot ();
		updateInventory (selectedItem.slotIndex);
	}

	void equipListener() {
		itemSelect.SetActive (false);

		equipWeaponDamage = selectedItem.getSlotItem ().itemValue;
		equipWeaponName = selectedItem.getSlotItem ().itemName;

		equippedWeapon.text = "Weapon: " + equipWeaponName + " +" + equipWeaponDamage;
	}

	void updateInventory(int index) {
		InventorySlot tempslot;
		// shift every item down 1
		for (int x = index; x < playerInventory.Length-2; x++) {
			// copy the contents of the next slot into the current one
			playerInventory [x].GetComponent<InventorySlot>().copyContents(playerInventory [x+1].GetComponent<InventorySlot>());
			// clear the slot that just shifted
			tempslot = playerInventory[x+1].GetComponent<InventorySlot>();
			tempslot.clearSlot ();
		}
	}

	public void onInventoryClick (InventorySlot iSlot) {
		selectedItem = iSlot;
		currentItem.text = selectedItem.getSlotItem().itemName;

		itemSelect.SetActive (true);
		itemSelect.transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y + 15f, 0f);

		// hide use/equip button depending on item
		if (iSlot.isUsable()) {
			equipButton.gameObject.SetActive (false);
			useButton.gameObject.SetActive (true);
		} else {
			equipButton.gameObject.SetActive (true);
			useButton.gameObject.SetActive (false);
		}
	}

	public void onHoverExit() {
		itemSelect.SetActive (false);
	}

	public void openShop() {
		currShop.showShop ();
		inShop = true;
	}

	public void closeShop() {
		currShop.hideShop ();
		inShop = false;
	}

	public void obtainItem() {
		currItem.getItem ();
		dsSword = true;
	}

	public void addSwordsman() {
		swordsmanBattle = true;
		swordsman.SetActive (false);
	}

	public void battleToggleListener(bool b) {
		GameManager.instance.battleToggleOverride = b;
	}

	public void disableDialogue() {
		Destroy (currentDialogue.gameObject);
		allowMovement = true;
		firstConvo = true;
	}
}
