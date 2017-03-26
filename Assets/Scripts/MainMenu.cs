using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	public void Play(string newgamelevel){
		SceneManager.LoadScene (newgamelevel);	
	}

	public void doExitGame() {
		Application.Quit();
	}
}
