public class LogEventDTO
{
    public DateTime Date { get; set; }
    public TypeLogEvent Type_Event { get; set; }
    public List<int> UsersId { get; set; }
    public string? Description { get; set; }
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