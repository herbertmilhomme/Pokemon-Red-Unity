using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewBio : MonoBehaviour {
	public GameObject menu;
	public int pokemonID;
    public bool displayingBio;
	public Image pokemonSprite;
	public CustomText descriptionText, nameText, categoryText, heightText, weightText, dexNoText;
	string pokemonName = "";
	PokemonUnity.Monster.Data.PokemonData entryData;


	public IEnumerator DisplayABio(int whatBio){
		SoundManager.instance.SetMusicLow();
        PokedexEntry entry = GameData.instance.pokedexlist[whatBio];
		pokemonName = ((PokemonUnity.Pokemons)whatBio).ToString();

        Debug.Log("Display " + pokemonName +  "'s bio. This Pokemon " + (entry.seen && entry.caught ? "has been seen and caught." : entry.seen ? "has been seen." : "has not been seen or caught."));
		
		pokemonID = whatBio;
		entryData = PokemonData.GetPokemonData((PokemonUnity.Pokemons)pokemonID);

		InitText();
		menu.SetActive(true);
		displayingBio = true;

		SoundManager.instance.PlayCry(pokemonID);

		while(SoundManager.instance.isPlayingCry){
			yield return new WaitForSeconds(0.01f);
		}

		while (true) {
			yield return new WaitForSeconds(0.01f);
			if(InputManager.Pressed(Button.A)) break;
		}

		// ToDo: Fix this
		//If there's more than one page for the description, go to the next page
		/*if(entryData.descriptionText.Length > 1){
			descriptionText.text = entryData.descriptionText[1];

			while (true) {
				yield return new WaitForSeconds (0.01f);
				if(InputManager.Pressed(Button.A)) break;
			}
		}*/
		descriptionText.text = "Hi";


        displayingBio = false;
		menu.SetActive(false);
		SoundManager.instance.SetMusicNormal();
	}

	// ToDo: Fix this
	public void InitText(){
		nameText.text = pokemonName;
		categoryText.text = "Pokemon";
		//heightText.text = entryData.heightFeet + " " + (entryData.heightInches < 10 ? "0" : "") + entryData.heightInches;
		heightText.text = entryData.Height.ToString();
		weightText.text = string.Format("{0,5:0.0}",entryData.Weight);
		dexNoText.text = (pokemonID > 99 ? "" : pokemonID > 9 ? "0" : "00") + pokemonID.ToString();
		//descriptionText.text = entryData.descriptionText[0];
		descriptionText.text = "Hi, pokemon";
		pokemonSprite.sprite = GameData.instance.frontMonSprites[pokemonID - 1];
	}


}

