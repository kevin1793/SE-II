using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreFinalBattle : MonoBehaviour {

	// Use this for initialization


	public int fireAttack = 0;
	public int begin = 0;
	Animator Hero;
	public int counter = 0;
	GameObject fire;
	AudioSource Roar;
	AudioSource Laugh;
	GameObject h;


	void Start () {
		fire = GameObject.Find ("DragonAttack");
		h = GameObject.Find ("Hero");
		Roar = GameObject.Find ("Growl").GetComponent<AudioSource> ();
		Laugh = GameObject.Find ("Laugh").GetComponent<AudioSource> ();
		Hero = GameObject.Find ("Hero").GetComponent<Animator> ();

		fire.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.F)) {
			counter++;
		}

		if (counter == 2 && Input.GetKeyUp (KeyCode.F)) {
			Laugh.Play ();
		}

		if (counter == 4 && Input.GetKeyUp (KeyCode.F)) {
			Roar.Play ();
		}
		if (counter == 5 && Input.GetKeyUp (KeyCode.F)) {
			StartCoroutine ("BeginBattle");
		}
	}

	IEnumerator BeginBattle() {
		yield return new WaitForSeconds (.5f);
		SceneManager.LoadScene ("Final Battle Scene");
		}
			
}