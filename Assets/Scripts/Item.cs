// Author: Daniel Berg
// Date: 2/24/2017
// Description: Item parent class representing items in-game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	// private variables (name, item type i.e. sword, potion)
	private string itemName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// setter for name
	public void setItemName( string name ) {
		itemName = name;
	}

	// getter for name
	public string getItemName() {
		return itemName;
	}
}
