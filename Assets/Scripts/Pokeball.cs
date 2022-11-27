using PokemonUnity.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour, InteractableObject
{
    public Items item;

    public IEnumerator Interact(){
        Inventory.instance.AddItem(item, 1);
        yield return Dialogue.instance.text(GameData.instance.playerName + " found &l" + item.ToString() + "!");
        this.gameObject.SetActive(false); //maybe replace with Destroy
    }
}
