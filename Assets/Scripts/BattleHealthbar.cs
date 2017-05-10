using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHealthbar : MonoBehaviour {

	// associated healthbar
	Slider healthBar;
	// associated monster/npc id
	private int id;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// setter and getter functions
	public void setId(int i) {
		id = i;
	}

	public int getId() {
		return id;
	}
}
