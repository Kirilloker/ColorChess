using GameServer.Database;
using GameServer.Database.DTO;
using GameServer.Enum;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        [HttpGet("all")]
        [SwaggerOperation(
            Summary = "Получение всего списка пользователей",
            Description = "Возвращает всю таблицу Пользователей + Пользовательская статистика"
            )]
        [SwaggerResponse((int)APIResponseStatus.Success, "Успешный запрос")]
        [SwaggerResponse((int)APIResponseStatus.NotFound, "Не нашлись данные")]
        public async Task<IActionResult> GetUserInfoAll()
        {
            List<UserInfoDTO> users = Mapper.ListUserInfoToDTO(await DB.GetAllAsync<User>(), await DB.GetAllAsync<UserStatistic>());

            if (users == null || users.Count == 0)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            return new JsonResult(users);
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Получение информации о пользователе", 
            Description = "Возвращает информацию о пользователе + его статистика"
            )]
        [SwaggerResponse((int)APIResponseStatus.Success, "Успешный запрос")]
        [SwaggerResponse((int)APIResponseStatus.NotCorrect, "Не корректные данные")]
        [SwaggerResponse((int)APIResponseStatus.NotFound, "Не нашлись данные")]
        public async Task<IActionResult> GetUserInfo(
            [FromQuery][SwaggerParameter("ID пользователя")] string userId)
        {
            if (int.TryParse(userId, out int id) == false)
                return new ObjectResult("В параметре должно быть число") { StatusCode = (int)APIResponseStatus.NotCorrect };

            User? user = await DB.GetAsync<User>(id);
            UserStatistic? userStatistic = await DB.GetAsync<UserStatistic>(id);

            if (user == null || userStatistic == null)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            UserInfoDTO users = Mapper.UserInfoToDTO(user, userStatistic);

            if (users == null)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            return new JsonResult(users);
        }

        [HttpGet("byName")]
        [SwaggerOperation(
            Summary = "Получение информации о пользователе", 
            Description = "Возвращает информацию о пользователе + его статистика"
            )]
        [SwaggerResponse((int)APIResponseStatus.Success, "Успешный запрос")]
        [SwaggerResponse((int)APIResponseStatus.NotCorrect, "Не корректные данные")]
        [SwaggerResponse((int)APIResponseStatus.NotFound, "Не нашлись данные")]
        public async Task<IActionResult> GetUserInfoByName(
            [FromQuery][SwaggerParameter("Имя пользователя")] string userName)
        {
            if (userName == "" || userName == null)
                return new ObjectResult("В параметре должна быть строка") { StatusCode = (int)APIResponseStatus.NotCorrect };

            User? user = await DB.GetUserAsync(userName);

            if (user == null)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            UserStatistic? userStatistic = await DB.GetUserStatisticAsync(user.Id);
        
            if (userStatistic == null)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            UserInfoDTO? userInfo = Mapper.UserInfoToDTO(user, userStatistic);

            if (userInfo == null)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            return new JsonResult(userInfo);
        }


        [HttpPost]
        [SwaggerOperation(
            Summary = "Добавить пользователя", 
            Description = "Добавляет нового пользователя в таблицу Пользователя и Пользовательская статистика, и возвращает его копию"
            )]
        [SwaggerResponse((int)APIResponseStatus.Success, "Успешный запрос")]
        [SwaggerResponse((int)APIResponseStatus.UnKnown, "Не известная ошибка")]
        [SwaggerResponse((int)APIResponseStatus.AlreadyExist, "Данная запись уже существует в таблице")]
        public async Task<IActionResult> AddUser(
            [FromQuery][SwaggerParameter("Информация о новом пользователе")] UserDTO userDTO)
        {
            User user = Mapper.DTOToEntity(userDTO);

            if (await DB.GetUserAsync(user.Name) != null)
                return new ObjectResult("Пользователь с таким логином уже существует") { StatusCode = (int)APIResponseStatus.AlreadyExist };

            int? usedId = await DB.AddEntityAsync(user);

            if (usedId == null)
                return new ObjectResult("Произошла ошибка при создании пользователя") { StatusCode = (int)APIResponseStatus.UnKnown };


            UserStatistic userStatistic = new()
            {
                UserId = (int)usedId,
                Draw = 0,
                Lose = 0,
                Win = 0,
                MaxScore = 0,
                Rate = 30
            };

            if (DB.AddEntityAsync(userStatistic) == null)
                return new ObjectResult("Произошла ошибка при создании статистики пользователя") { StatusCode = (int)APIResponseStatus.UnKnown };

            UserInfoDTO userInfoDTO = Mapper.UserInfoToDTO(user, userStatistic);

            if (userInfoDTO == null)
                return new ObjectResult("Не удалось найти данные") { StatusCode = (int)APIResponseStatus.NotFound };

            return new JsonResult(userInfoDTO);
        }
    }
}