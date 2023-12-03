using GameServer.Database.DTO;
using GameServer.Migrations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


[ApiController]
[Route("api/[controller]")]
public class TableEventController : ControllerBase
{
    [HttpGet("period")]
    [SwaggerOperation(Summary = "Получение списка событий", Description = "Возвращает таблицы Event, в определенном промежутке времени")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse(400, "Не корректные данные")]
    [SwaggerResponse(401, "Не логичные данные")]
    [SwaggerResponse(402, "Не нашлись данные")]
    public IActionResult GetEventsByTimePeriod(
        [FromQuery][SwaggerParameter("Начальный период. Пример: 2023-11-29 12:30:00")] string startPeriod,
        [FromQuery][SwaggerParameter("Конечный период. Пример: 2023-12-29 12:30:00")] string endPeriod)
    {
        if (DateTime.TryParse(startPeriod, out DateTime start) == false ||
            DateTime.TryParse(endPeriod, out DateTime end) == false)
            return new ObjectResult("Дата не в правильном формате") { StatusCode = 400 };

        if (start >= end)
            return new ObjectResult("Начальная период не может быть позже конечного") { StatusCode = 401 };

        List<LogEventDTO> events = Mapper.List_EntityToDTO(DB.GetEvents(start, end), Mapper.EntityToDTO);

        if (events == null || events.Count == 0)
            return new ObjectResult("Не удалось найти данные в данном промежутке") { StatusCode = 402 };

        return new JsonResult(events);
    }


    [HttpGet("period_types")]
    [SwaggerOperation(Summary = "Получение списка событий по определенным типам события", Description = "Возвращает таблицы Event с определенным типом события, в определенном промежутке времени. Типы событий: Registration - регистрация пользователя, Authorization - авторизация пользователя, SearchGame - запущен поиск игры, StartGame - игра началась, EndGame - игра завершилась, SurrenderGame - один из пользователей сдался")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse(400, "Не корректные данные")]
    [SwaggerResponse(401, "Не логичные данные")]
    [SwaggerResponse(402, "Не нашлись данные")]
    public IActionResult GetEventsByTimePeriodWithParams(
    [FromQuery][SwaggerParameter("Начальный период. Пример: 2023-11-29 12:30:00")] string startPeriod,
    [FromQuery][SwaggerParameter("Конечный период. Пример: 2023-12-29 12:30:00")] string endPeriod,
    [FromQuery][SwaggerParameter("Типы эвента, перечисленные через запятую. Пример: Registration, Authorization")] string listTypeEvent)
    {
        if (DateTime.TryParse(startPeriod, out DateTime start) == false ||
            DateTime.TryParse(endPeriod, out DateTime end) == false)
            return new ObjectResult("Дата не в правильном формате") { StatusCode = 400 };

        if (start >= end)
            return new ObjectResult("Начальная период не может быть позже конечного") { StatusCode = 401 };


        if (string.IsNullOrWhiteSpace(listTypeEvent))
            return new ObjectResult("В параметры пришла пустая строка") { StatusCode = 400 };

        string[] eventNames = listTypeEvent.Split(new[] { ", " }, StringSplitOptions.None);

        List<TypeLogEvent> logEvents = eventNames.
            Select(eventName => Enum.TryParse(eventName, out TypeLogEvent logEvent) ? logEvent : (TypeLogEvent?)null).
            Where(logEvent => logEvent.HasValue).Select(logEvent => logEvent.Value).ToList();

        if (logEvents == null || logEvents.Count == 0)
            return new ObjectResult("Не верные параметры") { StatusCode = 400 };

        List<LogEventDTO> events = Mapper.List_EntityToDTO(DB.GetEventsWithTypes(start, end, logEvents), Mapper.EntityToDTO);

        if (events == null || events.Count == 0)
            return new ObjectResult("Не удалось найти данные в данном промежутке") { StatusCode = 402 };

        return new JsonResult(events);
    }


    [HttpGet("all")]
    [SwaggerOperation(Summary = "Получение всего списка событий", Description = "Возвращает всю таблицу Event")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse(402, "Не нашлись данные")]
    public IActionResult GetEventsAll()
    {
        List<LogEventDTO> events = Mapper.List_EntityToDTO(DB.GetAll<LogEvent>(), Mapper.EntityToDTO);

        if (events == null || events.Count == 0)
            return new ObjectResult("Не удалось найти данные в данном промежутке") { StatusCode = 402 };

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