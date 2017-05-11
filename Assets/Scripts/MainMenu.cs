using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject Controls;

	void Start(){
		Controls = GameObject.Find ("Canvascontrol");
		Controls.SetActive(false);
	}

	// Use this for initialization
	public void Play(string newgamelevel){
		SceneManager.LoadScene (newgamelevel);	
	}

	public void doExitGame() {
		Application.Quit();
	}
		
	public void onControls() {
		Controls.SetActive (true);
	}

	public void onControlsReturn() {
		Controls.SetActive (false);
	}
		
}
