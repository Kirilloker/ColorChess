using GameServer.Database.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    string password_db = "qwerty";


    [HttpGet("all")]
    [SwaggerOperation(Summary = "Получение всего списка пользователей", Description = "Возвращает всю таблицу Пользователей (пароль зашифрован) + Пользовательская статистика")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse(402, "Не нашлись данные")]
    public IActionResult GetUserInfoAll()
    {
        List<UserInfoHashDTO> users = Converter.List_UserInfoHashToDTO(DB.GetAllUser(), DB.GetAllUserStatistic());

        if (users == null || users.Count == 0)
            return new ObjectResult("Не удалось найти данные") { StatusCode = 402 };

        return new JsonResult(users);
    }


    [HttpGet]
    [SwaggerOperation(Summary = "Получение информации о пользователе", Description = "Возвращает информацию о пользователе (пароль зашифрован) + его статистика")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse(400, "Не корректные данные")]
    [SwaggerResponse(402, "Не нашлись данные")]
    public IActionResult GetUserInfo(
        [FromQuery][SwaggerParameter("ID пользователя")] string userId)
    {
        if (int.TryParse(userId, out int id) == false)
            return new ObjectResult("В параметре должно быть число") { StatusCode = 400 };

        UserInfoHashDTO users = Converter.UserInfoHashToDTO(DB.Get<User>(id), DB.Get<UserStatistic>(id));

        if (users == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = 402 };

        return new JsonResult(users);
    }




}