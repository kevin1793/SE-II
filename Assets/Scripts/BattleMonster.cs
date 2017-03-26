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
	// id corresponds to an associated healthbar id
	public int monsterID;

	private Vector3 location;
	private Animator monstAnim;

	// Use this for initialization
	void Start () {
		// set location to return to when attacking
		location = gameObject.transform.position;
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
}
