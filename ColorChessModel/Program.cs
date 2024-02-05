using ColorChessModel;

class TestClass
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World");
        GameStateBuilder gameStateBuilder = new();

        gameStateBuilder.SetDefaultHotSeatGameState();
        var map = gameStateBuilder.CreateGameState();
        Console.WriteLine(map.CountEmptyCell);

        gameStateBuilder.SetDefaultOnlineGameState();
        map = gameStateBuilder.CreateGameState();
        Console.WriteLine(map.CountEmptyCell);
    }
}