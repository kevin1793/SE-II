using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	[System.Serializable]
	public class ShopItem {
		public Item item;
		public int cost;
	}

	public ShopItem[] items;

	public GameObject shopScreen;

	private Text goldText;

	private Button buyLgHP;
	private Button buyHP;
	private Button buyMP;
	private Button exit;

	private bool hasLoaded = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!hasLoaded) {
			goldText = shopScreen.transform.Find ("shopBG/goldText").gameObject.GetComponent<Text> ();
			goldText.text = "" + GameManager.instance.playerG;

			buyLgHP = shopScreen.transform.Find ("shopBG/LgHealthPotion/BuyLgHP").gameObject.GetComponent<Button> ();
			buyLgHP.onClick.AddListener (buyLgHPListener);
			buyHP = shopScreen.transform.Find ("shopBG/HealthPotion/BuyHP").gameObject.GetComponent<Button> ();
			buyHP.onClick.AddListener (buyHPListener);
			buyMP = shopScreen.transform.Find ("shopBG/ManaPotion/BuyMP").gameObject.GetComponent<Button> ();
			buyMP.onClick.AddListener (buyMPListener);
			exit = shopScreen.transform.Find ("shopBG/Exit").gameObject.GetComponent<Button> ();
			exit.onClick.AddListener (exitListener);

			hideShop ();

			hasLoaded = true;
		}
	}

	void buyItem ( int id ) {
		// if player has the money, take gold and drop item to player
		if (GameManager.instance.playerG >= items [id].cost) {
			GameManager.instance.addGoldToInventory(-(items [id].cost));
			GameManager.instance.addItemToInventory (items [id].item);
			goldText.text = "" + GameManager.instance.playerG;
		}
	}

	public void hideShop() {
		shopScreen.SetActive (false);
		GameManager.instance.inShop = false;
	}

	public void showShop() {
		shopScreen.SetActive (true);
		goldText.text = "" + GameManager.instance.playerG;
	}

	// button listeners
	void buyLgHPListener () {
		buyItem (0);
	}

	void buyHPListener () {
		buyItem (1);
	}

	void buyMPListener () {
		buyItem (2);
	}

	void exitListener () {
		hideShop ();
	}
}
