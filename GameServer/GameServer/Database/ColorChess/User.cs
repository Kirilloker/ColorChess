public class User : IId
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";
}