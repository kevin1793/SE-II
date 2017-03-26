﻿// Author: Daniel Berg
// Date: 3/24/2017
// Description: Healthbar UI class used to represent player/monster health within a battle
//	

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarText : MonoBehaviour {

	public Text healthText;

	// Use this for initialization
	void Start () {
		healthText.text = "Health: " + gameObject.GetComponent<Slider> ().value;
	}
	
	// Update is called once per frame
	void Update () {
		healthText.text = "Health: " + gameObject.GetComponent<Slider> ().value;
	}
}
