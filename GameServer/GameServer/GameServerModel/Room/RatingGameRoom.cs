using ColorChessModel;

public class RatingGameRoom : GameRoom
{
    private Dictionary<int,int> _playersRate = new();
    private const int _ratingBet = 25;

    public RatingGameRoom(int MaxNumOfPlayers, List<int> PlayersIds, GameModeType GameMode) : base(MaxNumOfPlayers, PlayersIds, GameMode) { }
    public override void AddPlayer(int PlayerId)
    {
        _playersId.Add(PlayerId);
        _playersRate[PlayerId] = DB.GetUserStatistic(PlayerId).Rate;
    }
    public override void RemovePlayer(int playerId)
    {
        base.RemovePlayer(playerId);

        _playersRate.Remove(playerId);
    }
    public override void FinishTheGame()
    {
        List<int> playersScores = new();
        for (int i = 0; i < _playersId.Count; i++)
        {
            playersScores.Add(base._gameState.GetScorePlayer(i));
        }
        DB.AddGameStatistic(playersScores, _gameMode, _playersId);


        List<AttributeUS> GameResults = Enumerable.Repeat(AttributeUS.Draw, _maxNumOfPlayers).ToList();
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
        for (int i = 0; i < _playersId.Count; i++)
        {
            int playerScore = _gameState.GetScorePlayer(i);
            userStatistic = DB.GetUserStatistic(_playersId[i]);
            if (userStatistic.MaxScore < playerScore)
            {
                DB.ChangeUserStatistic(_playersId[i], AttributeUS.MaxScore, playerScore);
                
            }
            DB.ChangeUserStatistic(_playersId[i], GameResults[i], 1);

            if(GameResults[i] == AttributeUS.Win) DB.ChangeUserStatistic(_playersId[i], AttributeUS.Rate, _ratingBet);
            if (GameResults[i] == AttributeUS.Lose) DB.ChangeUserStatistic(_playersId[i], AttributeUS.Rate, -_ratingBet);
        }
    }
    public override void PlayerLeaveEndGame(int PlayerId)
    {
        DB.ChangeUserStatistic(PlayerId, AttributeUS.Lose, 1);
        DB.ChangeUserStatistic(PlayerId, AttributeUS.Rate, -_ratingBet);

        for(int i = 0; i < _playersId.Count; i++)
        {
            if (_playersId[i] != PlayerId) 
            { 
                if(_maxNumOfPlayers == 2)
                {
                    DB.ChangeUserStatistic(_playersId[i], AttributeUS.Win, 1);
                    DB.ChangeUserStatistic(_playersId[i], AttributeUS.Rate, _ratingBet);

                    List<int> playersScores = new();
                    
                    for (int j = 0; j < _playersId.Count; j++)
                        playersScores.Add(base._gameState.GetScorePlayer(j));
                    
                    DB.AddGameStatistic(playersScores, _gameMode, _playersId);
                    DB.AddGameStatistic(playersScores, _gameMode, _playersId);
                }
                else
                {
                    DB.ChangeUserStatistic(_playersId[i], AttributeUS.Rate, 5);
                }
            }
        }
    }
    public int AverageRating
    {
        get
        {
            int playersRate = 0;
            foreach (int rate in _playersRate.Values)
                playersRate += rate;
            playersRate = playersRate / _playersId.Count;
            return playersRate;
        }
    }
}