using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformLoader : MonoBehaviour {

	public GameObject HeroPos;
	public bool start;
	public bool did= false;

	// Use this for initialization
	void Start () {
		
		HeroPos = GameObject.Find ("Hero");
		HeroPos.transform.position = new Vector3(29.00f ,10.81f, 0f);
		Debug.Log("Start");
	}

	void Awake(){
		
		HeroPos = GameObject.Find ("Hero");
		HeroPos.transform.position = new Vector3(29.00f ,10.81f, 0f);
		Debug.Log("Awake");
	}
	void OnTriggerEnter2D(Collider other)
	{
		other.gameObject.transform.position = new Vector3(29.00f, 10.81f, 0f);
		Debug.Log("trigger");
	}
	// Update is called once per frame
	void FixedUpdate ()
	{

		if (start) {
			HeroPos.transform.position = new Vector3 (29.00f, 10.81f, 0f);
			start = false;
			Debug.Log ("Update");
			did = true;
		}

		if (HeroPos.transform.position.x == 0) {
			HeroPos.transform.position = new Vector3 (29.00f, 10.81f, 0f);
			Debug.Log("trigger");
		}
	}
}