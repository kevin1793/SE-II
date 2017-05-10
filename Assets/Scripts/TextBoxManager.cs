using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {
	
	private List<string> dialogueText;
	private int currentIndex;
	public bool inConvo = false;
	public bool cont = false;

	private GameObject dBox;
	private Text dText;

	// Use this for initialization
	void Start () {
		dBox = GameManager.instance.dBox;
		dialogueText = new List<string> ();
		dText = dBox.GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {

		//to not skip first piece of dialogue when 'f' is pressed initially for the dialogue box
		if (inConvo == true && Input.GetKeyUp (KeyCode.F)) {
			cont = true;
		}

		// if you are talking, do conversation stuff
		if (inConvo && cont == true) {
			// if user hits space to continue the dialogue
			if (Input.GetKeyDown (KeyCode.F) && cont == true) {
				
				// if you reached the end
				if (currentIndex == dialogueText.Count-1) {
					endDialogue ();
					cont = false;    // helps with not skipping on new convo
					inConvo = false;
				}
				// else continue to next line of text
				else {
					currentIndex++;
					updateText ();
				}
			}
			// else do nothing (add scrolling here if needed)
		}
	}

	// call this when a conversation ends
	private void endDialogue() {
		GameManager.instance.inConversation = false;
		dBox.gameObject.SetActive (false);
	}

	// call this when TextBoxManager needs to update the dialogue text
	private void updateText() {
		dText.text = dialogueText [currentIndex];
	}

	// setter function
	public void setDialogue(string[] dialogue) {
		// clear list if needed
		if (dialogueText.Count > 0) {
			dialogueText.Clear ();
		}

		// add the strings to the list
		foreach (string x in dialogue) {
			dialogueText.Add (x);
		}

		// set index to 0
		currentIndex = 0;
		updateText ();
		inConvo = true;
	}
}
