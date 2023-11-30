using System.Collections.Generic;
using System.Text.Json;

namespace GameServer
{
    static public class Test
    {
        static public async Task<IResult> GetTop(HttpContext context) 
        {
            string nameUser = context.Request.Query["name"];

            List<Pair<string,int>> result = DB.GetListTopRate(5);

            // Проверяем, есть ли указанный пользователь в топе
            bool userInTop = result.Any(pair => pair.First == nameUser);

            if (userInTop == false)
            {
                // Получаем рейтинг указанного пользователя и добавляем его в конец списка
                int userRate = DB.GetRateUser(nameUser);
                result.Add(new Pair<string, int>(nameUser, userRate));
            }

            string jsonString = JsonSerializer.Serialize(result);

            return Results.Content(jsonString);
        }
                
        static public async Task<IResult> GetCountPlayersOnline(HttpContext context)
        {
            var x = Results.Content(GameLobby.GetCountPlayersInGame().ToString());
            return x;
        }

        static public async  Task<IResult> GetNumberPlaceTop(HttpContext context)
        {
            string nameUser = context.Request.Query["name"];
            var x = Results.Content(DB.GetNumberPlaceUserByRate(nameUser).ToString());
            return x;
        }
    }
}
