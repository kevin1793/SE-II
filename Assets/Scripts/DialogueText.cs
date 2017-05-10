using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueText : MonoBehaviour {

	public TextAsset textFile;
	public string[] dialogue;

	// Use this for initialization
	void Start () {
		dialogue = textFile.text.Split ('\n');
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
