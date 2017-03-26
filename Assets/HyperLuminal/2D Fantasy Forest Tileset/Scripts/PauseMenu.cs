using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	public bool isPaused;
	public GameObject pausemenucanvas;

	// Update is called once per frame
	void Update () {

		if (isPaused) {
			pausemenucanvas.SetActive (true);
		} else {
			pausemenucanvas.SetActive (false);
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			isPaused = !isPaused;
		}
		
	}

	public void Resume(){
		isPaused = false;	
	}

	public void doExitGame() {
		Application.Quit();
	}
}
