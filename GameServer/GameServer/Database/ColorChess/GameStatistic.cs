public class GameStatistic
{
    public int Id { get; set; }
    public int Time { get; set; }
    public List<int>? PlayerScore { get; set; }
    public DateTime Date { get; set; }
    public GameMode GameMode { get; set; }
    public List<int>? UsersId { get; set; }

}