namespace GameServer.Database.DTO
{
    public class LogEventDTO
    {
        public DateTime? Date { get; set; }
        public LogEventType? Type_Event { get; set; }
        public List<int>? UsersId { get; set; }
        public string? Description { get; set; }
    }
}
