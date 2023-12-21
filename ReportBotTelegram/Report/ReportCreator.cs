using Newtonsoft.Json;
using System.Net;

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
        var data = DataDeliver.GetCountRegistration(DateTime.MinValue, DateTime.MaxValue, "Registration");

        if (data.statusCode != ServerStatus.Success)
            return "Код результата: " + data.statusCode + "\n" + "Ответ сервера: " + data.content;


        List<LogEventDTO> logEvents = JsonConvert.DeserializeObject<List<LogEventDTO>>(data.content);

        return "Всего зарегистрированных пользователей: " + logEvents.Count;
    }

    public static string CountRegistrationUsersInRange(string userMessage) 
    {
        List<DateTime> dateTimes = Utill.GetDateTime(userMessage);

        if (dateTimes.Count == 0) return "Что-то не так с вводом даты";
       
        ResponseContentStatus data = DataDeliver.GetCountRegistration(dateTimes[0], dateTimes[1], "Registration");

        if (data.statusCode != ServerStatus.Success)
            return "Код результата: " + data.statusCode + "\n" + "Ответ сервера: " + data.content;

        List<LogEventDTO> logEvents = JsonConvert.DeserializeObject<List<LogEventDTO>>(data.content);

        return "Зарегистрированных пользователей с " + dateTimes[0].ToString() + " по " + dateTimes[1].ToString() + ": " + logEvents.Count; 
    }
}