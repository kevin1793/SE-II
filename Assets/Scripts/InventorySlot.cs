using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler {

	public int slotIndex;
	public bool isEmpty = true;
	private int itemId = -1;
	private Image slotImage;
	private Text slotText;
	private int amount = 0;
	private bool usable = false;
	private Item currentItem = null;

	// Use this for initialization
	void Start () {
		slotImage = gameObject.GetComponent<Image> ();
		slotText = gameObject.GetComponentInChildren<Text> ();
		slotText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setItem(Item i) {
		currentItem = i;
		itemId = i.itemId;
		amount = 1;
		isEmpty = false;
		usable = i.usable;
		slotImage.sprite = i.itemSprite;
		slotImage.color = new Color (slotImage.color.r, slotImage.color.g, slotImage.color.b, 1.0f);
		if (i.stackable) {
			slotText.text = "" + amount;
		}
	}

	public void clearSlot() {
		currentItem = null;
		itemId = -1;
		amount = 0;
		slotText.text = "";
		isEmpty = true;
		usable = false;
		slotImage.sprite = null;
		slotImage.color = new Color (slotImage.color.r, slotImage.color.g, slotImage.color.b, 0.0f);
	}

	public void copyContents(InventorySlot i) {
		isEmpty = i.isEmpty;
		itemId = i.getId ();
		amount = i.getAmount();
		usable = i.isUsable();
		currentItem = i.getSlotItem();
		slotImage.sprite = currentItem.itemSprite;
		if (isEmpty) {
			slotImage.color = new Color (slotImage.color.r, slotImage.color.g, slotImage.color.b, 0.0f);
		} else {
			slotImage.color = new Color (slotImage.color.r, slotImage.color.g, slotImage.color.b, 1.0f);
		}
			
		if (currentItem.stackable) {
			slotText.text = "" + amount;
		} else {
			slotText.text = "";
		}
	}

	public void addStackable() {
		amount++;
		slotText.text = "" + amount;
	}

	public void removeStackable() {
		amount--;
		slotText.text = "" + amount;
	}

	public int getId() {
		return itemId;
	}

	public bool isUsable() {
		return usable;
	}

	public Item getSlotItem() {
		return currentItem;
	}

	public int getAmount() {
		return amount;
	}

	public void OnPointerDown(PointerEventData d) {
		// if player clicked a non-empty inventory space
		if (isEmpty == false) {
			GameManager.instance.onInventoryClick (this);
		}
	}
}
