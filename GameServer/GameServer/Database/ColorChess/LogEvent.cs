public class LogEvent
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public TypeLogEvent? Type_Event { get; set; }
    public List<int>? UsersId { get; set; } 
    public string? Description { get; set; }
}