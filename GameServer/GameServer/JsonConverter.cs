using ColorChessModel;
using Newtonsoft.Json;

static public class JsonConverter
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
        return JsonConvert.DeserializeObject<Map>(JSON,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        ;
    }

    public static Step ConvertJSONtoSTEP(string JSON)
    {
        return JsonConvert.DeserializeObject<Step>(JSON,
        new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }); ;
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


}

public class Step
{
    private Figure figure;
    private Cell cell;

    public Step(Figure _figure, Cell _cell)
    {
        // Тут надо что-то сделать но пока не очень понятно
        this.figure = _figure;
        this.cell = _cell;
    }

    public Figure Figure { get { return figure; } set { figure = value; } }
    public Cell Cell { get { return cell; } set { cell = value; } }

    public bool IsReal(Map map)
    {
        List<Cell> allSteps = WayCalcSystem.CalcAllSteps(map, figure);

        foreach(Cell step in allSteps)
        {
            if (step == cell) return true;
        }

        return false;
    }
}