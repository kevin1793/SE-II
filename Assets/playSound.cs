using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour {

	void Start()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}

}