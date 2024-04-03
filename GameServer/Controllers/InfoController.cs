using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet("GetTop")]
        [SwaggerOperation(Summary = "Получение топ пользователей", Description = "Возвращает список топ пользователей с их рейтингом. Если указанный пользователь не в топе, добавляет его в конец списка")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Успешный запрос")]
        public async Task<IActionResult> GetTop([FromQuery][SwaggerParameter("Имя пользователя")] string nameUser)
        {
            List<Pair<string, int>> result = DB.GetListTopRate(5);

            bool userInTop = result.Any(pair => pair.First == nameUser);
            
            if (nameUser != "." && !userInTop)
            {
                int userRate = DB.GetRateUser(nameUser);
                result.Add(new Pair<string, int>(nameUser, userRate));
            }

            return new JsonResult(result);
        }

        [HttpGet("GetCountPlayersOnline")]
        [SwaggerOperation(Summary = "Получение количества онлайн игроков", Description = "Возвращает текущее количество игроков онлайн")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Успешный запрос")]
        public async Task<IActionResult> GetCountPlayersOnline()
        {
            int countPlayersOnline = GameLobby.GetCountPlayersInGame();
            return new JsonResult(countPlayersOnline);
        }

        [HttpGet("GetNumberPlaceTop")]
        [SwaggerOperation(Summary = "Получение места пользователя в топе", Description = "Возвращает место указанного пользователя в топе по рейтингу")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Успешный запрос")]
        public async Task<IActionResult> GetNumberPlaceTop([FromQuery][SwaggerParameter("Имя пользователя")] string nameUser)
        {
            int numberPlace = DB.GetNumberPlaceUserByRate(nameUser);
            return new JsonResult(numberPlace);
        }

        [HttpGet("GetInfoUsers")]
        public async Task<IActionResult> GetInfoUsers([FromQuery] string nameUser)
        {
            var user = DB.GetUser(nameUser);

            if (user == null)
            {
                return NotFound("Не удалось найти пользователя.");
            }

            var userStats = DB.GetUserStatistic(user.Id);
            if (userStats == null)
            {
                return NotFound("Статистика пользователя не найдена.");
            }

            int numberPlace = DB.GetNumberPlaceUserByRate(nameUser);

            var result = new { Wins = userStats.Win, Rate = userStats.Rate, NumberPlace = numberPlace };

            return Ok(result);
        }
    }
}
