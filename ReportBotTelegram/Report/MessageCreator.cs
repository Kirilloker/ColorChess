
public static class MessageCreator
{
    static string lastBotMes = "";
    static string lastUserMes = "";
    public static string GetReply(string userMessage) 
    {
        if (lastUserMes == "Получить информацию о пользователе") 
        {
            lastBotMes = ReportCreator.UserInfo(DataDeliver.GetUserInfo(userMessage));
            return lastBotMes;
        }

        if (lastUserMes == "Получить информацию о пользователе. TXT")
        {
            lastBotMes = ReportCreator.UserInfo(DataDeliver.GetUserInfo(userMessage));
            return "Высылаю документ.";
        }

        lastUserMes = userMessage;

        if (userMessage == null)
            return "Что-то не понятное";

        if (userMessage == "Получить информацию о пользователе") 
            return "Введите имя пользователя";

        if (userMessage == "Получить информацию о пользователе. TXT")
            return "Введите имя пользователя";

        return "Даже не знаю что и ответить";
    }
}