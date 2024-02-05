using GameServer.Database.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Reflection.PortableExecutable;

[ApiController]
[Route("api/[controller]")]
public class UserInfoController : ControllerBase
{
    string password_db = "qwerty";


    [HttpGet("all_with_hash")]
    [SwaggerOperation(Summary = "Получение всего списка пользователей", Description = "Возвращает всю таблицу Пользователей (пароль зашифрован) + Пользовательская статистика")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetUserInfoAll()
    {
        List<UserInfoHashDTO> users = Mapper.List_UserInfoHashToDTO(DB.GetAll<User>(), DB.GetAll<UserStatistic>());

        if (users == null || users.Count == 0)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(users);
    }


    [HttpGet("with_hash")]
    [SwaggerOperation(Summary = "Получение информации о пользователе", Description = "Возвращает информацию о пользователе (пароль зашифрован) + его статистика")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetUserInfo(
        [FromQuery][SwaggerParameter("ID пользователя")] string userId)
    {
        if (int.TryParse(userId, out int id) == false)
            return new ObjectResult("В параметре должно быть число") { StatusCode = (int)ServerStatus.NotCorrect };

        UserInfoHashDTO users = Mapper.UserInfoHashToDTO(DB.Get<User>(id), DB.Get<UserStatistic>(id));

        if (users == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(users);
    }



    [HttpGet("all")]
    [SwaggerOperation(Summary = "Получение всего списка пользователей", Description = "Возвращает всю таблицу Пользователей + Пользовательская статистика")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetUserInfoAllWithoutHash(
        )
    {
        List<UserInfoDTO> users = Mapper.List_UserInfoToDTO(DB.GetAll<User>(), DB.GetAll<UserStatistic>());

        if (users == null || users.Count == 0)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(users);
    }


    [HttpGet]
    [SwaggerOperation(Summary = "Получение информации о пользователе", Description = "Возвращает информацию о пользователе + его статистика")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetUserInfoWithoutHash(
        [FromQuery][SwaggerParameter("ID пользователя")] string userId)
    {
        if (int.TryParse(userId, out int id) == false)
            return new ObjectResult("В параметре должно быть число") { StatusCode = (int)ServerStatus.NotCorrect };

        UserInfoDTO users = Mapper.UserInfoToDTO(DB.Get<User>(id), DB.Get<UserStatistic>(id));

        if (users == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(users);
    }

    [HttpGet("byName")]
    [SwaggerOperation(Summary = "Получение информации о пользователе", Description = "Возвращает информацию о пользователе + его статистика")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetUserInfoWithoutHashByName(
        [FromQuery][SwaggerParameter("Имя пользователя")] string userName)
    {

        if (userName == "")
            return new ObjectResult("В параметре должна быть строка") { StatusCode = (int)ServerStatus.NotCorrect };

        //!
        User user = DB.GetUser(userName);

        if (user == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        UserInfoDTO userInfo = Mapper.UserInfoToDTO(user, DB.GetUserStatistic(user.Id));

        if (userInfo == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(userInfo);
    }



    [HttpDelete]
    [SwaggerOperation(Summary = "Удалить пользователя", Description = "Удаляет пользователя из таблиц Пользователя и Пользовательская статистика, и возвращает его копию")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    [SwaggerResponse((int)ServerStatus.UnKnown, "Не известная ошибка")]
    public IActionResult DeleteUser(
        [FromQuery][SwaggerParameter("Id пользователя")] string userId) 
    {
        if (int.TryParse(userId, out int id) == false)
            return new ObjectResult("В параметре должно быть число") { StatusCode = (int)ServerStatus.NotCorrect };

        User user = DB.Get<User>(id);
        UserStatistic userStatistic = DB.GetUserStatistic(id);

        if (user == null || userStatistic == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        UserInfoDTO userInfo = Mapper.UserInfoToDTO(user, userStatistic);

        //!
        if (DB.Delete<User>(id) && DB.Delete<UserStatistic>((DB.GetUserStatistic(id)).Id))
            return new JsonResult(userInfo);
        else
            return new ObjectResult("Произошла ошибка") { StatusCode = (int)ServerStatus.UnKnown };
    }


    [HttpPost]
    [SwaggerOperation(Summary = "Добавить пользователя", Description = "Добавляет нового пользователя в таблицу Пользователя и Пользовательская статистика, и возвращает его копию")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.UnKnown, "Не известная ошибка")]
    [SwaggerResponse((int)ServerStatus.AlreadyExist, "Данная запись уже существует в таблице")]
    public IActionResult AddUser(
        [FromQuery][SwaggerParameter("Информация о новом пользователе")] UserDTO userDTO)
    {
        User user = Mapper.DTOToEntity(userDTO);

        User old_user = DB.GetUser(user.Name);

        if (old_user != null)
            return new ObjectResult("Пользователь с таким логином уже существует") { StatusCode = (int)ServerStatus.AlreadyExist };

        if (DB.AddUser(user) == false)
            return new ObjectResult("Произошла ошибка при создании пользователя") { StatusCode = (int)ServerStatus.UnKnown };

        User new_user = DB.GetUser(user.Name);

        if (new_user == null)
            return new ObjectResult("Произошла ошибка при создании пользователя") { StatusCode = (int)ServerStatus.UnKnown };

        UserStatistic userStatistic = new UserStatistic
        {
            UserId = new_user.Id,
            Draw = 0,
            Lose = 0,
            Win = 0,
            MaxScore = 0,
            Rate = 30
        };

        if (DB.AddUserStatistic(userStatistic) == false)
            return new ObjectResult("Произошла ошибка при создании статистики пользователя") { StatusCode = (int)ServerStatus.UnKnown };

        UserInfoDTO userInfoDTO = Mapper.UserInfoToDTO(DB.Get<User>(new_user.Id), DB.GetUserStatistic(new_user.Id));

        if (userInfoDTO == null)
            return new ObjectResult("Не удалось найти данные") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(userInfoDTO);
    }


    [HttpPut]
    [SwaggerOperation(Summary = "Обновить пользователя", Description = "Обновляет пользователя в таблицах Пользователя и Пользовательская статистика. В случае, если не нужно изменять поле, нужно указать у него значение -1 в DTO.")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.UnKnown, "Не известная ошибка")]
    [SwaggerResponse((int)ServerStatus.NotExist, "Запись не найдена")]
    public IActionResult UpdateUser(
        [FromQuery][SwaggerParameter("Информация о пользователе")] UserInfoDTO userInfoDTO,
        [FromQuery][SwaggerParameter("ID пользователя")] int user_id)
    {
        User old_user = DB.Get<User>(user_id);

        if (old_user == null)
            return new ObjectResult("Пользователя с таким идентификатор не существует") { StatusCode = (int)ServerStatus.NotExist };

        User update_user = new User { Id = user_id };

        if (userInfoDTO.Password != "-1") update_user.Password = userInfoDTO.Password;

        if (userInfoDTO.Name != "-1") update_user.Name = userInfoDTO.Name;

        if (DB.Update(update_user) == false)
            return new ObjectResult("Произошла ошибка при обновлении данных пользователя") { StatusCode = (int)ServerStatus.UnKnown };


        UserStatistic old_user_statistic = DB.GetUserStatistic(update_user.Id);

        if (old_user_statistic == null)
            return new ObjectResult("Пользователя с таким идентификатор не существует") { StatusCode = (int)ServerStatus.NotExist };

        UserStatistic update_user_statistic = new UserStatistic { Id = user_id, UserId = update_user.Id };
        
        if (userInfoDTO.Draw != -1) update_user_statistic.Draw = userInfoDTO.Draw;

        if (userInfoDTO.Win != -1) update_user_statistic.Win = userInfoDTO.Win;
        
        if (userInfoDTO.Lose != -1) update_user_statistic.Lose = userInfoDTO.Lose;
        
        if (userInfoDTO.Rate != -1)  update_user_statistic.Rate = userInfoDTO.Rate;

        if (userInfoDTO.MaxScore != -1) update_user_statistic.MaxScore = userInfoDTO.MaxScore;

        if (DB.Update(update_user_statistic) == false)
            return new ObjectResult("Произошла ошибка при обновлении данных о статистике пользователя") { StatusCode = (int)ServerStatus.UnKnown };

        return new JsonResult(userInfoDTO);
    }
}