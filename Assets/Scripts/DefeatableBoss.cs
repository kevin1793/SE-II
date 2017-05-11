using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatableBoss : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (gameObject.activeSelf);
		if (!gameObject.activeSelf && GameManager.instance.finalBossDefeated) {
			gameObject.SetActive (true);
		}
	}
}
