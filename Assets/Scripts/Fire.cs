using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fire : MonoBehaviour {

	// Use this for initialization


	public int fireAttack = 0;
	public int begin = 0;
	GameObject dBox;
	GameObject dBox1;
	public int counter = 0;
	GameObject fire;
	AudioSource Roar;
	GameObject h;


	void Start () {
		fire = GameObject.Find ("DragonAttack");
		dBox = GameObject.Find ("Canvas(Clone)");
		dBox1 = dBox.transform.Find ("dialogueBox").gameObject;
		h = GameObject.Find ("Hero");
		Roar = GameObject.Find ("Growl").GetComponent<AudioSource> ();


		fire.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate() {

		if (counter == 9) {
			fire.SetActive (true);
			StartCoroutine ("vanish");

		}

		if (dBox1.activeSelf) {
			begin = 1;
		}
	
		
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.F)) {
			counter++;
		}

		if (counter == 11 && Input.GetKeyUp(KeyCode.F)) {
			Roar.Play ();
		}

		if (counter == 13 && Input.GetKeyUp(KeyCode.F)) {
			SceneManager.LoadScene ("Tutorial scene");
		}
	}

	IEnumerator vanish() {
		// drop 200-500 gold to player inventory
		yield return new WaitForSeconds (1.5f); //time for victory music
		h.GetComponent<SpriteRenderer>().sortingOrder = -5;
	}


}
