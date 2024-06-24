using GameServer.Database;
using GameServer.GameServer.GameServerModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet("get_top")]
        [SwaggerOperation(
            Summary = "Получение топ пользователей", 
            Description = "Возвращает список пользователей с наивысшим рейтингом. " +
            "Если указанный пользователь не в топе, добавляет его в конец списка" +
            "Если в имени пользователя указать точку (.), то добавление в конец не происходит"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Успешный запрос")]
        public async Task<IActionResult> GetTop([FromQuery][SwaggerParameter("Имя пользователя")] string nameUser)
        {
            var result = await DB.GetListTopRateAsync(5);
            
            if (nameUser != "." && !result.Any(pair => pair.Key == nameUser))
            {
                int? userRate = await DB.GetRateUserAsync(nameUser);

                if (userRate == null)
                    return NotFound("Не удалось найти рейтинг пользователя.");

                result.Add(new (nameUser, userRate.Value));
            }

            return new JsonResult(result);
        }



        [HttpGet("get_count_players_online")]
        [SwaggerOperation(Summary = "Получение количества онлайн игроков")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Успешный запрос")]
        public IActionResult GetCountPlayersOnline() 
            => new JsonResult(GameLobby.GetCountPlayersInGame());



        [HttpGet("get_number_place_top")]
        [SwaggerOperation(Summary = "Получение места пользователя в топе рейтингу")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Успешный запрос")]
        public async Task<IActionResult> GetNumberPlaceTop([FromQuery][SwaggerParameter("Имя пользователя")] string nameUser) 
            => new JsonResult(await DB.GetNumberPlaceUserByRateAsync(nameUser));



        [HttpGet("get_info_users")]
        [SwaggerOperation(
            Summary = "Получение информации о пользователе",
            Description = "Возвращает список с количеством побед, рейтингом и номером в топе"
        )]
        public async Task<IActionResult> GetInfoUsers([FromQuery][SwaggerParameter("Имя пользователя")] string nameUser)
        {
            var user = await DB.GetUserAsync(nameUser);

            if (user == null)
                return NotFound("Не удалось найти пользователя.");

            var userStats = await DB.GetUserStatisticAsync(user.Id);

            if (userStats == null)
                return NotFound("Статистика пользователя не найдена.");

            int? numberPlace = await DB.GetNumberPlaceUserByRateAsync(nameUser);

            if (numberPlace == null)
                return NotFound("Не удалось найти место в топе.");

            var result = new { Wins = userStats.Win, userStats.Rate, NumberPlace = numberPlace };

            return Ok(result);
        }
    }
}
