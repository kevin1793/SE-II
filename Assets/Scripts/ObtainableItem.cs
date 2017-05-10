using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableItem : MonoBehaviour {

	public Item item;
	public TextAsset altDialogue;

	private SpriteRenderer spriteRend;
	private bool obtainable = true;

	// Use this for initialization
	void Start () {
		spriteRend = gameObject.GetComponent<SpriteRenderer> ();
		if(GameManager.instance.dsSword) {
			deactivate();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getItem() {
		if (obtainable) {
			deactivate ();
			GameManager.instance.addItemToInventory (item);
		}
	}

	public void deactivate() {
		obtainable = false;
		spriteRend.sprite = null;
	}
}
