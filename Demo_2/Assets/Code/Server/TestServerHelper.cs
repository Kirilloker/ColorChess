using ColorChessModel;
using Newtonsoft.Json;
using System.Collections.Generic;

public class TestServerHelper
{
    /*
    Игрок1 делает ход - отправляет класс Step(figure, cell) и свою Map (после сделанного хода)
    Сервер принимает Step - делает этот ход на своей Map и сравнивает с присланной Map от Игрока1
    Если они одинаковые - всё ок
    Если не оддинаковые - ...
    Далее сервер отправляет остальным игрокам (всем кроме Игрок1) Step
    Игроки принимают этот ход и применяют его у себя (и возможно отправляют свою Map на проверку)

    */

    public static Map ConvertJSONtoMap(string JSON)
    {
        Map gameState = JsonConvert.DeserializeObject<Map>(JSON,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

        return gameState;
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
    public static bool TestCheckStep(string _step, string _map)
    {
        Step step = ConvertJSONtoSTEP(_step);
        Map map = ConvertJSONtoMap(_map);

        if (step.IsReal(myMap) == false)
            return false;

        Map newMap = GameStateCalcSystem.ApplyStep(myMap, step.Figure, step.Cell);

        if (myMap != map)
            return false;

        return true;
    }
    

    public static void TestServerStep()
    {
        string step = "Step JSON";
        string map = "Map JSON";

        if (TestCheckStep(step, map) == true)
        {

        }
        else
        {
            DebugConsole.Print("Что-то пошло не так");
        }
    }

}

public class Step
{
    private Figure figure;
    private Cell cell;

    public Step(Figure _figure, Cell _cell)
    {
        this.figure = _figure;
        this.cell = _cell;
    }

    public Figure Figure { get { return figure; } set { figure = value; } }
    public Cell Cell { get { return cell; } set { cell = value; } }

    public bool IsReal(Map map)
    {
        List<Cell> allSteps = WayCalcSystem.CalcAllSteps(map, figure);

        return allSteps.Contains(cell);
    }
}