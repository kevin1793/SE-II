// Author: Daniel Berg
// Date: 2/24/2017
// Description: Item class representing items in-game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	// variables

	// unique id
	public int itemId;

	// image for item in inventory
	public Sprite itemSprite;

	// name of the item
	public string itemName;

	// if weapon value is damage, if potion value is amount healed
	public int itemValue;

	// can you have multiple of this item in 1 inventory space
	public bool stackable;

	// is this an item to be used to equipped?
	public bool usable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
