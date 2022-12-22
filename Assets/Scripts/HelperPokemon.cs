using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonUnity;
using UnityEngine;

public static class HelperPokemon
{
    public static void RegisterInDex(this Pokemons pokemon, bool caught = false)
    {
        GameData.instance.pokedexlist[(int)pokemon].seen = true;
        if (caught)
            GameData.instance.pokedexlist[(int)pokemon].caught = true;
    }
    public static Sprite GetFrontSprite(this Pokemons pokemon)
    {
        return GameData.instance.frontMonSprites[(int)pokemon - 1];
    }
    public static Sprite GetBackSprite(this Pokemons pokemon)
    {
        return GameData.instance.backMonSprites[(int)pokemon - 1];
    }
    public static PokedexEntry GetPokedexEntry(this Pokemons pokemon) 
    {
        return GameData.instance.pokedexlist[(int)pokemon - 1];
    }
}