public class Lobby
{
    public int Id { get; set; }
    public GameMode GameMode { get; set; }
    public List<int>? UsersId { get; set; }
}