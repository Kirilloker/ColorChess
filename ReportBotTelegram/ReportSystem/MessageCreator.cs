
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
            case KeyBoardMessage.CountUniqueUsersAuthorization:
                return ReportCreator.CountUniqueUsersAuthorizationInRange(userMessage);
            case KeyBoardMessage.CountSearchGameWithTypeInRange:
                return ReportCreator.CountSearchGameWithTypeInRange(userMessage);
            case KeyBoardMessage.TimeGameWithTypeInRange:
                return ReportCreator.TimeGameWithTypeInRange(userMessage);
            case KeyBoardMessage.GeneralReportInRange:
                return ReportCreator.GeneralReportInRange(userMessage);
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
            case KeyBoardMessage.CountUniqueUsersAuthorization:
                return "Введите дату";
            case KeyBoardMessage.CountSearchGameWithTypeInRange:
                return "Введите дату";
            case KeyBoardMessage.TimeGameWithTypeInRange:
                return "Введите дату";
            case KeyBoardMessage.GeneralReportInRange:
                return "Введите дату";


            case KeyBoardMessage.GeneralReportAll:
                return ReportCreator.GeneralReportAll();
            case KeyBoardMessage.CountRegistrationAll:
                return ReportCreator.CountRegistrationUsersAll();
            case KeyBoardMessage.CountEndGameAll:
                return ReportCreator.CountEndGameAll();
            case KeyBoardMessage.CountGameNotFinishAll:
                return ReportCreator.CountGameNotFinishAll();
            case KeyBoardMessage.CountSearchGameWithTypeAll:
                return ReportCreator.CountSearchGameWithTypeAll();
            case KeyBoardMessage.TimeGameWithTypeAll:
                return ReportCreator.TimeGameWithTypeAll();


            case KeyBoardMessage.HowToWriteDate:
                return "Чтобы ввести промежуток нужно указать начальный и конечный период.\n" +
                    "Если ввести одну дату (то есть начальную), в конечный период будет подставлен нынешний момент \n" +
                    "Например: \"12.10\", \"10.09.2021\", \"20.10.2023 12:30\"\n" +
                    "Для того чтобы ввести конечный период, его нужно написать на следующей строке\n" +
                    "Например: \n\"12.10\n13.10\"";
        }

        return "Что-то не понятное";
    }
}