using System.Collections;

namespace Assets.Scripts.PokemonUnity
{
    public interface IPokeBattleIE : PokemonEssentials.Interface.Screen.IPokeBattle_Scene
    {
        public IEnumerator pbDisplay(string msg, bool brief = false);
    }
}
