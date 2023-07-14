using ColorChessModel;

public class DefaultGameRoom : GameRoom
{
    DefaultGameRoom(int MaxNumOfPlayers, List<int> PlayersIds, GameMode GameMode) : base(MaxNumOfPlayers, PlayersIds, GameMode) { }

    public override void AddPlayer(int PlayerId)
    {
        PlayersIds.Add(PlayerId);
    }

    public override void RemovePlayer(int PlayerId)
    {
        for (int i = 0; i < PlayersIds.Count; i++)
        {
            if (PlayersIds[i] == PlayerId) PlayersIds.RemoveAt(i);
        }
    }
    public override void EndGame() 
    {
    
    }

    public override Map ApplyPlayerStep(int playerID, string step)
    {
        throw new NotImplementedException();
    }
    public override Map StartGame()
    {
        throw new NotImplementedException();
    }
}

