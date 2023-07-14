public class Room
{
    public int Id { get; set; }
    public string? Map { get; set; }
    public GameMode GameMode { get; set; }
    public List<int>? UsersId { get; set; }

}