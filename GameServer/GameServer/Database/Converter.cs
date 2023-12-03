using GameServer.Database.DTO;

public static class Converter
{
    public static LogEventDTO EntityToDTO(LogEvent resource)
    {
        return new LogEventDTO
        {
            Date = resource.Date,
            Type_Event = resource.Type_Event,
            UsersId = resource.UsersId,
            Description = resource.Description
        };
    }
    public static UserInfoHashDTO UserInfoHashToDTO(User user, UserStatistic userStatistic)
    {
        return new UserInfoHashDTO
        {
            Name = user.Name,
            PasswordHash = EncodeStringToBase64(user.Password),
            Win = userStatistic.Win,
            Lose = userStatistic.Lose,
            MaxScore = userStatistic.MaxScore,
            Draw = userStatistic.Draw,
            Rate = userStatistic.Rate
        };
    }

    public static UserInfoDTO UserInfoToDTO(User user, UserStatistic userStatistic)
    {
        return new UserInfoDTO
        {
            Name = user.Name,
            Password = user.Password,
            Win = userStatistic.Win,
            Lose = userStatistic.Lose,
            MaxScore = userStatistic.MaxScore,
            Draw = userStatistic.Draw,
            Rate = userStatistic.Rate
        };
    }

    public static List<UserInfoHashDTO> List_UserInfoHashToDTO(List<User> users, List<UserStatistic> userStatistics) 
    {
        List<UserInfoHashDTO> resourceDTO = new List<UserInfoHashDTO>(users.Count);

        foreach (var user in users)
        {
            var userStatistic = userStatistics.FirstOrDefault(stat => stat.Id == user.Id);

            if (userStatistic != null)
                resourceDTO.Add(UserInfoHashToDTO(user, userStatistic));
        }

        return resourceDTO;
    }


    public static List<UserInfoDTO> List_UserInfoToDTO(List<User> users, List<UserStatistic> userStatistics)
    {
        List<UserInfoDTO> resourceDTO = new List<UserInfoDTO>(users.Count);

        foreach (var user in users)
        {
            var userStatistic = userStatistics.FirstOrDefault(stat => stat.Id == user.Id);

            if (userStatistic != null)
                resourceDTO.Add(UserInfoToDTO(user, userStatistic));
        }

        return resourceDTO;
    }





    public delegate TDto ConvertEntityToDto<T, TDto>(T entity);
    public static List<TDto> List_EntityToDTO<T, TDto>(List<T> resource, ConvertEntityToDto<T, TDto> convertFunc) where T : new()
    {
        List<TDto> resourceDTO = new List<TDto>(resource.Count);
        foreach (var item in resource)
            resourceDTO.Add(convertFunc(item));
        return resourceDTO;
    }
    public static string EncodeStringToBase64(string text)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(bytes);
    }
}
