// Author: Daniel Berg
// Date: 3/24/2017
// Description: BattleMonster parent class for all monsters
//	

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonster : MonoBehaviour {

	// vars
	// monster health
	public int monsterHealth;
	// how much base damage this monster does
	public int monsterStrength;
	// id corresponds to an associated healthbar id
	public int monsterID;
	// list of items the monster can drop with chances
	[System.Serializable]
	public class ItemDrop
	{
		public float chance;
		public GameObject item;
	}
	// array of drops, ordered from highest rarity to lowest rarity
	public ItemDrop[] dropTable;
	// animator
	private Animator monstAnim;

	// Use this for initialization
	void Start () {
		monstAnim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void monstAttack() {
		monstAnim.SetTrigger ("onAttack");
	}

	public void monstOnHit() {
		monstAnim.SetTrigger ("onHit");
	}

	// randomly drop an item from this monster's droptable for the player
	public void dropItem() {
		float drop = Random.Range (0.0f, 1.0f);

		// loop through ordered droptable to see what dropped
		foreach(ItemDrop i in dropTable) {
			if(drop <= i.chance) {
				// add it to player's inventory and stop checking
				GameManager.instance.addItemToInventory (i.item.GetComponent<Item>());
				return;
			}
		}
	}
}
