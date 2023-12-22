using Newtonsoft.Json;
using System.Net;
using System.Numerics;

public static class ReportCreator
{
    public static string UserInfo(string userMessage) 
    {
        var data = DataDeliver.GetUserInfo(userMessage);

        if (data.statusCode != ServerStatus.Success)
            return "Код результата: " + data.statusCode + "\n" + "Ответ сервера: " + data.content;


        UserInfoDTO userInfo = JsonConvert.DeserializeObject<UserInfoDTO>(data.content);
        if (userInfo == null) return "Error!";

        string reply = "";

        reply += "Имя: " + userInfo.Name + "\n";
        reply += "Пароль: " + userInfo.Password + "\n";
        reply += "Рейтинг: " + userInfo.Rate + "\n";
        reply += "Количество побед: " + userInfo.Win + "\n";
        reply += "Количество поражений: " + userInfo.Lose + "\n";
        reply += "Количество ничьи: " + userInfo.Draw + "\n";
        reply += "Максимальное количество очков за игру: " + userInfo.MaxScore + "\n";

        return reply;
    }

    public static string CountRegistrationUsersAll() 
    {
        return GetCountRows("Registration", "Количество всего зарегистрированных пользователей: ");
    }

    public static string CountEndGameAll() 
    {
        return GetCountRows("EndGame", "Количество всего завершенных игр: ");
    }

    public static string CountGameNotFinishAll()
    {
        return CountGameNotFinish();
    }

    public static string CountGameNotFinishInRange(string userMessage)
    {
        return CountGameNotFinish(userMessage);
    }

    public static string CountGameNotFinish(string date = "") 
    {
        WrapGetEvent listStartGame = GetEventInfo("StartGame", date);
        WrapGetEvent listEndGame = GetEventInfo("EndGame", date);

        string errorAnswer = "";

        if (listStartGame.errorMessage != "")
            errorAnswer = listStartGame.errorMessage + "\n";

        if (listEndGame.errorMessage != "")
            errorAnswer = listEndGame.errorMessage + "\n";

        if (errorAnswer != "")
            return errorAnswer;

        int subtraction = (listStartGame.logEventDTOs.Count - listEndGame.logEventDTOs.Count);

        return "Количество не завершенных игр: " + subtraction + " \n Это " + (subtraction / listEndGame.logEventDTOs.Count) * 100 + "% от завершенных";
    }



    public static string CountRegistrationUsersInRange(string userMessage)
    {
        return GetCountRows("Registration", "Количество зарегистрированных пользователей в этом промежутке: ", userMessage);
    }

    public static string CountEndGameInRange(string userMessage)
    {
        return GetCountRows("EndGame", "Количество завершенных игр в этом промежутке: ", userMessage);
    }



    private static string GetCountRows(string types, string blank, string date = "") 
    {
        WrapGetEvent reply = GetEventInfo(types);

        if (reply.errorMessage == null || reply.errorMessage == "")
            return blank + reply.logEventDTOs.Count;
        else
            return reply.errorMessage;
    }


    private static WrapGetEvent GetEventInfo(string types, string date = "") 
    {
        List<DateTime> dateTimes = new List<DateTime>();

        if (date == "") 
        {
            dateTimes.Add(DateTime.MinValue);
            dateTimes.Add(DateTime.MaxValue);
        }
        else 
        {
            dateTimes = Utill.GetDateTime(date);
               
            if (dateTimes.Count == 0) 
            {
                return new WrapGetEvent(new(), "Что-то не так с вводом даты");
            }
        }

        ResponseContentStatus data = DataDeliver.GetEventTable(dateTimes[0], dateTimes[1], types);

        if (data.statusCode != ServerStatus.Success)
            return new WrapGetEvent(new(), "Код результата: " + data.statusCode + "\n" + "Ответ сервера: " + data.content);

        return new WrapGetEvent(JsonConvert.DeserializeObject<List<LogEventDTO>>(data.content));
    }
}

public struct WrapGetEvent 
{
    public List<LogEventDTO> logEventDTOs = new();
    public string errorMessage = "";

    public WrapGetEvent(List<LogEventDTO> logEvents, string errorMes = "") 
    {
        this.logEventDTOs = logEvents;
        this.errorMessage = errorMes;
    }
}