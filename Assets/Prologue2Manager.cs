using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue2Manager : MonoBehaviour {

	// Use this for initialization

	public int counter=0;

void Start () {
	}
// Update is called once per frame
void FixedUpdate() {

}

void Update(){
	if (Input.GetKeyDown (KeyCode.F)) {
		counter++;
	}

	if (counter == 11 && Input.GetKeyUp(KeyCode.F)) {
			StartCoroutine ("Fade");
	}
}

	IEnumerator Fade(){
		//GameObject.Find("FadePanel").GetComponent<FadeScript>().FadeOut();
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("Prologue3");
	}


}
