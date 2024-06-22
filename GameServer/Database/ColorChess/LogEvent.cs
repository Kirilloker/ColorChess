public class LogEvent : IId
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public LogEventType? Type_Event { get; set; }
    public List<int>? UsersId { get; set; }
    public string? Description { get; set; } = "";
}