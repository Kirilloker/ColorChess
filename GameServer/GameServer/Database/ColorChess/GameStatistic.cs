public class GameStatistic
{
    public int Id { get; set; }
    public int Time { get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public DateTime Date { get; set; }
    public GameMode GameMode { get; set; }

    public int User1Id { get; set; }
    public int User2Id { get; set; }

}