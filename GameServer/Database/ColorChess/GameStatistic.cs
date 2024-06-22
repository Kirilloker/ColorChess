public class GameStatistic : IId
{
    public int Id { get; set; }
    public List<int>? PlayerScore { get; set; }
    public GameModeType GameMode { get; set; }
    public List<int>? UsersId { get; set; }
}