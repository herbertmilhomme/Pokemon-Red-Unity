using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class LevelUpMove{
    public PokemonUnity.Moves move;
    public int level;

    public LevelUpMove(PokemonUnity.Moves move, int level){
        this.move = move;
        this.level = level;
    }
}

//Enum for the different evolution methods
public enum EvolutionMethod {
  Level,
  Item,
  Trade
}

[System.Serializable]
public class PokemonEvolution{
    public PokemonUnity.Pokemons pokemon;
    public EvolutionMethod method;
	/*
	Contains different values depending on the evolution method
	(level up: level, item: evo item id, trade: n/a)
	*/
	/*
	evolution item ids:
	0: Fire Stone
	1: Thunder Stone
	2: Water Stone
	3: Leaf Stone
	4: Moon Stone
	
	*/
    public int param;
}

//Class for encounter tables.
[System.Serializable]
public class EncounterData{
    public int encounterChance;
    public Tuple<PokemonUnity.Pokemons,int>[] slots;
}

public class FishingGroup{
    public Tuple<string,int>[] slots;
    public FishingGroup(Tuple<string,int>[] slots) {
        this.slots = slots;
    } 
}

public class PokemonData
{
    public static string GetTypeName(PokemonUnity.Types type){
        if (type == PokemonUnity.Types.NONE) return "";
        else return type.ToString();
    }

    public static int GetItemPrice(PokemonUnity.Inventory.Items item){
        return GetItemData(item).Price;
    }

    public static PokemonUnity.Moves TMHMToMove(int tmhmIndex){
        return TMHMMoves[tmhmIndex];
    }

    public static PokemonUnity.Attack.Data.MoveData GetMoveData(PokemonUnity.Moves moveToGet) => PokemonUnity.Kernal.MoveData[moveToGet];
    public static PokemonUnity.Monster.Data.PokemonData GetPokemonData(PokemonUnity.Pokemons pokemon) => PokemonUnity.Kernal.PokemonData[pokemon];
    public static PokemonUnity.Inventory.ItemData GetItemData(PokemonUnity.Inventory.Items item) => PokemonUnity.Kernal.ItemData[item];

    // ToDo: Fix this
    public static int GetPartySprite(PokemonUnity.Pokemons pokemon) => 0;

    public static List<EncounterData> encounters = new List<EncounterData>();
    /* Encounter Table Indices:
    0:Diglett Cave
    1:Mansion 1
    2:Mansion 2
    3:Mansion 3
    4:Mansion B1
    5:Mt Moon 1
    6:Mt Moon B1
    7:Mt Moon B2
    8:Pokemon Tower 1
    9:Pokemon Tower 2
    10:Pokemon Tower 3
    11:Pokemon Tower 4
    12:Pokemon Tower 5
    13:Power Plant
    14:Rock Tunnel 1
    15:Rock Tunnel 2
    16:Route 1
    17:Route 2
    18:Route 3
    19:Route 4
    20:Route 5
    21:Route 6
    22:Route 7
    23:Route 8
    24:Route 9
    25:Route 10
    26:Route 11
    27:Route 12
    28:Route 13
    29:Route 14
    30:Route 15
    31:Route 16
    32:Route 17
    33:Route 18
    34:Route 21
    35:Route 22
    36:Route 23
    37:Route 24
    38:Route 25
    39:Safari Zone 1
    40:Safari Zone 2
    41:Safari Zone 3
    42:Safari Zone Center
    43:Seafoam Island 1
    44:Seafoam Island B1
    45:Seafoam Island B2
    46:Seafoam Island B3
    47:Seafoam Island B4
    48:Unknown Dungeon 1
    49:Unknown Dungeon 2
    50:Unknown Dungeon B1
    51:Victory Road 1
    52:Victory Road 2
    53:Victory Road 3
    54:Viridian Forest
    55:Water Pokemon
     */

    //TODO: These really should be put into a json file
     
    //fishing group for good rod
    public FishingGroup goodRodFishingGroup = new FishingGroup(new Tuple<string,int>[]{new Tuple<string, int>("Goldeen",10),new Tuple<string, int>("Poliwag",10)});
    
