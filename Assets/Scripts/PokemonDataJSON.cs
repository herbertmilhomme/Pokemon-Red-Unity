using PokemonUnity;
using System.Collections.Generic;
using UnityEngine;

/*
 {
    "name": "MISSINGNO.",
    "id": 152,
    "category": "???",
    "heightFeet": 3,
    "heightInches": 3,
    "weight": 22,
    "descriptionText": [
      "コメント　さくせいちゅう"
    ],
    "partySprite": 10,
    "baseStats": [
      33,
      136,
      0,
      6,
      29
    ],
    "catchRate": 29,
    "baseExp": 143,
    "expGroup": 4,
    "evolutions": [],
    "types": [
      "Bird",
      "Normal"
    ],
    "tmhmLearnset": [
      1,
      2,
      3,
      5,
      6,
      9,
      10,
      11,
      13,
      14,
      17,
      19,
      20,
      25,
      26,
      27,
      29,
      30,
      43,
      44,
      45,
      49,
      50,
      51,
      52
    ],
    "levelupLearnset": [
      {
        "move": "WaterGun",
        "level": 1
      },
      {
        "move": "WaterGun",
        "level": 1
      },
      {
        "move": "SkyAttack",
        "level": 1
      }
    ]
  }

ToDo: Add that data back
 
 */

// ToDo: Delete this script
public class PokemonDataJSON : MonoBehaviour {
    public VersionManager versionManager;
    static readonly string db = $"Data Source ={Application.streamingAssetsPath}/veekun-pokedex.sqlite";
    static void ResetSQLConnection()
    {
        Game.con = new Mono.Data.Sqlite.SqliteConnection(db);
        PokemonUnity.Game.ResetSqlConnection(db);
    }

    void Awake(){
        // Load the data from sqlite
        ResetSQLConnection();

        //this should also be a class list
        //PokemonData.TypeEffectiveness = Serializer.JSONtoObject<Dictionary<Types,Dictionary<Types,float>>>("typeEffectiveness.json");
        //PokemonData.pokemonData = Serializer.JSONtoObject<List<PokemonDataEntry>>("pokemonData.json");
        //PokemonData.moves = Serializer.JSONtoObject<List<MoveData>>("moveData.json");
        //PokemonData.itemData = Serializer.JSONtoObject<List<MoveData>>("itemData.json");
        //also should be changed
        PokemonData.shopItemsLists = Serializer.JSONtoObject<Dictionary<string,string[]>>("shopItemsData.json");
    }

    public static void InitVersion() => PokemonData.encounters = Serializer.JSONtoObject<List<EncounterData>>($"encounterData{GameData.instance.version}.json");
}

