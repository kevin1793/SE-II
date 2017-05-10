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
}
