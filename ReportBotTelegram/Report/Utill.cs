using System.Net.Http.Headers;

public static class Utill 
{
    public static List<DateTime> GetDateTime(string date)
    {
        List<DateTime> result = new List<DateTime>();

        try
        {
            string[] dateStrings = date.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string dateString in dateStrings)
            {
                if (DateTime.TryParse(dateString, out DateTime parsedDate))
                    result.Add(parsedDate);
                else
                    return new List<DateTime>();
            }

            if (result.Count == 1)
                result.Add(DateTime.Now);
        }
        catch (Exception)
        {
            return new List<DateTime>();
        }

        return result;
    }


    public static string CalculateGameTimes(WrapGetEvent startGameEvents, WrapGetEvent endGameEvents)
    {
        var gameDurationsByMode = new Dictionary<string, List<TimeSpan>>();

        foreach (var startEvent in startGameEvents.logEventDTOs)
        {
            if (startEvent.Description.StartsWith("start game:"))
            {
                List<int> userIds = startEvent.UsersId;
                DateTime startTime = startEvent.Date;

                // Извлекаем режим из описания
                string mode = startEvent.Description.Substring("start game:".Length);

                // Найдем соответствующее событие EndGame
                var endEvent = endGameEvents.logEventDTOs
                    .FirstOrDefault(e => e.Description == "end game" && e.UsersId.SequenceEqual(userIds) && e.Date > startTime);

                if (endEvent != null)
                {
                    // Проверим, что нет промежуточных StartGame событий
                    bool hasIntermediateStartGame = startGameEvents.logEventDTOs
                        .Any(se => se.Description.StartsWith("start game:") &&
                                   se.UsersId == userIds &&
                                   se.Date > startTime &&
                                   se.Date < endEvent.Date);

                    if (!hasIntermediateStartGame)
                    {
                        DateTime endTime = endEvent.Date;
                        TimeSpan duration = endTime - startTime;

                        // Добавляем продолжительность в соответствующий режим
                        if (!gameDurationsByMode.ContainsKey(mode))
                            gameDurationsByMode[mode] = new List<TimeSpan>();

                        gameDurationsByMode[mode].Add(duration);
                    }
                }
            }
        }

        string answer = "";

        if (gameDurationsByMode.Any())
        {
            foreach (var mode in gameDurationsByMode.Keys)
            {
                List<TimeSpan> modeDurations = gameDurationsByMode[mode];

                TimeSpan maxDuration = modeDurations.Max();
                TimeSpan minDuration = modeDurations.Min();
                TimeSpan avgDuration = TimeSpan.FromTicks((long)modeDurations.Average(duration => duration.Ticks));

                answer += $"Режим: {mode}\n";
                answer += $"Максимальное время в игре: {maxDuration.TotalMinutes:F0} минут {maxDuration.Seconds} секунд\n";
                answer += $"Минимальное время в игре: {minDuration.TotalMinutes:F0} минут {minDuration.Seconds} секунд\n";
                answer += $"Среднее время в игре: {avgDuration.TotalMinutes:F0} минут {avgDuration.Seconds} секунд\n\n";
            }
        }
        else
            return "Не нашлись данные!";

        return answer;
    }

}