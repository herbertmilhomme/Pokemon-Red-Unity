using PokemonUnity.Inventory;
using System.Collections;
using UnityEngine;

public class TextDatabase : MonoBehaviour {
	public static TextDatabase instance;
	void Awake(){
		instance = this;
	}

    public void GetItem(Items item){
        StartCoroutine(GetItemText(item));
    }

    public IEnumerator GetItemText(Items item){
       	Inventory.instance.AddItem(item, 1);
        yield return Dialogue.instance.text(GameData.instance.playerName + " found &l" + item.ToString() + "!");
    }
}
