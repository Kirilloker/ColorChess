using ColorChessModel;

public class GameRoom
{
    protected List<int> _playersId;
    protected Map _gameState;
    protected int _maxNumOfPlayers;
    protected GameModeType _gameMode;

    public GameRoom(int maxNumOfPlayers, List<int> playersId, GameModeType gameMode)
    {
        _playersId = playersId;
        _maxNumOfPlayers = maxNumOfPlayers;
        _gameMode = gameMode;
    }

    public virtual void AddPlayer(int playerId) 
    { 
        if (_playersId.Contains(playerId) == false)
            _playersId.Add(playerId);
    }

    public virtual void RemovePlayer(int playerId)
    {
        _playersId.RemoveAll(id => id == playerId);
    }

    public bool TryApplyStep(Step step) 
    {
        if (_gameState.IsPermissibleStep(step) == false)
            return false;
        
        _gameState = GameStateCalcSystem.ApplyStep(_gameState, step.Figure, step.Cell);
    
        return true;
    }
    public virtual void FinishTheGame()
    {
        List<int> playersScore = _gameState.GetListScorePlayer();

        AddGameStatistic(playersScore, _gameMode, _playersId);
        ChangeUserStatistic(playersScore, _playersId);
    }
    public virtual void PlayerLeaveEndGame(int PlayerId)
    {
        DB.ChangeUserStatistic(PlayerId, AttributeUS.Lose, 1);
    }

    public Map InitMap() 
    {
        GameStateBuilder builder = new GameStateBuilder();

        PlayerType[] playersTypes= new PlayerType[_maxNumOfPlayers];

        foreach (var playerId in _playersId.Skip(1))
            playersTypes[playerId] = PlayerType.Human;

        CornerType[] cornerTypes= new CornerType[4] 
            {CornerType.DownLeft, CornerType.UpRight, CornerType.DownRight, CornerType.UpLeft };
        
        ColorType[] colorTypes = new ColorType[4] 
            {ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow};

        const int sizeMap = 9;

        builder.SetCustomGameState(sizeMap, playersTypes, cornerTypes, colorTypes);

        _gameState = builder.CreateGameState();
        return _gameState;
    }


    private void AddGameStatistic(List<int> playersScore, GameModeType gameMode, List<int> playersId)
    {
        DB.AddGameStatistic(playersScore, gameMode, playersId);
    }

    private void ChangeUserStatistic(List<int> playersScore, List<int> playersId)
    {
        var gameResults = CalculateGameResult(playersScore);

        for (int i = 0; i < playersId.Count; i++)
        {
            UserStatistic? userStatistic = DB.GetUserStatistic(playersId[i]);

            if (userStatistic == null)
                throw new Exception("Not found User Statistic in scoring ");

            if (userStatistic.MaxScore < playersScore[i])
                DB.ChangeUserStatistic(playersId[i], AttributeUS.MaxScore, playersScore[i]);

            DB.ChangeUserStatistic(playersId[i], gameResults[i], 1);
        }
    }

    private List<AttributeUS> CalculateGameResult(List<int> playersScore)
    {
        List<AttributeUS> gameResults = Enumerable.Repeat(AttributeUS.Draw, _maxNumOfPlayers).ToList();

        int maxScore = playersScore.Max();
        int maxIndex = playersScore.IndexOf(maxScore);

        gameResults[maxIndex] = AttributeUS.Win;

        for (int i = 0; i < playersScore.Count; i++)
        {
            if (playersScore[i] == maxScore && i != maxIndex)
            {
                gameResults[i] = AttributeUS.Draw;
                gameResults[maxIndex] = AttributeUS.Draw;
            }
            if (playersScore[i] < maxScore)
            {
                gameResults[i] = AttributeUS.Lose;
            }
        }

        return gameResults;
    }



    public bool IsFull => _playersId.Count == _maxNumOfPlayers;
    public GameModeType RoomGameMode => _gameMode;
    public int MaxPlayers => _maxNumOfPlayers;
    public List<int> PlayersIdInRoom => new List<int>(_playersId);
    public bool EndGame => _gameState.EndGame;
}

