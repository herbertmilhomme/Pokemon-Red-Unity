[System.Serializable]
public class Move {
    public PokemonUnity.Moves ID;
    public int moveIndex;
    public string name;
    public int pp;
    public int maxpp;
    public PokemonUnity.Types type;


    public Move(PokemonUnity.Moves index)
    {
        ID = index;
        this.moveIndex = (int)index;

        var moveData = PokemonData.GetMoveData(index);
        name = moveData.ID.ToString();
        maxpp = moveData.PP;
        pp = maxpp;
        type = moveData.Type;
    }
    public Move()
    {
        ID = PokemonUnity.Moves.NONE;
        this.moveIndex = 0;

        maxpp = 0;
        pp = 0;
        type = PokemonUnity.Types.NONE; //Look at ths later
    }
}

public enum Status {
    Ok,
    Sleep,
    Burn,
    Poison,
    Paralyzed,
    Frozen,
    Fainted
}