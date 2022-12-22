using PokemonEssentials.Interface;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;
using PokemonUnity;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using System;
using UnityEngine;

using Pokemon = PokemonUnity.Monster.Pokemon;
using CombatPokemon = PokemonUnity.Combat.Pokemon;
using static PokemonUnity.Combat.Effects;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;
using PokemonUnity.UX;

public class BattleScene : MonoBehaviour, IPokeBattle_Scene
{
    #region Unity UI
    public GameObject PlayerMonObject, PlayerObject, TrainerObject, EnemyMonObject;
    public GameObject PlayerStats, EnemyStatsObject;
    public GameObject PlayerPokeballs, EnemyPokeballs;
    public GameObject CurrentMenu, BattleMenu, MovesMenu;
    public GameObject BattleBG;
    public GameObject BGTextBox;
    public GameObject BattleTransitionShaderObj;
    // Why??
    public GameObject[] AllMenus;
    
    public Material ShrinkSplitMat;

    public TilemapRenderer grassTilemap;

    public GameCursor Cursor;

    public int SelectedOption;
    public int CurrentLoadedMon;
    public int TransitionType;

    public bool isFadingIn;

    //public BattleType BattleType;

    public Image BattleOverlay, FrontPortrait, BackPortrait;
    public Sprite Blank;
    public Sprite[] PartyBallSprites;
    public Sprite[] BattleOverlaySprites;

    public AudioClip SendOutMonClip, RunClip;

    // PlayerMon
    public CustomText PlayerHPText, PlayerName, PlayerMonLevelText;
    public Image PlayerHPBar;
    public Image[] PlayerPartyBalls;
    // EnemyMon
    public CustomText EnemyMonLevelText, EnemyMonName;
    public Image EnemyHPBar;
    public Image[] EnemyPartyBalls;
    // END of PlayerMon and EnemyMon

    public Animator BattleMainAnim, BattleTransitionAnim;
    #endregion

    private int[] lastmove;
    public MenuCommands[] lastcmd { get; set; }

    public bool battlestart;
    public bool abortable;
    public bool aborted;

    private Battle battle;
    
    public bool inPartyAnimation => throw new NotImplementedException();

    public int Id { get; }

    public void Awake()
    {

    }

    public void Start()
    {
        /*
        Debug.Log("######################################");
        Debug.Log("# Hello - Welcome to Unity Battle!   #");
        Debug.Log("######################################");

        initialize();

        Pokemon[] p1 = new Pokemon[]
        {
            new Pokemon(Pokemons.BULBASAUR, 20, false),
            new Pokemon(Pokemons.MEW, 20, false),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
        };

        Pokemon[] p2 = new Pokemon[]
        {
            new Pokemon(Pokemons.CHARIZARD, 20, false),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
            new Pokemon(Pokemons.NONE),
        };

        PokemonUnity.Trainer player = new PokemonUnity.Trainer("RED", TrainerTypes.PLAYER);
        player.party = p1;

        Game.GameData.Trainer = player;

        PokemonUnity.Trainer opponent = new PokemonUnity.Trainer("FlakTester", TrainerTypes.WildPokemon);
        opponent.party = p2;

        //battle = new Battle(this,player.party, opponent.party, new PokemonUnity.Trainer[] { player }, new PokemonUnity.Trainer[] { opponent });
        battle = new Battle(this, player.party, opponent.party, player, opponent);
        battle.rules.Add("alwaysflee", false);
        battle.rules.Add("drawclause", false);
        battle.rules.Add(BattleRule.MODIFIEDSELFDESTRUCTCLAUSE, false);
        battle.rules.Add(BattleRule.SUDDENDEATH, false);

        battle.weather = Weather.NONE;

        battle.pbStartBattle(false);
        */
    }

    public IPokeBattle_Scene initialize()
    {
        throw new NotImplementedException();
    }

    public void pbUpdate()
    {
        throw new NotImplementedException();
    }

    public void pbGraphicsUpdate()
    {
        throw new NotImplementedException();
    }