    //groups for the super rod
    public static List<FishingGroup> superRodFishingGroups = new List<FishingGroup>(new FishingGroup[]{ 
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Tentacool",15), new Tuple<string,int>("Poliwag",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Poliwag",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Psyduck",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Krabby",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Poliwhirl",23), new Tuple<string,int>("Slowpoke",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Shellder",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Dratini",15), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Psyduck",15), new Tuple<string,int>("Slowpoke",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Tentacool",5), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Magikarp",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Staryu",15), new Tuple<string,int>("Horsea",15), new Tuple<string,int>("Shellder",15), new Tuple<string,int>("Goldeen",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Slowbro",23), new Tuple<string,int>("Seaking",23), new Tuple<string,int>("Kingler",23), new Tuple<string,int>("Seadra",23)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Seaking",23), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Magikarp",15)}),
    });

    public static Dictionary<string,string[]> shopItemsLists = new Dictionary<string, string[]>();

    public static PokemonUnity.Moves[] TMHMMoves = {
    PokemonUnity.Moves.MEGA_PUNCH,
    PokemonUnity.Moves.RAZOR_WIND,
    PokemonUnity.Moves.SWORDS_DANCE,
    PokemonUnity.Moves.WHIRLWIND,
    PokemonUnity.Moves.MEGA_KICK,
    PokemonUnity.Moves.TOXIC,
    PokemonUnity.Moves.HORN_DRILL,
    PokemonUnity.Moves.BODY_SLAM,
    PokemonUnity.Moves.TAKE_DOWN,
    PokemonUnity.Moves.DOUBLE_EDGE,
    PokemonUnity.Moves.BUBBLE_BEAM,
    PokemonUnity.Moves.WATER_GUN,
    PokemonUnity.Moves.ICE_BEAM,
    PokemonUnity.Moves.BLIZZARD,
    PokemonUnity.Moves.HYPER_BEAM,
    PokemonUnity.Moves.PAY_DAY,
    PokemonUnity.Moves.SUBMISSION,
    PokemonUnity.Moves.COUNTER,
    PokemonUnity.Moves.SEISMIC_TOSS,
    PokemonUnity.Moves.RAGE,
    PokemonUnity.Moves.MEGA_DRAIN,
    PokemonUnity.Moves.SOLAR_BEAM,
    PokemonUnity.Moves.DRAGON_RAGE,
    PokemonUnity.Moves.THUNDERBOLT,
    PokemonUnity.Moves.THUNDER,
    PokemonUnity.Moves.EARTHQUAKE,
    PokemonUnity.Moves.FISSURE,
    PokemonUnity.Moves.DIG,
    PokemonUnity.Moves.PSYCHIC,
    PokemonUnity.Moves.TELEPORT,
    PokemonUnity.Moves.MIMIC,
    PokemonUnity.Moves.DOUBLE_TEAM,
    PokemonUnity.Moves.REFLECT,
    PokemonUnity.Moves.BIDE,
    PokemonUnity.Moves.METRONOME,
    PokemonUnity.Moves.SELF_DESTRUCT,
    PokemonUnity.Moves.EGG_BOMB,
    PokemonUnity.Moves.FIRE_BLAST,
    PokemonUnity.Moves.SWIFT,
    PokemonUnity.Moves.SKULL_BASH,
    PokemonUnity.Moves.SOFT_BOILED,
    PokemonUnity.Moves.DREAM_EATER,
    PokemonUnity.Moves.SKY_ATTACK,
    PokemonUnity.Moves.REST,
    PokemonUnity.Moves.THUNDER_WAVE,
    PokemonUnity.Moves.PSYWAVE,
    PokemonUnity.Moves.EXPLOSION,
    PokemonUnity.Moves.ROCK_SLIDE,
    PokemonUnity.Moves.TRI_ATTACK,
    PokemonUnity.Moves.SUBSTITUTE,
    PokemonUnity.Moves.CUT,
    PokemonUnity.Moves.FLY,
    PokemonUnity.Moves.SURF,
    PokemonUnity.Moves.STRENGTH,
    PokemonUnity.Moves.FLASH
};

}