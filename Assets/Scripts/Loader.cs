// Author: Daniel Berg
// Date: 2/24/2017
// Description: Loader class used to instantiate Singleton objects for the first time
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject UIelement;

	// Use this for initialization
	void Awake () {
		// load all elements, then gamemanager
		if (PersistentUI.instance == null) {
			Instantiate (UIelement);
		}
		if (GameManager.instance == null) {
			Instantiate (gameManager);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
