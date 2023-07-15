using ColorChessModel;

public class RatingGameRoom : GameRoom
{
    private Dictionary<int,int> PlayersRate = new();
    private int AverageRate = 0;
    private const int RatingBet = 25;

    public RatingGameRoom(int MaxNumOfPlayers, List<int> PlayersIds, GameMode GameMode) : base(MaxNumOfPlayers, PlayersIds, GameMode) { }
    public override void AddPlayer(int PlayerId)
    {
        PlayersIds.Add(PlayerId);
        PlayersRate[PlayerId] = DB.GetUserStatistic(PlayerId).Rate;
    }
    public override void RemovePlayer(int PlayerId)
    {
        for (int i = 0; i < PlayersIds.Count; i++)
        {
            if (PlayersIds[i] == PlayerId) PlayersIds.RemoveAt(i);
        }
        PlayersRate.Remove(PlayerId);
    }
    public override void EndGame()
    {
        List<int> playersScores = new();
        for (int i = 0; i < PlayersIds.Count; i++)
        {
            playersScores.Add(base.GameState.GetScorePlayer(i));
        }
        DB.AddGameStatistic(playersScores, GameMode, PlayersIds);


        List<AttributeUS> GameResults = new List<AttributeUS>(MaxNumOfPlayers);
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

            if(GameResults[i] == AttributeUS.Win) DB.ChangeUserStatistic(PlayersIds[i], AttributeUS.Rate, RatingBet);
            if (GameResults[i] == AttributeUS.Lose) DB.ChangeUserStatistic(PlayersIds[i], AttributeUS.Rate, -RatingBet);
        }
    }
    public override void PlayerLeaveEndGame(int PlayerId)
    {
        DB.ChangeUserStatistic(PlayerId, AttributeUS.Lose, 1);
        DB.ChangeUserStatistic(PlayerId, AttributeUS.Rate, -RatingBet);

        for(int i = 0; i < PlayersIds.Count; i++)
        {
            if (PlayersIds[i] != PlayerId) 
            { 
                if(MaxNumOfPlayers == 2)
                {
                    DB.ChangeUserStatistic(PlayersIds[i], AttributeUS.Win, 1);
                    DB.ChangeUserStatistic(PlayersIds[i], AttributeUS.Rate, RatingBet);
                    List<int> playersScores = new();
                    for (int j = 0; j < PlayersIds.Count; j++)
                    {
                        playersScores.Add(base.GameState.GetScorePlayer(j));
                    }
                    DB.AddGameStatistic (playersScores,GameMode, PlayersIds);
                    DB.AddGameStatistic(playersScores, GameMode, PlayersIds);
                }
                else
                {
                    DB.ChangeUserStatistic(PlayersIds[i], AttributeUS.Rate, 5);
                }
            }
        }
    }
    public int AverageRating
    {
        get
        {
            int playersRate = 0;
            foreach (int rate in PlayersRate.Values)
                playersRate += rate;
            playersRate = playersRate / PlayersIds.Count;
            return playersRate;
        }
    }
}