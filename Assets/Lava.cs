using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

	private Animator anim;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.speed = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
