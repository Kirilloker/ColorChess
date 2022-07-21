using ColorChessModel;
using Newtonsoft.Json;
using System.IO;

public static class TestSerialization
{
    public static void Save(Map map)
    {
        string json = JsonConvert.SerializeObject(map, Formatting.Indented,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

        StreamWriter file1 = File.CreateText("D://Github//GameColorChess//ColorChess//Demo_2//Assets//Code//SaveMap.json");
        System.Console.WriteLine(file1);
        file1.WriteLine(json);
        file1.Close();
    }

    public static Map Load()
    {
        string data = File.ReadAllText("D://Github//GameColorChess//ColorChess//Demo_2//Assets//Code//SaveMap.json");

        Map gameState = JsonConvert.DeserializeObject<Map>(data,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

        return gameState;
    }
}
