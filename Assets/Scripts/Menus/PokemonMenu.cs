using PokemonUnity;
using PokemonUnity.Monster;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PartyAnim //maybe make this into a struct instead? (and other similar small classes with only variables) do more research and experimentation w/ structs in the future
{
    public List<Sprite> anim;
}
public class PokemonMenu : MonoBehaviour
{
    public GameObject mainwindow, switchstats, stats1, stats2;
    public GameObject currentMenu;
    public GameObject[] allmenus;
    public List<GameObject> partyslots;
    public int selectedOption;
    public GameCursor cursor;
    public int selectedMon;
    public int numberofPokemon;
    public bool switching, selectingPokemon;
    public Sprite[] switchMenuSprites;
    public Image switchMenuImage;
    public Image[] healthbars = new Image[6];
    public GameObject[] fieldMoveObj;
    public CustomText[] fieldMoveText;
    //STATS1DATA
    public Image stats1portrait;
    public Image stat1bar;
    public CustomText pokedexNO, attacktext, speedtext, specialtext, defensetext, MonStatustext, monhptext, monnametext, montype1, montype2, owneridtext, ownernametext, monleveltext, monstatustext;
    //STATS2DATA
    public Image stats2portrait;
    public CustomText movetext, exptext, explefttoleveltext, nextleveltext, monname2text, pokedexno2;
    public List<PartyAnim> partyanims;
    Pokemon highlightedmon;
    public float partyAnimTimer = 0;
    public int switchMenuOffset, switchMenuOffsetX;
    public string[] fieldMoveNames = new string[4];
    public static PokemonMenu instance;

