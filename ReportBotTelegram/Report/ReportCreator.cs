using Newtonsoft.Json;

public static class ReportCreator
{
    public static string UserInfo(string userInfoDTO) 
    {
        UserInfoDTO userInfo = JsonConvert.DeserializeObject<UserInfoDTO>(userInfoDTO);
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
}