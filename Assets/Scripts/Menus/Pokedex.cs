﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Pokedex : MonoBehaviour
{
    public GameObject entriescontainer;
    public List<GameObject> entries;
    public GameCursor cursor;
    public CustomText seentext, owntext;
    public int selectedSlot, topSlotIndex;
    public bool selectingMon;
    public ViewBio viewBio;

    int seen{
        get
        {
            int seennumber = 0;
            foreach (PokedexEntry entry in GameData.instance.pokedexlist)
            {
                if (entry.seen) seennumber++;
            }
            return seennumber;
        }
    }
    int caught{
        get
        {
            int caughtnumber = 0;
            foreach (PokedexEntry entry in GameData.instance.pokedexlist)
            {
                if (entry.caught) caughtnumber++;
            }
            return caughtnumber;
        }
    }

    public static Pokedex instance;

    void Awake()
    {
        instance = this;
        entries = new List<GameObject>();
        entries.Clear();
        for (int i = 0; i < 7; i++)
        {
            entries.Add(entriescontainer.transform.GetChild(i).gameObject);
        }
    }
  
    public void Init(){
        topSlotIndex = 1;
        selectedSlot = 0;
        seentext.text = seen.ToString();
        owntext.text = caught.ToString();
      
        UpdateScreen();
    }

    void UpdateScreen(){
        for (int i = 0; i < 7; i++)
        {
            int slotNo = topSlotIndex + i;
            entries[i].transform.GetChild(0).GetComponent<CustomText>().text = slotNo.ZeroFormat("00x") + "\n" + (!GameData.instance.pokedexlist[slotNo - 1].seen ? "   ----------" : "   " + PokemonData.IndexToMon(slotNo));
            entries[i].transform.GetChild(1).gameObject.SetActive(GameData.instance.pokedexlist[slotNo - 1].caught);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!selectingMon) cursor.SetPosition(0,112 - 16 * selectedSlot);
        if(!cursor.isActive) cursor.SetActive(true);
        if(viewBio.displayingBio) cursor.SetActive(false);

        if(MainMenu.instance.currentmenu == MainMenu.instance.pokedexmenu)
        {
            if(Inputs.pressed("b") && !viewBio.displayingBio)
            {
                SoundManager.instance.PlayABSound();
                if(Player.instance.isDisabled) Player.instance.isDisabled = false;
                Debug.Log(Player.instance.isDisabled);
                if(selectingMon) selectingMon = false;
                else{
                    Inputs.Enable("start");
                    
                    MainMenu.instance.currentmenu = MainMenu.instance.thismenu;

                    gameObject.SetActive(false);
                }
            }
            if(Inputs.pressed("a") && !viewBio.displayingBio)
            {
                SoundManager.instance.PlayABSound();

                if(!selectingMon && GameData.instance.pokedexlist[topSlotIndex + selectedSlot - 1].seen)
                {
                    selectingMon = true;
                    cursor.SetPosition(120,56);
                }
                else if(GameData.instance.pokedexlist[topSlotIndex + selectedSlot - 1].seen)
                {
                    StartCoroutine(viewBio.DisplayABio(topSlotIndex + selectedSlot));
                }

            }
            if (Inputs.pressed("down"))
            {
                if (!selectingMon)
                {
                    selectedSlot++;

                    if (selectedSlot > 6)
                    {
                        selectedSlot = 6;
                        if (topSlotIndex < GameData.instance.pokedexlist.Count - 6)
                        {
                            topSlotIndex += 1;
                        }
                        UpdateScreen();
                    }
                }
            }
            if (Inputs.pressed("up"))
            {
                if (!selectingMon)
                {
                    selectedSlot--;

                    if (selectedSlot < 0)
                    {
                        selectedSlot = 0;
                        if (topSlotIndex > 1)
                        {
                            topSlotIndex -= 1;
                        }
                        UpdateScreen();
                    }
                }
            }
            if (Inputs.pressed("right"))
            {
                if (!selectingMon)
                {
                    topSlotIndex += 10;
                    if (topSlotIndex > GameData.instance.pokedexlist.Count - 6) topSlotIndex = GameData.instance.pokedexlist.Count - 6;
                    UpdateScreen();
                }
            }
            if (Inputs.pressed("left"))
            {
                if (!selectingMon)
                {
                    topSlotIndex -= 10;
                    if (topSlotIndex < 1) topSlotIndex = 1;
                    UpdateScreen();
                }
            }
        }

    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Pokedex))]
public class PokedexDebug : Editor
{
    public int OverwriteIndex;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        OverwriteIndex = EditorGUILayout.IntField("Selected Index", OverwriteIndex);
        if (GUILayout.Button("Set Pokedex Entry to Seen"))
        {
            GameData.instance.pokedexlist[OverwriteIndex - 1].seen = true;
        }
        if (GUILayout.Button("Set Pokedex Entry to Owned"))
        {
            GameData.instance.pokedexlist[OverwriteIndex - 1].caught = true;
        }
    }
}
#endif