    void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        switchMenuOffset = 0;
        if (GameData.instance.party.Count == 0)
        {
            currentMenu = mainwindow;
            MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
            gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < GameData.instance.party.Count; i++)
        {
            int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[i].HP * 48 / GameData.instance.party[i].TotalHP);
            healthbars[i] = partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateMainMenu();
        }
        UpdateScreen();
        Dialogue.instance.Deactivate();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        StartCoroutine(Dialogue.instance.text("Choose a POKéMON."));
        Dialogue.instance.finishedText = true;
    }

    public void UpdateScreen()
    {
        int index = 0;
        foreach (Pokemon pokemon in GameData.instance.party)
        {
            Transform slottransform = partyslots[index].transform;
            int pixelCount = Mathf.RoundToInt((float)pokemon.HP * 48 / pokemon.TotalHP);
            slottransform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = (float)pixelCount / 48;
            slottransform.GetChild(2).GetComponent<CustomText>().text = pokemon.Name;
            slottransform.GetChild(3).GetComponent<CustomText>().text = ((pokemon.Level < 100) ? "<LEVEL>" : "") + pokemon.Level.ToString();
            slottransform.GetChild(4).GetComponent<CustomText>().text = (pokemon.HP > 99 ? "" : pokemon.HP > 9 ? " " : "  ") + pokemon.HP + (pokemon.TotalHP > 99 ? "/" : pokemon.TotalHP > 9 ? "/ " : "/  ") + pokemon.TotalHP;
            index++;
        }
    }

    public void UpdateMainMenu()
    {
        selectedOption = selectedMon;
        cursor.SetActive(true);
        for (int l = 0; l < 6; l++)
        {
            partyslots[l].SetActive(false);
        }
        for (int i = 0; i < GameData.instance.party.Count; i++)
        {
            if (i == 0)
            {
                numberofPokemon = 0;
            }
            if (GameData.instance.party[i].Name != "")
            {
                partyslots[i].SetActive(true);
                numberofPokemon++;
            }
        }
        UpdateMenus();
    }

    public void UpdateSwitch()
    {
        cursor.SetPosition(96 - 16 * switchMenuOffsetX, 40 - 16 * selectedOption + switchMenuOffset * 16);
        UpdateMenus();
    }

    // ToDo: Pokemon's Special, OT
    public void UpdateStats1()
    {
        cursor.SetActive(false);
        //stats1portrait.sprite = GameData.instance.frontMonSprites[(int)GameData.instance.party[selectedMon].Species];
        stats1portrait.sprite = GameData.instance.party[selectedMon].Species.GetFrontSprite();
        int id = (int)GameData.instance.party[selectedMon].Species;
        pokedexNO.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        attacktext.text = GameData.instance.party[selectedMon].ATK.ToString();
        speedtext.text = GameData.instance.party[selectedMon].SPE.ToString();
        specialtext.text = GameData.instance.party[selectedMon].SPA.ToString();
        defensetext.text = GameData.instance.party[selectedMon].DEF.ToString();
        PokemonUnity.Types type1 = GameData.instance.party[selectedMon].Type1, type2 = GameData.instance.party[selectedMon].Type2;
        montype1.text = PokemonData.GetTypeName(type1);
        string type2String = PokemonData.GetTypeName(type2);
        montype2.text = type2String != "" ? ("TYPE2/" + "\n " + type2String) : "";
        monnametext.text = GameData.instance.party[selectedMon].Name;
        owneridtext.text = GameData.instance.party[selectedMon].TrainerId;
        ownernametext.text = GameData.instance.party[selectedMon].TrainerId;
        int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[selectedMon].HP * 48 / (float)GameData.instance.party[selectedMon].TotalHP);
        stat1bar.fillAmount = (float)pixelCount / 48;
        monhptext.text = (GameData.instance.party[selectedMon].HP > 99 ? "" : GameData.instance.party[selectedMon].HP > 9 ? " " : "  ") + GameData.instance.party[selectedMon].HP + " " + GameData.instance.party[selectedMon].TotalHP;
        monleveltext.text = ((GameData.instance.party[selectedMon].Level < 100) ? "<LEVEL>" : "") + GameData.instance.party[selectedMon].Level.ToString();

        switch (GameData.instance.party[selectedMon].Status)
        {
            case Status.NONE:
                monstatustext.text = "OK";
                break;
            case Status.FAINT:
                monstatustext.text = "FNT";
                break;
            case Status.FROZEN:
                monstatustext.text = "FRZ";
                break;
            case Status.BURN:
                monstatustext.text = "BRN";
                break;
            case Status.PARALYSIS:
                monstatustext.text = "PRZ";
                break;
            case Status.POISON:
                monstatustext.text = "PSN";
                break;
            case Status.SLEEP:
                monstatustext.text = "SLP";
                break;
        }

        UpdateMenus();
    }

    // ToDo: Fix this
    public void UpdateStats2()
    {
        cursor.SetActive(false);
        string movestr = "";
        for (int i = 0; i < 4; i++)
        {
            //if (GameData.instance.party[selectedMon].SlotHasMove(i))
            //{
            //    Move move = GameData.instance.party[selectedMon].moves[i];
            //    movestr += (i > 0 ? "\n" : "") + move.name.ToUpper() + "\n" + "         " + "PP "  + (move.pp < 10 ? " " : "") + move.pp +  "/" + (move.maxpp < 10 ? " " : "") + move.maxpp;
            //}
            //else
            movestr += (i > 0 ? "\n" : "") + "-" + "\n" + "         " + "--";
        }
        movetext.text = movestr;
        stats2portrait.sprite = GameData.instance.party[selectedMon].Species.GetFrontSprite();
        monname2text.text = GameData.instance.party[selectedMon].Name;

        exptext.text = TruncateExpNumber(GameData.instance.party[selectedMon].Exp.ToString());
        explefttoleveltext.text = TruncateExpNumber((GameData.instance.party[selectedMon].Experience.NextLevel.ToString()));
        nextleveltext.text = (GameData.instance.party[selectedMon].Level < 99 ? "<LEVEL>" + (GameData.instance.party[selectedMon].Level + 1).ToString() : 100.ToString());
        int id = (int)GameData.instance.party[selectedMon].Species + 1;
        pokedexno2.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        UpdateMenus();
    }

    string TruncateExpNumber(string num)
    {
        // truncate the number to the last 6 digits
        return num.Length > 6 ? num.Substring(num.Length - 6) : num;
    }

    void UpdateMenus()
    {
        foreach (GameObject menu in allmenus)
        {
            menu.SetActive(menu == currentMenu);

            if (menu == switchstats && (currentMenu == mainwindow))
            {
                menu.SetActive(false);
            }
            if (menu == mainwindow && (currentMenu == switchstats))
            {
                menu.SetActive(true);
            }
        }

        if (currentMenu != switchstats) 
            DisableFieldText();
    }

    void DisableFieldText()
    {
        switchMenuOffsetX = 0;
        for (int i = 0; i < 4; i++)
        {
            fieldMoveObj[i].SetActive(false);
        }
    }

    void Update()
    {
        if (currentMenu == switchstats)
        {
            if (InputManager.Pressed(Button.Down))
            {
                selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 2 + switchMenuOffset);
                UpdateSwitch();
            }
            if (InputManager.Pressed(Button.Up))
            {
                selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 2 + switchMenuOffset);
                UpdateSwitch();
            }
        }

        if (currentMenu == mainwindow)
        {
            highlightedmon = GameData.instance.party[selectedOption];
            partyAnimTimer += 1;
            float hpratio = (float)highlightedmon.HP / highlightedmon.TotalHP;
            float animLoopTime = hpratio > .5f ? 10 : hpratio > .21f ? 32 : 64; //the animation takes 10,32, or 64 frames

            if (partyAnimTimer == animLoopTime)
            {
                partyAnimTimer = 0;
            }

            foreach (Pokemon pokemon in GameData.instance.party)
            {
                Transform slottransform = partyslots[GameData.instance.party.IndexOf(pokemon)].transform;
                if (GameData.instance.party.IndexOf(pokemon) != selectedOption)
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.GetPartySprite(pokemon.Species)].anim[0];
                }
                else
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.GetPartySprite(pokemon.Species)].anim[Mathf.FloorToInt(2f * partyAnimTimer / animLoopTime)];
                }
            }

            cursor.SetPosition(0, 128 - 16 * selectedOption);

            if (Dialogue.instance.finishedText || selectingPokemon)
            {
                if (InputManager.Pressed(Button.Down))
                {
                    selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, numberofPokemon - 1);
                }
                if (InputManager.Pressed(Button.Up))
                {
                    selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, numberofPokemon - 1);
                }
            }
        }

        if (InputManager.Pressed(Button.B) && Dialogue.instance.finishedText)
        {
            SoundManager.instance.PlayABSound();
            if (currentMenu == mainwindow)
            {
                InputManager.instance.DisableForSeconds(Button.B, 0.2f);
                Dialogue.instance.fastText = false;
                Dialogue.instance.Deactivate();
                InputManager.Enable(Button.Start);
                MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
                gameObject.SetActive(false);
            }
            else if (currentMenu == switchstats)
            {
                selectedOption = selectedMon;
                currentMenu = mainwindow;
                UpdateMainMenu();
            }
        }

        if (InputManager.Pressed(Button.A) && Dialogue.instance.finishedText)
        {
            SoundManager.instance.PlayABSound();

            if (currentMenu == mainwindow)
            {
                if (!switching)
                {
                    int textOffsetLength = 4;
                    switchMenuOffsetX = 0;
                    switchMenuOffset = 0;
                    int numberOfFieldMoves = 0;
                    int selectedMenu = 0;
                    selectedMon = selectedOption;
                    Pokemon selectedPokemon = GameData.instance.party[selectedMon];

                    for (int i = 0; i < 4; i++)
                    {
                        fieldMoveNames[i] = "";
                        fieldMoveObj[i].SetActive(false);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        //if(selectedPokemon.SlotHasMove(i)){
                        //    var move = selectedPokemon.moves[i];
                        //    if(isFieldMove(move)){
                        //        numberOfFieldMoves++;
                        //        if(move.name.Length > 6 && selectedMenu != 8){
                        //            selectedMenu = 4;
                        //            textOffsetLength = 2;
                        //        } 
                        //
                        //        if(move.name.Length > 8){
                        //            selectedMenu = 8;
                        //            textOffsetLength = 0;
                        //        }
                        //
                        //        fieldMoveNames[switchMenuOffset] = move.id.ToString();
                        //        fieldMoveObj[3 - switchMenuOffset].SetActive(true);
                        //
                        //        for(int j = 0; j < textOffsetLength; j++){
                        //            fieldMoveText[3 - switchMenuOffset].text += " ";  
                        //        }
                        //
                        //        switchMenuOffset++;
                        //    }
                        //}
                    }

                    for (int i = 0; i < numberOfFieldMoves; i++)
                    {
                        fieldMoveText[(4 - switchMenuOffset) + i].text += fieldMoveNames[i].ToUpper();
                    }

                    switchMenuOffsetX = selectedMenu / 4;
                    selectedMenu += switchMenuOffset;
                    switchMenuImage.sprite = switchMenuSprites[selectedMenu];

                    selectedOption = 0;
                    currentMenu = switchstats;
                    UpdateSwitch();
                }
                else
                {
                    Dialogue.instance.Deactivate();
                    StartCoroutine(Switch());
                    UpdateScreen();
                    StartCoroutine(Dialogue.instance.text("Choose a POKéMON."));
                    Dialogue.instance.finishedText = true;
                    switching = false;
                }
            }
            else if (currentMenu == switchstats)
            {
                if (selectedOption < switchMenuOffset)
                {
                    StartCoroutine(UseFieldMove(fieldMoveNames[selectedOption]));
                }

                if (selectedOption == switchMenuOffset)
                {
                    SoundManager.instance.SetMusicLow();
                    SoundManager.instance.PlayCry(GameData.instance.party[selectedMon].Species);
                    Dialogue.instance.Deactivate();
                    currentMenu = stats1;
                    UpdateStats1();
                }
                else if (selectedOption == switchMenuOffset + 1)
                {
                    switching = true;
                    selectedOption = selectedMon;
                    currentMenu = mainwindow;
                    StartCoroutine(Dialogue.instance.text("Move #MON&lwhere?"));
                    Dialogue.instance.finishedText = true;
                    UpdateMainMenu();
                }
                else if (selectedOption == switchMenuOffset + 2)
                {
                    selectedOption = selectedMon;
                    currentMenu = mainwindow;
                    UpdateMainMenu();
                }
            }
            else if (currentMenu == stats1)
            {
                currentMenu = stats2;
                UpdateStats2();
            }
            else if (currentMenu == stats2)
            {
                SoundManager.instance.SetMusicNormal();
                Dialogue.instance.Deactivate();
                Dialogue.instance.fastText = true;
                Dialogue.instance.keepTextOnScreen = true;
                Dialogue.instance.needButtonPress = false;
                StartCoroutine(Dialogue.instance.text("Choose a POKéMON."));
                Dialogue.instance.finishedText = true;
                selectedOption = selectedMon;
                currentMenu = mainwindow;
                UpdateMainMenu();
            }
        }
    }

    IEnumerator UseFieldMove(string whatFieldMove)
    {
        string monName = GameData.instance.party[selectedMon].Name;

        if (whatFieldMove == "CUT")
        {
            if (Player.instance.facingTree)
            {
                currentMenu = mainwindow;
                UpdateMenus();
                CloseMenu();
                Player.instance.Cut(monName);
                gameObject.SetActive(false);
                yield return 0;
            }
            else
            {
                currentMenu = mainwindow;
                UpdateMainMenu();
                yield return Dialogue.instance.text("There isn't&lanything to CUT!");
                //TODO: implement cutting grass
            }
        }
        if (whatFieldMove == "SURF")
        {
            Player.instance.UpdateFacedTile();
            if (Player.instance.facedTile.hasTile && Player.instance.facedTile.isWater)
            {
                SoundManager.instance.PlaySong(Music.Ocean);
                currentMenu = mainwindow;
                UpdateMainMenu();
                yield return Dialogue.instance.text(GameData.instance.playerName + " got on&l" + monName + "!");
                Player.instance.Surf();
                CloseMenu();
                gameObject.SetActive(false);
                yield return 0;
            }
            else
            {
                currentMenu = mainwindow;
                UpdateMainMenu();
                yield return Dialogue.instance.text("No SURFing on&l" + monName + " here!");
            }
        }
        if (whatFieldMove == "SOFT_BOILED")
        {
            while (selectedOption == selectedMon)
            {
                selectingPokemon = true;
                currentMenu = mainwindow;
                UpdateMainMenu();
                Dialogue.instance.fastText = true;
                Dialogue.instance.keepTextOnScreen = true;
                Dialogue.instance.needButtonPress = false;
                StartCoroutine(Dialogue.instance.text("Use item on which&l#MON?"));

                while (selectingPokemon)
                {
                    yield return new WaitForSeconds(0.01f);
                    if (InputManager.Pressed(Button.B)) yield return 0;
                    if (InputManager.Pressed(Button.A)) break;
                }

                selectingPokemon = false;
                Pokemon pokemon = GameData.instance.party[selectedOption];

                if (selectedOption != selectedMon)
                {
                    if (pokemon.HP != pokemon.TotalHP)
                    {
                        int amount = GameData.instance.party[selectedMon].TotalHP / 5;
                        yield return AnimateOurHealth(-amount, selectedMon);
                        yield return AnimateOurHealth(amount, selectedOption);
                        yield return Dialogue.instance.text(pokemon.Name + "&lrecovered by " + amount + "!");
                    }
                    else
                    {
                        yield return NoEffectText();
                    }
                }
            }
        }

        currentMenu = mainwindow;
        UpdateMainMenu();
    }


    IEnumerator NoEffectText() 
    {
        yield return Dialogue.instance.text("It won't have any&leffect.");
    }

    IEnumerator Switch()
    {
        //Swap selected Pokemon.
        Pokemon pokemon = GameData.instance.party[selectedOption];
        GameData.instance.party[selectedOption] = GameData.instance.party[selectedMon];
        GameData.instance.party[selectedMon] = pokemon;
        yield return null;
    }

    IEnumerator AnimateOurHealth(int amount, int i)
    {
        if (amount + GameData.instance.party[i].HP < 0) amount = GameData.instance.party[i].HP;
        if (amount + GameData.instance.party[i].HP > GameData.instance.party[i].TotalHP) amount = GameData.instance.party[i].TotalHP - GameData.instance.party[i].HP;
        int result = amount;
        WaitForSeconds wait = new WaitForSeconds(5 / GameData.instance.party[i].TotalHP);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            GameData.instance.party[i].HP += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[i].HP * 48 / (float)GameData.instance.party[i].TotalHP);
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateScreen();
        }
        yield return null;
    }

    public bool hasFieldMove(Pokemon pokemon)
    {
        foreach (Move move in pokemon.moves.Cast<Move>())
        {
            if (GameData.instance.fieldMoves.Contains(move.ID)) 
                return true;
        }
        return false;
    }

    public bool isFieldMove(Move move) => GameData.instance.fieldMoves.Contains(move.ID);

    public void CloseMenu()
    {
        InputManager.instance.DisableForSeconds(Button.B, 0.2f);
        Dialogue.instance.fastText = false;
        Dialogue.instance.Deactivate();
        InputManager.Enable(Button.Start);
        MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
    }
}