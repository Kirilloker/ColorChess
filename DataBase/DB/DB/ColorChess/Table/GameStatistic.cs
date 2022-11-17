using System;

public class GameStatistic
{
    public int Id { get; set; }
    public int Time { get; set; }
    public int ScorePlayer1 { get; set; }
    public int ScorePlayer2 { get; set; }
    public DateTime Data { get; set; }

    // Связи
    public int? Player1Id { get; set; }
    public int? Player2Id { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
}