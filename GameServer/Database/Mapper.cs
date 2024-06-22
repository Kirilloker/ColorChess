using GameServer.Database.DTO;

public static class Mapper
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

    public static User DTOToEntity(UserDTO userDTO) 
    {
        return new User
        {
            Name = userDTO.Name,
            Password = userDTO.Password
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

    public static List<UserInfoDTO> ListUserInfoToDTO(List<User> users, List<UserStatistic> userStatistics)
    {
        UserInfoDTO ConvertUserToDto(User user)
        {
            var userStatistic = userStatistics.FirstOrDefault(stat => stat.Id == user.Id);
            return userStatistic != null ? UserInfoToDTO(user, userStatistic) : throw new Exception("Not found User statistic in mapper");
        }

        var tempList = ListEntityToDTO(users, new ConvertEntityToDto<User, UserInfoDTO>(ConvertUserToDto));

        return tempList;
    }


    public delegate TDTO ConvertEntityToDto<T, TDTO>(T entity);
    public static List<TDTO> ListEntityToDTO<T, TDTO>(List<T> resource, ConvertEntityToDto<T, TDTO> convertFunc) where T : new()
    {
        List<TDTO> resourceDTO = new List<TDTO>(resource.Count);

        foreach (var item in resource)
        {
            var dto = convertFunc(item);

            if (dto != null)
                resourceDTO.Add(dto);
        }

        return resourceDTO;
    }
}
