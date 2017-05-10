using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	public bool isPaused;
	public bool isPausedControls;
	public GameObject pausemenucanvas;
	public GameObject controlscanvas;

	// Update is called once per frame
	void Update () {

		if (isPaused) {
			pausemenucanvas.SetActive (true);
			Time.timeScale = 0;
		} else {
			pausemenucanvas.SetActive (false);
			controlscanvas.SetActive (false);
			Time.timeScale = 1;
		}
		if (isPausedControls) {
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

	public void ControlsOpen(){
		controlscanvas.SetActive (true);
		isPausedControls = true;
	}

	public void ControlsClose(){
		controlscanvas.SetActive (false);
		isPausedControls = false;
	}

	public void mainMenu(string MainMenu){
		SceneManager.LoadScene (MainMenu);
	}
		
}
