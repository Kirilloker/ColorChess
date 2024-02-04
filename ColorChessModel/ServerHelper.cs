using ColorChessModel;
using Newtonsoft.Json;

public class ServerHelper
{
    /*
    Игрок1 делает ход - отправляет класс Step(figure, cell) и свою Map (после сделанного хода)
    Сервер принимает Step - делает этот ход на своей Map и сравнивает с присланной Map от Игрока1
    Если они одинаковые - всё ок
    Если не одинаковые - ...
    Далее сервер отправляет остальным игрокам (всем кроме Игрок1) Step
    Игроки принимают этот ход и применяют его у себя (и возможно отправляют свою Map на проверку)

    */

    public static Map ConvertJSONtoMap(string JSON)
    {
        try
        {
            Print.Log("Test1");
            Map gameState = JsonConvert.DeserializeObject<Map>(JSON,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            Print.Log("Test2");

            return gameState;
        }
        catch (Exception ex)
        {
            Print.Log($"Exception during JSON deserialization: {ex}");
            throw; // Проброс исключения для дальнейшего анализа
        }
    }


    public static Step ConvertJSONtoSTEP(string JSON)
    {
        Step step = JsonConvert.DeserializeObject<Step>(JSON,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

        return step;
    }

    public static string ConvertToJSON(Map map)
    {
        return JsonConvert.SerializeObject(map, Formatting.Indented,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
    }

    public static string ConvertToJSON(Step step)
    {
        return JsonConvert.SerializeObject(step, Formatting.Indented,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
    }


    static Map myMap;
    static Map newMap;
    public static bool NormalStep(string _step, string _map)
    {
        Step step = ConvertJSONtoSTEP(_step);
        Map map = ConvertJSONtoMap(_map);

        if (step.IsReal(myMap) == false)
            return false;

        newMap = GameStateCalcSystem.ApplyStep(myMap, step.Figure, step.Cell);

        if (myMap != map)
            return false;

        return true;
    }
}
