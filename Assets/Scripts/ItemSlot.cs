using PokemonUnity.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotMode{
    Item,
    MartItem,
    Empty,
    Cancel
}

public class ItemSlot : MonoBehaviour {
	public bool isKeyItem;
	public CustomText slotNameText, slotQuantityText;
	public Items item;
    //public ItemDataEntry itemData;
	public int quantity;
    public int price;
    public SlotMode mode;

	// Use this for initialization
	void Awake () {
        slotNameText = transform.GetChild(0).GetComponent<CustomText>();
        slotQuantityText = transform.GetChild(1).GetComponent<CustomText>();
	}
	
	// Update is called once per frame
	void Update () {
        switch(mode){
            case SlotMode.Item:
                slotNameText.text = item.ToString();
                break;
            case SlotMode.Empty:
                slotNameText.text = "";
                break;
            case SlotMode.Cancel:
                slotNameText.text = "CANCEL";
                break;
        }
		
		if (!isKeyItem && (mode == SlotMode.Item || mode == SlotMode.MartItem)) {
            if(mode == SlotMode.Item){
			    slotQuantityText.text = "*" + (quantity <= 9 ? " ": "") + quantity.ToString();
            }else{
                slotQuantityText.text = "$" + price;
            }
		} else {

			slotQuantityText.text = "";
		}
	}

    public void UpdatePrice()
    {
        var itemDataEntry = PokemonData.GetItemData(item);

        if (itemDataEntry.Price != 0)
        {
            price = itemDataEntry.Price;
        }
        else throw new UnityException("A price entry doesn't exist for " + "\"" + name + "\"");
    }
}
