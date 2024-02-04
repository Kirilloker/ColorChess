using ColorChessModel;
using ColorChessModel.Tools;

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
        for (int i = 0; i < PlayersIds.Count; i++)
        {
            if (PlayersIds[i] == PlayerId) PlayersIds.RemoveAt(i);
        }
    }
    public Map ApplyPlayerStep(int playerID, string _step) 
    {
        Step step = JsonConverter.ConvertJSONtoSTEP(_step);

        if (step.IsReal(GameState) == false)
        {
            return null;
        }

        GameState = GameStateCalcSystem.ApplyStep(GameState, step.Figure, step.Cell);
        return GameState;
    }
    public Map StartGame() 
    {
        GameStateBuilder builder = new GameStateBuilder();

        PlayerType[] playersTypes= new PlayerType[MaxNumOfPlayers];
        for(int i = 1; i < PlayersIds.Count; i++)
        {
            playersTypes[i] = PlayerType.Human;
        }

        CornerType[] cornerTypes= new CornerType[4] 
        {CornerType.DownLeft, CornerType.UpRight, CornerType.DownRight, CornerType.UpLeft };
        

        ColorType[] colorTypes = new ColorType[4] {ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow};


        builder.SetCustomGameState(9, playersTypes, cornerTypes, colorTypes);

        GameState = builder.CreateGameState();
        return GameState;
    }
    public virtual void EndGame()
    {
        List<int> playersScores = new();
        for (int i = 0; i < PlayersIds.Count; i++)
        {
            playersScores.Add(GameState.GetScorePlayer(i));
        }
        DB.AddGameStatistic(playersScores, GameMode, PlayersIds);


        List<AttributeUS> GameResults = Enumerable.Repeat(AttributeUS.Draw, MaxNumOfPlayers).ToList();
        int maxScore = int.MinValue;
        int maxIndex = 0;

        for (int i = 0; i < playersScores.Count; i++)
        {
            if (playersScores[i] > maxScore)
            {
                maxScore = playersScores[i];
                maxIndex = i;
            }
        }
        GameResults[maxIndex] = AttributeUS.Win;
        for (int i = 0; i < playersScores.Count; i++)
        {
            if (playersScores[i] == maxScore && i != maxIndex)
            {
                GameResults[i] = AttributeUS.Draw;
                GameResults[maxIndex] = AttributeUS.Draw;
            }
            if (playersScores[i] < maxScore) { GameResults[i] = AttributeUS.Lose; }
        }

        UserStatistic userStatistic;
        for (int i = 0; i < PlayersIds.Count; i++)
        {
            int playerScore = GameState.GetScorePlayer(i);
            userStatistic = DB.GetUserStatistic(PlayersIds[i]);
            if (userStatistic.MaxScore < playerScore)
            {
                DB.ChangeUserStatistic(PlayersIds[i], AttributeUS.MaxScore, playerScore);
            }
            DB.ChangeUserStatistic(PlayersIds[i], GameResults[i], 1);
        }
    }
    public virtual void PlayerLeaveEndGame(int PlayerId)
    {
        DB.ChangeUserStatistic(PlayerId, AttributeUS.Lose, 1);
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
}

