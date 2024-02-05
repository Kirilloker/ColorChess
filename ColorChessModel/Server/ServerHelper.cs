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


    static Map myMap;
    static Map newMap;
    public static bool NormalStep(string _step, string _map)
    {
        Step step = JSONConverter.ConvertJSONtoSTEP(_step);
        Map map = JSONConverter.ConvertJSONtoMap(_map);

        if (step.IsReal(myMap) == false)
            return false;

        newMap = GameStateCalcSystem.ApplyStep(myMap, step.Figure, step.Cell);

        if (myMap != map)
            return false;

        return true;
    }
}
