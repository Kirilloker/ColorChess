using GameServer.Database.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class TableEventController : ControllerBase
{
    [HttpGet("period")]
    [SwaggerOperation(Summary = "Получение списка событий", Description = "Возвращает таблицы Event, в определенном промежутке времени")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)ServerStatus.NotLogic, "Не логичные данные")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetEventsByTimePeriod(
        [FromQuery][SwaggerParameter("Начальный период. Пример: 2023-11-29 12:30:00")] string startPeriod,
        [FromQuery][SwaggerParameter("Конечный период. Пример: 2023-12-29 12:30:00")] string endPeriod)
    {
        if (DateTime.TryParse(startPeriod, out DateTime start) == false ||
            DateTime.TryParse(endPeriod, out DateTime end) == false)
            return new JsonResult("Дата не в правильном формате") { StatusCode = (int)ServerStatus.NotCorrect };

        if (start >= end)
            return new JsonResult("Начальная период не может быть позже конечного") { StatusCode = (int)ServerStatus.NotLogic };

        List<LogEventDTO> events = Mapper.List_EntityToDTO(DB.GetEvents(start, end), Mapper.EntityToDTO);

        if (events == null || events.Count == 0)
            return new JsonResult("Не удалось найти данные в данном промежутке") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(events);
    }


    [HttpGet("period_types")]
    [SwaggerOperation(Summary = "Получение списка событий по определенным типам события", Description = "Возвращает таблицы Event с определенным типом события, в определенном промежутке времени. Типы событий: Registration - регистрация пользователя, Authorization - авторизация пользователя, SearchGame - запущен поиск игры, StartGame - игра началась, EndGame - игра завершилась, SurrenderGame - один из пользователей сдался")]
    [SwaggerResponse((int)ServerStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)ServerStatus.NotLogic, "Не логичные данные")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetEventsByTimePeriodWithParams(
    [FromQuery][SwaggerParameter("Начальный период. Пример: 2023-11-29 12:30:00")] string startPeriod,
    [FromQuery][SwaggerParameter("Конечный период. Пример: 2023-12-29 12:30:00")] string endPeriod,
    [FromQuery][SwaggerParameter("Типы эвента, перечисленные через запятую. Пример: Registration, Authorization")] string listTypeEvent)
    {
        if (DateTime.TryParse(startPeriod, out DateTime start) == false ||
            DateTime.TryParse(endPeriod, out DateTime end) == false)
            return new JsonResult("Дата не в правильном формате") { StatusCode = (int)ServerStatus.NotCorrect };

        if (start >= end)
            return new JsonResult("Начальная период не может быть позже конечного") { StatusCode = (int)ServerStatus.NotLogic };


        if (string.IsNullOrWhiteSpace(listTypeEvent))
            return new JsonResult("В параметры пришла пустая строка") { StatusCode = (int)ServerStatus.NotCorrect };

        string[] eventNames = listTypeEvent.Split(new[] { ", " }, StringSplitOptions.None);

        List<TypeLogEvent> logEvents = eventNames.
            Select(eventName => Enum.TryParse(eventName, out TypeLogEvent logEvent) ? logEvent : (TypeLogEvent?)null).
            Where(logEvent => logEvent.HasValue).Select(logEvent => logEvent.Value).ToList();

        if (logEvents == null || logEvents.Count == 0)
            return new JsonResult("Не верные параметры") { StatusCode = (int)ServerStatus.NotCorrect };

        List<LogEventDTO> events = Mapper.List_EntityToDTO(DB.GetEventsWithTypes(start, end, logEvents), Mapper.EntityToDTO);

        if (events == null || events.Count == 0)
            return new JsonResult("Не удалось найти данные в данном промежутке") { StatusCode = (int) HttpStatusCode.NotFound };

        return new JsonResult(events) { StatusCode = (int)HttpStatusCode.OK };
    }


    [HttpGet("all")]
    [SwaggerOperation(Summary = "Получение всего списка событий", Description = "Возвращает всю таблицу Event")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse((int)ServerStatus.NotFound, "Не нашлись данные")]
    public IActionResult GetEventsAll()
    {
        List<LogEventDTO> events = Mapper.List_EntityToDTO(DB.GetAll<LogEvent>(), Mapper.EntityToDTO);

        if (events == null || events.Count == 0)
            return new JsonResult("Не удалось найти данные в данном промежутке") { StatusCode = (int)ServerStatus.NotFound };

        return new JsonResult(events);
    }

    public static List<TypeLogEvent> ParseLogEvents(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<TypeLogEvent>();

        string[] eventNames = input.Split(new[] { ", " }, StringSplitOptions.None);

        List<TypeLogEvent> logEvents = eventNames.
            Select(eventName => Enum.TryParse(eventName, out TypeLogEvent logEvent) ? logEvent : (TypeLogEvent?)null).
            Where(logEvent => logEvent.HasValue).Select(logEvent => logEvent.Value) .ToList();

        return logEvents;
    }
}