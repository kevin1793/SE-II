using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableItem : MonoBehaviour {

	public Item item;
	public TextAsset altDialogue;

	private SpriteRenderer spriteRend;
	private bool obtainable = true;
	AudioSource SwordDraw;

	// Use this for initialization
	void Start () {
		spriteRend = gameObject.GetComponent<SpriteRenderer> ();
		if(GameManager.instance.dsSword) {
			deactivate();
		}

		SwordDraw = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getItem() {
		if (obtainable) {
			deactivate ();
			GameManager.instance.addItemToInventory (item);
			SwordDraw.Play ();
		}
	}

	public void deactivate() {
		obtainable = false;
		StartCoroutine ("Delay");
	}

	IEnumerator Delay(){
		
		yield return new WaitForSeconds (.2f);
		spriteRend.sprite = null;
	}
}
