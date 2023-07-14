using ColorChessModel;

public class RatingGameRoom : GameRoom
{
    private Dictionary<int,int> PlayersRate = new();
    private int AverageRate = 0;

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

    public override Map ApplyPlayerStep(int playerID, string step)
    {
        throw new NotImplementedException();
    }
    public override Map StartGame()
    {
        throw new NotImplementedException();
    }

    public override void EndGame()
    {
        throw new NotImplementedException();
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