using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue3Manager : MonoBehaviour {

	public int counter = 0;
	Animator playerAnim;
	GameObject player;

	// Use this for initialization
	void Start () {
		playerAnim = GameObject.Find ("WakingUp").GetComponent<Animator>();
		//player = GameObject.Find("GameManager")

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.F)) {
			counter++;
		}

		if (counter == 5 && Input.GetKeyUp(KeyCode.F)) {
			playerAnim.SetInteger ("Prologue3", 1);
		}
		if (counter == 7 && Input.GetKeyUp (KeyCode.F)) {
			playerAnim.SetInteger ("Prologue3", 2);
		}

		if (counter == 8 && Input.GetKeyUp(KeyCode.F)) {
			playerAnim.SetInteger ("Prologue3", 3);
		}

		if (counter == 10 && Input.GetKeyUp(KeyCode.F)) {
			playerAnim.SetInteger ("Prologue3", 4);
		}

		if (counter == 12 && Input.GetKeyUp(KeyCode.F)) {
			GameManager.instance.inTutorial = true;
			GameManager.instance.inConversation = false;
			GameManager.instance.firstConvo = true;
			SceneManager.LoadScene("Tutorial scene");
		}
	}


}
