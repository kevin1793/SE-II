// Author: Daniel Berg
// Date: 2/24/2017
// Description: Class to ensure the UI persists across all scenes
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistentUI : MonoBehaviour {

	public static PersistentUI instance = null;

	// Use this for initialization
	void Awake () {
		// if no UI exists, set this as the UI
		if (instance == null) {
			instance = this;
		}
		// if there is an existing UI, delete this
		else if (instance != this) {
			Destroy (gameObject);
		}

		// persist
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