    public void pbInputUpdate()
    {
        throw new NotImplementedException();
    }

    public void pbShowWindow(int windowtype)
    {
        throw new NotImplementedException();
    }

    public void pbSetMessageMode(bool mode)
    {
        throw new NotImplementedException();
    }

    public void pbWaitMessage()
    {
        throw new NotImplementedException();
    }

    public void pbDisplay(string msg, bool brief = false)
    {
        throw new NotImplementedException();
    }

    public IIconSprite pbAddSprite(string id, float x, float y, string filename, IViewport viewport)
    {
        throw new NotImplementedException();
    }

    public void pbAddPlane(string id, string filename, IViewport viewport)
    {
        throw new NotImplementedException();
    }

    public void pbDisposeSprites()
    {
        throw new NotImplementedException();
    }

    public void pbShowHelp(string text)
    {
        throw new NotImplementedException();
    }

    public void pbHideHelp()
    {
        throw new NotImplementedException();
    }

    public void pbBackdrop()
    {
        throw new NotImplementedException();
    }

    public void partyAnimationUpdate()
    {
        throw new NotImplementedException();
    }

    public void pbTrainerSendOut(int battlerindex, IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbSendOut(int battlerindex, IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbTrainerWithdraw(IBattle battle, IBattler pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbSafariStart()
    {
        throw new NotImplementedException();
    }

    public void pbResetCommandIndices()
    {
        throw new NotImplementedException();
    }

    public void pbResetMoveIndex(int index)
    {
        throw new NotImplementedException();
    }

    public int pbSafariCommandMenu(int index)
    {
        throw new NotImplementedException();
    }

    public int pbChooseMove(IPokemon pokemon, string message)
    {
        throw new NotImplementedException();
    }

    public string pbNameEntry(string helptext, IPokemon pokemon)
    {
        throw new NotImplementedException();
    }

    public void pbSelectBattler(int index, int selectmode = 1)
    {
        throw new NotImplementedException();
    }

    public int pbFirstTarget(int index, Targets targettype)
    {
        throw new NotImplementedException();
    }

    public void pbUpdateSelected(int index)
    {
        throw new NotImplementedException();
    }

    public void pbShowPokedex(Pokemons species, int form = 0)
    {
        throw new NotImplementedException();
    }

    public void pbChangePokemon(IBattler attacker, Forms pokemon)
    {
        throw new NotImplementedException();
    }

    public void pbSaveShadows(Action action = null)
    {
        throw new NotImplementedException();
    }

    public void pbFindAnimation(Moves moveid, int userIndex, int hitnum)
    {
        throw new NotImplementedException();
    }

    public void pbCommonAnimation(string animname, IBattler user, IBattler target, int hitnum = 0)
    {
        throw new NotImplementedException();
    }

    public void pbAnimationCore(string animation, IBattler user, IBattler target, bool oppmove = false)
    {
        throw new NotImplementedException();
    }

    public void pbLevelUp(IPokemon pokemon, IBattler battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
    {
        throw new NotImplementedException();
    }

    public void pbThrowAndDeflect(Items ball, int targetBattler)
    {
        throw new NotImplementedException();
    }

    public void pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer = false)
    {
        throw new NotImplementedException();
    }

    public void pbThrowSuccess()
    {
        throw new NotImplementedException();
    }

    public void pbHideCaptureBall()
    {
        throw new NotImplementedException();
    }

    public void pbThrowBait()
    {
        throw new NotImplementedException();
    }

    public void pbThrowRock()
    {
        throw new NotImplementedException();
    }

    public void pbFrameUpdate(IViewport cw)
    {
        throw new NotImplementedException();
    }

    public void pbPokemonString(IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public string pbMoveString(IBattleMove move)
    {
        throw new NotImplementedException();
    }

    public void pbFirstTarget(int index, int targettype)
    {
        throw new NotImplementedException();
    }

    public void pbNextTarget(int cur, int index)
    {
        throw new NotImplementedException();
    }

    public void pbPrevTarget(int cur, int index)
    {
        throw new NotImplementedException();
    }

    IPokeBattle_DebugSceneNoGraphics IPokeBattle_DebugSceneNoGraphics.initialize()
    {
        throw new NotImplementedException();
    }

    public void pbDisplayMessage(string msg, bool brief = false)
    {
        throw new NotImplementedException();
    }

    public void pbDisplayPausedMessage(string msg)
    {
        throw new NotImplementedException();
    }

    public bool pbDisplayConfirmMessage(string msg)
    {
        throw new NotImplementedException();
    }

    public bool pbShowCommands(string msg, string[] commands, bool defaultValue)
    {
        throw new NotImplementedException();
    }

    public int pbShowCommands(string msg, string[] commands, int defaultValue)
    {
        throw new NotImplementedException();
    }

    public void pbBeginCommandPhase()
    {
        throw new NotImplementedException();
    }

    public void pbStartBattle(IBattle battle)
    {
        throw new NotImplementedException();
    }

    public void pbEndBattle(BattleResults result)
    {
        throw new NotImplementedException();
    }

    public void pbTrainerSendOut(IBattle battle, IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbSendOut(IBattle battle, IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbTrainerWithdraw(IBattle battle, IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbWithdraw(IBattle battle, IPokemon pkmn)
    {
        throw new NotImplementedException();
    }

    public int pbForgetMove(IPokemon pkmn, Moves move)
    {
        throw new NotImplementedException();
    }

    public void pbBeginAttackPhase()
    {
        throw new NotImplementedException();
    }

    public int pbCommandMenu(int index)
    {
        throw new NotImplementedException();
    }

    public int pbFightMenu(int index)
    {
        throw new NotImplementedException();
    }

    public Items pbItemMenu(int index)
    {
        throw new NotImplementedException();
    }

    public int pbChooseTarget(int index, Targets targettype)
    {
        throw new NotImplementedException();
    }

    public void pbHPChanged(IBattler pkmn, int oldhp, bool anim = false)
    {
        throw new NotImplementedException();
    }

    public void pbFainted(IBattler pkmn)
    {
        throw new NotImplementedException();
    }

    public void pbWildBattleSuccess()
    {
        throw new NotImplementedException();
    }

    public void pbTrainerBattleSuccess()
    {
        throw new NotImplementedException();
    }

    public void pbEXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
    {
        throw new NotImplementedException();
    }

    public void pbLevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
    {
        throw new NotImplementedException();
    }

    public int pbBlitz(int keys)
    {
        throw new NotImplementedException();
    }

    public void pbShowOpponent(int opp)
    {
        throw new NotImplementedException();
    }

    public void pbHideOpponent()
    {
        throw new NotImplementedException();
    }

    public void pbRecall(int battlerindex)
    {
        throw new NotImplementedException();
    }

    public void pbDamageAnimation(IBattler pkmn, TypeEffective effectiveness)
    {
        throw new NotImplementedException();
    }

    public void pbBattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2)
    {
        throw new NotImplementedException();
    }

    public void pbBattleArenaBattlers(IBattle b1, IBattle b2)
    {
        throw new NotImplementedException();
    }

    public void pbCommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum = 0)
    {
        throw new NotImplementedException();
    }

    public void pbAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum = 0)
    {
        throw new NotImplementedException();
    }

    public void pbChatter(IBattler attacker, IBattler opponent)
    {
        throw new NotImplementedException();
    }

    public int pbSwitch(int index, bool lax, bool cancancel)
    {
        throw new NotImplementedException();
    }

    public void pbChooseEnemyCommand(int index)
    {
        throw new NotImplementedException();
    }

    public int pbChooseNewEnemy(int index, IPokemon[] party)
    {
        throw new NotImplementedException();
    }

    public void pbRefresh()
    {
        throw new NotImplementedException();
    }

    public void pbDisplay(string v)
    {
        throw new NotImplementedException();
    }

    public bool pbDisplayConfirm(string v)
    {
        throw new NotImplementedException();
    }
}
