using GameServer.Database;
using GameServer.Database.DTO;
using GameServer.Enum;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Mapper;

[ApiController]
[Route("api/[controller]")]
public class TableEventController : ControllerBase
{
    [HttpGet("period")]
    [SwaggerOperation(
        Summary = "Получение списка событий", 
        Description = "Возвращает таблицы Event, в определенном промежутке времени. " +
        "Пример формы передачи периода: 2023-11-29 12:30:00"
    )]
    [SwaggerResponse((int)APIResponseStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)APIResponseStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)APIResponseStatus.NotLogic, "Не логичные данные")]
    [SwaggerResponse((int)APIResponseStatus.NotFound, "Не нашлись данные")]
    public async Task<IActionResult> GetEventsByTimePeriod(
        [FromQuery][SwaggerParameter("Начальный период")] string startPeriod,
        [FromQuery][SwaggerParameter("Конечный период")] string endPeriod)
    {
        bool isCorrectDate = TryParseDateTime(startPeriod, endPeriod, out DateTime start, out DateTime end, out JsonResult errorDate);

        if (isCorrectDate == false)
            return errorDate;

        var events = await DB.GetEventsAsync(start, end);
        return CreateLogEventsResult(events);
    }


    [HttpGet("period_types")]
    [SwaggerOperation(
        Summary = "Получение списка событий по определенным типам события", 
        Description = "Возвращает таблицы Event с определенным типом события, в определенном промежутке времени. " +
        "Типы событий: " +
        "Registration - регистрация пользователя, " +
        "Authorization - авторизация пользователя, " +
        "SearchGame - запущен поиск игры, " +
        "StartGame - игра началась, " +
        "EndGame - игра завершилась, " +
        "SurrenderGame - один из пользователей сдался" + 
        "Пример формы передачи периода: 2023-11-29 12:30:00. " +
        "Пример передачи типов эвента: Registration, Authorization"
    )]
    [SwaggerResponse((int)APIResponseStatus.Success, "Успешный запрос")]
    [SwaggerResponse((int)APIResponseStatus.NotCorrect, "Не корректные данные")]
    [SwaggerResponse((int)APIResponseStatus.NotLogic, "Не логичные данные")]
    [SwaggerResponse((int)APIResponseStatus.NotFound, "Не нашлись данные")]
    public async Task<IActionResult> GetEventsByTimePeriodWithParams(
    [FromQuery][SwaggerParameter("Начальный период.")] string startPeriod,
    [FromQuery][SwaggerParameter("Конечный период.")] string endPeriod,
    [FromQuery][SwaggerParameter("Типы эвента, перечисленные через запятую.")] string stringListTypeEvent)
    {
        bool isCorrectDate = TryParseDateTime(startPeriod, endPeriod, out DateTime start, out DateTime end, out JsonResult errorDate);

        if (isCorrectDate == false)
            return errorDate;

        bool isCorrectLogEvents = TryParseLogEvents(stringListTypeEvent, out List<LogEventType> logEvents, out JsonResult errorLogEvents);
        
        if (isCorrectLogEvents == false)
            return errorLogEvents;

        var events = await DB.GetEventsWithTypesAsync(start, end, logEvents);
        return CreateLogEventsResult(events);
    }


    [HttpGet("all")]
    [SwaggerOperation(Summary = "Получение всего списка событий", Description = "Возвращает всю таблицу Event")]
    [SwaggerResponse(200, "Успешный запрос")]
    [SwaggerResponse((int)APIResponseStatus.NotFound, "Не нашлись данные")]
    public async Task<IActionResult> GetEventsAll()
    {
        var events = await DB.GetAllAsync<LogEvent>();
        return CreateLogEventsResult(events);
    }


    private static bool TryParseLogEvents(string stringListTypeEvent, out List<LogEventType> logEvents, out JsonResult errorLogEvents)
    {
        logEvents = new();
        errorLogEvents = new JsonResult("Error") { StatusCode = (int)APIResponseStatus.UnKnown };

        if (string.IsNullOrWhiteSpace(stringListTypeEvent)) 
        {
            errorLogEvents = new JsonResult("В параметры пришла пустая строка") { StatusCode = (int)APIResponseStatus.NotCorrect };
            return false;
        }

        string[] eventNames = stringListTypeEvent.Split(new[] { ", " }, StringSplitOptions.None);

        logEvents = eventNames.
            Select(eventName => Enum.TryParse(eventName, out LogEventType logEvent) ? logEvent : (LogEventType?)null).
            Where(logEvent => logEvent.HasValue).Select(logEvent => logEvent.Value).ToList();

        if (logEvents == null || logEvents.Count == 0) 
        {
            errorLogEvents = new JsonResult("Не верные параметры") { StatusCode = (int)APIResponseStatus.NotCorrect };
            return false;
        }

        return true;
    }

    private bool TryParseDateTime(string startPeriod, string endPeriod, out DateTime start, out DateTime end,out JsonResult errorDate) 
    {
        errorDate = new JsonResult("Error");
        end = default;

        if (DateTime.TryParse(startPeriod, out start) == false ||
            DateTime.TryParse(endPeriod, out end) == false) 
        {
            errorDate =  new JsonResult("Дата не в правильном формате") { StatusCode = (int)APIResponseStatus.NotCorrect };
            return false;
        }

        if (start >= end) 
        {
            errorDate = new JsonResult("Начальная период не может быть позже конечного") { StatusCode = (int)APIResponseStatus.NotLogic };
            return false;
        }

        return true;
    }

    private JsonResult CreateLogEventsResult(List<LogEvent> events)
    {
        if (events == null || events.Count == 0)
            return new JsonResult("Не удалось найти данные в данном промежутке") { StatusCode = (int)APIResponseStatus.NotFound };
        else
            return new JsonResult(events);
    }
}