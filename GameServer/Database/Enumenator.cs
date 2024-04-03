public static class Error
{
    public static readonly string NotFound = "Not Found";
    public static readonly string Unknown = "Unknown";
    public static readonly string UnknownAttribute = "Unknown Attribute";
    public static readonly string NotFoundOpponent = "Not Found Opponent";
    public static readonly string NotFoundEnum = "Not Found Enum";
    public static readonly string UserExist = "This user exist";
    public static readonly string RoomExist = "This room exist";
    public static readonly string UserStatisticExist = "This user statistic exist";
    public static readonly string UserInLobbyExist = "This user in Lobby exist";
}

public enum AttributeUS
{
    Win = 0,
    Lose = 1,
    MaxScore = 2,
    Draw = 3,
    Rate = 4,
}

public enum GameMode
{
    Default = 0,
    Rating = 1,
    Custom = 2,
}

public enum TypeLogEvent
{
    Registration,
    Authorization,
    SearchGame,
    StartGame,
    EndGame,
    SurrenderGame
}

public interface IId
{
    int Id { get; set; }
}