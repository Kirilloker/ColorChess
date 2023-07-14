using ColorChessModel;

public class GameRoom
{
    protected List<int> PlayersIds;
    protected Map? GameState;
    protected int MaxNumOfPlayers;
    protected GameMode GameMode;

    public GameRoom(int MaxNumOfPlayers, List<int> PlayersIds, GameMode GameMode)
    {
        this.PlayersIds = PlayersIds;
        this.MaxNumOfPlayers = MaxNumOfPlayers;
        this.GameMode = GameMode;
    }
    public virtual void AddPlayer(int PlayerId) 
    { 
        PlayersIds.Add(PlayerId);
    }
    public virtual void RemovePlayer(int PlayerId) 
    {
        PlayersIds.Remove(PlayerId);
    }
    public virtual Map ApplyPlayerStep(int playerID, string step) 
    { 
        return null; 
    }
    public virtual Map StartGame() 
    { 
        return null; 
    }
    public virtual void EndGame() 
    { 

    }
    public bool IsFull
    {
        get
        {
            if (PlayersIds.Count == MaxNumOfPlayers) return true;
            else return false;
        }
    }
    public GameMode RoomGameMode
    {
        get
        {
            return GameMode;
        }
    }
    public int MaxPlayers
    {
        get { return  MaxNumOfPlayers; }
    }
    public List<int> PlayersInRoom
    {
        get { return new List<int>(PlayersIds);}
    }
    protected Map VerifyAndApplyStep(string _step, Map _map)
    {
        Step step = JsonConverter.ConvertJSONtoSTEP(_step);

        if (step.IsReal(_map) == false)
        {
            return null;
        }

        Map newMap = GameStateCalcSystem.ApplyStep(_map, step.Figure, step.Cell);
        return newMap;
    }
}

