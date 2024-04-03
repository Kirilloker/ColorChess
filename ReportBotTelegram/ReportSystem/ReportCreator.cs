using Newtonsoft.Json;
using System;
using System.Net;
using System.Numerics;
using System.Security;

public static class ReportCreator
{
    public static string GeneralReportAll() 
    {
        return GeneralReport();
    }

    public static string GeneralReportInRange(string userMessage)
    {
        return GeneralReport(userMessage);
    }

    public static string GeneralReport(string date = "")
    {
        string report = "";

        if (date == "") 
            report += "Общий отчет\n\n";
        else 
        {
            var dateTimes = HelpTools.GetDateTime(date);
            if (dateTimes.Count == 0)
                return "Что-то не так с вводом даты";
            
            report += "Отчет за период с " + dateTimes[0].ToString() + " по " + dateTimes[1].ToString() + "\n\n";
        }

        report += CountRegistrationUsers(date) + "\n\n";
        report += CountEndGame(date) + "\n\n";
        report += CountGameNotFinish(date) + "\n\n";
        report += TimeGameWithType(date) + "\n";
        report += CountSearchGameWithType(date) + "\n";

        if (date != "") 
            report += "\n" + CountUniqueUsersAuthorizationInRange(date);

        return report;
    }


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
        return CountRegistrationUsers();
    }

    public static string CountRegistrationUsersInRange(string userMessage)
    {
        return CountRegistrationUsers(userMessage);
    }

    public static string CountRegistrationUsers(string date = "") 
    {
        return GetCountRows("Registration", "Количество зарегистрированных пользователей: ", date);
    }

    public static string CountEndGameAll() 
    {
        return CountEndGame();
    }

    public static string CountEndGameInRange(string userMessage)
    {
        return CountEndGame(userMessage);
    }

    public static string CountEndGame(string date = "")
    {
        return GetCountRows("EndGame", "Количество завершенных игр: ", date);
    }


    public static string TimeGameWithTypeInRange(string userMessage)
    {
        return TimeGameWithType(userMessage);
    }

    public static string TimeGameWithTypeAll()
    {
        return TimeGameWithType();
    }

    public static string TimeGameWithType(string date = "")
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

        return HelpTools.CalculateGameTimes(listStartGame, listEndGame); 
    }

    public static string CountSearchGameWithTypeAll() 
    {
        return CountSearchGameWithType();
    }

    public static string CountSearchGameWithTypeInRange(string userMessage)
    {
        return CountSearchGameWithType(userMessage);
    }

    public static string CountSearchGameWithType(string date = "") 
    {
        WrapGetEvent listSearchGame = GetEventInfo("SearchGame", date);

        if (listSearchGame.errorMessage != "")
            return listSearchGame.errorMessage;

        var descriptionCounts = listSearchGame.logEventDTOs
            .GroupBy(le => le.Description)
            .Select(group => new { Description = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count);

        string reply = "Количество запускаемых поисков игры различных режимов:\n";

        foreach (var item in descriptionCounts)
            reply += item.Description.Substring(10) + " = " + item.Count + "\n";

        return reply;
    }

    public static string CountUniqueUsersAuthorizationInRange(string userMessage)
    {
        WrapGetEvent listAuthorizationUsers = GetEventInfo("Authorization", userMessage);

        if (listAuthorizationUsers.errorMessage != "")
            return listAuthorizationUsers.errorMessage;
        
        var uniqueCount = listAuthorizationUsers.logEventDTOs
            .GroupBy(le => string.Join(",", le.UsersId ?? Enumerable.Empty<int>()))
            .Count();

        return "Количество уникальных пользователей зашедших в игру: " + uniqueCount;
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
            dateTimes = HelpTools.GetDateTime(date);
               
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