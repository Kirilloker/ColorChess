
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
            case KeyBoardMessage.CountEndGameInRange:
                return ReportCreator.CountGameNotFinishInRange(userMessage);
            case KeyBoardMessage.CountGameNotFinishInRange:
                return ReportCreator.CountGameNotFinishInRange(userMessage);
        }

        if (userMessage == null)
            return "Пришло пустое сообщение";

        switch (userMessage)
        {
            case KeyBoardMessage.InfoForUser:
                return "Введите имя пользователя";
            case KeyBoardMessage.CountRegistrationInRange:
                return "Введите дату";
            case KeyBoardMessage.CountGameNotFinishInRange:
                return "Введите дату";


            case KeyBoardMessage.CountRegistrationAll:
                return ReportCreator.CountRegistrationUsersAll();
            case KeyBoardMessage.CountEndGameAll:
                return ReportCreator.CountEndGameAll();
            case KeyBoardMessage.CountGameNotFinishAll:
                return ReportCreator.CountGameNotFinishAll();
        }

        return "Что-то не понятное";
    }
}