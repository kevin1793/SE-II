using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : MonoBehaviour {

	public TextAsset joinText;

	// Use this for initialization
	void Start () {
		// if we have him in our party
		if (GameManager.instance.swordsmanBattle) {
			gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
