using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExodusManager : MonoBehaviour {

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

	if (counter == 5 && Input.GetKeyUp(KeyCode.F)) {
			StartCoroutine ("Fade");
	}
}

	IEnumerator Fade(){
		//GameObject.Find("FadePanel").GetComponent<FadeScript>().FadeOut();
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("MainMenu");
	}


}
