public class PlayerStatistic
{
    public int Id { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int MaxScore { get; set; }
    public int Draw { get; set; }
    public int Rate { get; set; }

    // Связи
    public int? PLayerId { get; set; }
    public Player Player { get; set; }
}