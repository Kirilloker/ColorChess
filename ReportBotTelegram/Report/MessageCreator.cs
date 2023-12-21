
public static class MessageCreator
{
    public static string GetReply(string userMessage, string lastUserMes) 
    {
        switch (lastUserMes)
        {
            case KeyBoardMessage.InfoForUser:
                return ReportCreator.UserInfo(userMessage);
            case KeyBoardMessage.CountRegistrationInRange:
                return ReportCreator.CountRegistrationUsersInRange(userMessage);
        }

        if (userMessage == null)
            return "Пришло пустое сообщение";

        switch (userMessage)
        {
            case KeyBoardMessage.InfoForUser:
                return "Введите имя пользователя";
            case KeyBoardMessage.CountRegistrationAll:
                return ReportCreator.CountRegistrationUsersAll();
            case KeyBoardMessage.CountRegistrationInRange:
                return "Введите дату";
        }

        return "Что-то не понятное";
    }
}