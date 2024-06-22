﻿using FirstEF6App;
using GameServer.Enum;
using GameServer.Tools;
using Microsoft.EntityFrameworkCore;

public static class DB 
{
    #region Get




    public static T Get<T>(int id) where T : class, IId
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string tableName = typeof(T).Name + "s"; 
                string sqlQuery = $"SELECT * FROM {tableName} WHERE Id = {id};";
                return db.ExecuteSqlQuery<T>(sqlQuery).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }



    /// <summary>
    /// Возвращает имя пользователя
    /// </summary>>
    public static string GetNameUser(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.users.Where(b => b.Id == userID).ToList()[0].Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetNameUser: " + ErrorMessage.NotFound);
                return ErrorMessage.NotFound;
            }
        }
    }

    public static async Task<string> GetNameUserAsync(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                User user = await db.users.FirstOrDefaultAsync(b => b.Id == userID);
                if (user != null)
                {
                    return user.Name;
                }
                else
                {
                    Console.WriteLine("User not found");
                    return ErrorMessage.NotFound;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetNameUser: " + ErrorMessage.NotFound);
                return ErrorMessage.NotFound;
            }
        }
    }


    /// <summary>
    /// Возвращает рейтинг пользователя
    /// </summary>>
    public static int GetRateUser(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var user = db.userstatistics.Where(b => b.UserId == userID).ToList();

                if (user.Count == 0)
                {
                    Console.WriteLine("Error GetNameUser: " + ErrorMessage.NotFound);
                    return -1;
                }

                return user[0].Rate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetNameUser: " + ErrorMessage.NotFound);
                return -1;
            }
        }
    }


    public static async Task<int> GetRateUserAsync(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var user = await db.userstatistics.Where(b => b.UserId == userID).ToListAsync();

                if (user.Count == 0)
                {
                    Console.WriteLine("Error GetNameUser: " + ErrorMessage.NotFound);
                    return -1;
                }

                return user[0].Rate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetNameUser: " + ErrorMessage.NotFound);
                return -1;
            }
        }
    }

    /// <summary>
    /// Возвращает рейтинг пользователя
    /// </summary>>
    public static int GetRateUser(string userName)
    {
        return GetRateUser(GetUser(userName).Id);
    }

    public static async Task<int> GetRateUserAsync(string userName)
    {
        return await GetRateUserAsync(GetUser(userName).Id);
    }

    /// <summary>
    /// Возвращает пароль Пользователя
    /// </summary>>
    public static string GetPasswordUser(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.users.Where(b => b.Id == userID).ToList()[0].Password;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetPasswordUser: " + ErrorMessage.NotFound);
                return ErrorMessage.NotFound;
            }
        }
    }

    /// <summary>
    /// Возвращает пароль Пользователя
    /// </summary>>
    public static string GetPasswordUser(User user) 
    {
        return GetPasswordUser(user.Id);
    }

    /// <summary>
    /// Возвращает пароль Пользователя
    /// </summary>>
    public static string GetPasswordUser(string name)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.users.Where(b => b.Name == name).ToList()[0].Password;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetPasswordUser: " + ErrorMessage.NotFound);
                return ErrorMessage.NotFound;
            }
        }
    }

    /// <summary>
    /// Возврашает статистику пользователя
    /// </summary>


    /// <summary>
    /// Возврашает статистику пользователя
    /// </summary>
    public static UserStatistic GetUserStatistic(User user)
    {
        return GetUserStatistic(user.Id);
    }

    /// <summary>
    /// Возврашает все статистики игры пользователя
    /// </summary>
    static List<GameStatistic>? GetGameStatisticUser(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.gamestatistics.Where(b => b.UsersId.Contains(userId)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetGameStatisticUser: " + ErrorMessage.NotFound);
                return null;
            }
        }
    }

    /// <summary>
    /// Возврашает все статистики игры пользователя
    /// </summary>
    static List<GameStatistic> GetGameStatisticUser(User user)
    {
        return GetGameStatisticUser(user.Id);
    }

    /// <summary>
    /// Возвращает Список пары - имя игрока - количество очков. CountTop - количество элементов в массиве
    /// </summary>
    public static async Task<List<KeyValuePair<string, int>>> GetListTopRate(int countTop = 5) 
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<KeyValuePair<string, int>> top = new();
                
                List<UserStatistic> userStatistics = await db.userstatistics.OrderByDescending(u => u.Rate).Take(5).ToListAsync();

                foreach (UserStatistic userStat in userStatistics)
                {
                    string userName = await GetNameUserAsync(userStat.UserId);
                    top.Add(new KeyValuePair<string, int>(userName, userStat.Rate));
                }

                return top;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetListTopRate");
                return null; 
            }
        }
    }

    public static async Task<List<KeyValuePair<string, int>>> GetListTopRateAsync(int countTop = 5)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<KeyValuePair<string, int>> top = new();

                List<UserStatistic> userStatistics = await db.userstatistics.OrderByDescending(u => u.Rate).Take(5).ToListAsync();

                foreach (UserStatistic userStat in userStatistics)
                {
                    string userName = GetNameUser(userStat.UserId);
                    top.Add(new KeyValuePair<string, int>(userName, userStat.Rate));
                }

                return top;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetListTopRate");
                return null;
            }
        }
    }

    /// <summary>
    /// Возвращает место игрока в топе по рейтенгу
    /// </summary>
    public static int GetNumberPlaceUserByRate(string userName)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var userRank = db.userstatistics.OrderByDescending(u => u.Rate)
                .AsEnumerable()
                .Select((userStat, index) => new { UserName = GetNameUser(userStat.UserId), Rank = index + 1 })
                .FirstOrDefault(u => u.UserName == userName);


                if (userRank != null)
                {
                    return userRank.Rank;
                }

                // Если имя пользователя не найдено в списке
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetUserRankByRate: " + ErrorMessage.NotFound);
                return -1;
            }
        }
    }


    public static async Task<int> GetNumberPlaceUserByRateAsync(string userName)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var userRank = await db.userstatistics.OrderByDescending(u => u.Rate)
                    .ToListAsync(); 

                var user = userRank
                    .Select((userStat, index) => new { UserName = GetNameUser(userStat.UserId), Rank = index + 1 })
                    .FirstOrDefault(u => u.UserName == userName);

                if (user != null)
                    return user.Rank;

                // Если имя пользователя не найдено в списке
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetUserRankByRate: " + ErrorMessage.NotFound);
                return -1;
            }
        }
    }



    #endregion

    #region Get All DB

    public static List<T> GetAll<T>() where T : class, new()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string sqlQuery = $"SELECT * FROM {typeof(T).Name}s;";
                return db.ExecuteSqlQuery<T>(sqlQuery);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<T>();
            }
        }
    }

    public static async Task<List<T>> GetAllAsync<T>() where T : class, new()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string sqlQuery = $"SELECT * FROM {typeof(T).Name}s;";
                return await db.ExecuteSqlQueryAsync<T>(sqlQuery);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<T>();
            }
        }
    }


    #endregion

    #region Add

    public static bool AddUser(User user)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string sqlQuery = $"INSERT INTO users (Name, Password) VALUES ('{user.Name}', '{user.Password}');";
                db.ExecuteSqlQuery<User>(sqlQuery);
                return true;
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }


    public static bool AddUserStatistic(UserStatistic userStatistic)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string sqlQuery = $"INSERT INTO UserStatistics (Win, Lose, MaxScore, Draw, Rate, UserId) VALUES " +
                              $"({userStatistic.Win}, {userStatistic.Lose}, {userStatistic.MaxScore}, " +
                              $"{userStatistic.Draw}, {userStatistic.Rate}, {userStatistic.UserId});";

                db.ExecuteSqlQuery<User>(sqlQuery);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }






        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        public static bool AddUser(string username, string password)
        {
            using (ColorChessContext db = new ColorChessContext())
            {
                try
                {
                    List<User> users =  db.users.Where(b => b.Name == username).ToList();

                    if (users.Count == 0) 
                    {
                        User user = new User { Name = username, Password = password };

                        db.users.Add(user);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error AddUser: " + ErrorMessage.UserExist);
                        return false;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Error AddUser: " + ErrorMessage.Unknown);
                    return false;
                }
            }
        }

    /// <summary>
    /// Добавление нового пользователя
    /// </summary>
    //public static void AddUser(User user)
    //{
    //    AddUser(user.Name, user.Password);
    //}

    /// <summary>
    /// Добавление игровой статистики пользователя
    /// </summary>
    public static void AddUserStatistic(int win, int lose, int draw, int maxScore, int rate, int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<UserStatistic> userStatistics = db.userstatistics.Where(b => b.UserId == userId).ToList();

                if (userStatistics.Count == 0)
                {
                    UserStatistic statistic = new UserStatistic
                    {
                        Win = win,
                        Lose = lose,
                        Draw = draw,
                        MaxScore = maxScore,
                        Rate = rate,
                        UserId = userId
                    };

                    db.userstatistics.Add(statistic);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error AddUserStatistic: " + ErrorMessage.UserStatisticExist);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddUserStatistic: " + ErrorMessage.Unknown);
            }
        }
    }


    /// <summary>
    /// Добавление игровой статистики пользователя
    /// </summary>
    //public static void AddUserStatistic(UserStatistic userStatistic)
    //{
    //    AddUserStatistic(userStatistic.Win, userStatistic.Lose,
    //                     userStatistic.Draw, userStatistic.MaxScore,
    //                     userStatistic.Rate, userStatistic.UserId);
    //}

   


    /// <summary>
    ///  Добавление информации за игру
    /// </summary>
    public static void AddGameStatistic(List<int> usersScore, GameModeType gameMode, List<int> usersId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                GameStatistic statistic = new GameStatistic
                {
                    PlayerScore = usersScore,
                    GameMode = gameMode,
                    UsersId = usersId
                };

                db.gamestatistics.Add(statistic);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddGameStatistic: " + ErrorMessage.Unknown);
            }
        }
    }

    /// <summary>
    ///  Добавление логирование 
    /// </summary>
    public static void AddLogEvent(LogEventType _Type_Event, List<int> _usersId, string _Description)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                LogEvent logEvent= new LogEvent
                {
                    Date = DateTime.Now,
                    Type_Event = _Type_Event,
                    UsersId = _usersId,
                    Description = _Description
                };

                db.logevents.Add(logEvent);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddLogEvent: " + ErrorMessage.Unknown);
            }
        }
    }

    /// <summary>
    ///  Добавление логирование 
    /// </summary>
    public static void AddLogEvent(LogEventType _Type_Event, int _userId, string _Description)
    {
        List<int> usersId = new List<int>() { _userId};
        AddLogEvent(_Type_Event, usersId, _Description);
    }

    #endregion

    #region Changes


    public static bool Update<T>(T entity) where T : class, IId
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string updateQuery = $"UPDATE {typeof(T).Name}s SET ";
                var properties = typeof(T).GetProperties();

                foreach (var prop in properties)
                {
                    if (prop.Name == "Id")
                        continue;

                    updateQuery += $"{prop.Name} = '{prop.GetValue(entity)}', ";
                }

                updateQuery = updateQuery.TrimEnd(',', ' ') + $" WHERE Id = {entity.Id};";
                db.ExecuteSqlQuery<T>(updateQuery);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Изменяет имя пользователя
    /// </summary>
    public static void ChangeNameUser(int userId, string newName)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                User user = db.users.Where(b => b.Id == userId).ToList()[0];
                user.Name = newName;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeNameUser: " + ErrorMessage.NotFound);
            }
        }
    }

  

    /// <summary>
    /// Изменяет имя пользователя
    /// </summary>
    public static void ChangeNameUser(string oldName, string newName)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                User user = db.users.Where(b => b.Name == oldName).ToList()[0];
                user.Name = newName;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeNameUser: " + ErrorMessage.NotFound);
            }
        }
    }

    /// <summary>
    /// Изменяет атрибут статистики пользователя на переданное значение  
    /// </summary>
    public static void ChangeUserStatistic(int userId, AttributeUS attribute, int value)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                UserStatistic userStatistic = db.userstatistics.Where(b => b.UserId == userId).ToList()[0];

                switch (attribute)
                {
                    case AttributeUS.Win:
                        userStatistic.Win += value;
                        if ((userStatistic.Win) < 0) userStatistic.Win = 0;
                        break;
                    case AttributeUS.Lose:
                        userStatistic.Lose += value;
                        if ((userStatistic.Lose) < 0) userStatistic.Lose = 0;
                        break;
                    case AttributeUS.MaxScore:
                        userStatistic.MaxScore = value;
                        if ((userStatistic.MaxScore) < 0) userStatistic.MaxScore = 0;
                        break;
                    case AttributeUS.Draw:
                        userStatistic.Draw += value;
                        if ((userStatistic.Draw) < 0) userStatistic.Draw = 0;
                        break;
                    case AttributeUS.Rate:
                        userStatistic.Rate += value;
                        if ((userStatistic.Rate) < 0) userStatistic.Rate = 0;
                        break;
                    default:
                        Console.WriteLine("Error ChangeUserStatistic: " + ErrorMessage.UnknownAttribute);
                        break;
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeUserStatistic: " + ErrorMessage.Unknown);
            }
        }
    }

    /// <summary>
    /// Изменяет атрибут статистики пользователя на переданное значение  
    /// </summary>
    public static void ChangeUserStatistic(User user, AttributeUS attribute, int value)
    {
        ChangeUserStatistic(user.Id, attribute, value); 
    }

    #endregion

    #region Delete

    public static bool Delete<T>(int id) where T : class
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                string sqlQuery = $"DELETE FROM {typeof(T).Name.ToLower()}s WHERE Id = {id};";
                Console.WriteLine(sqlQuery);
                db.ExecuteSqlQuery2(sqlQuery);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }


    #endregion

    #region ClearDB


    /// <summary>
    /// Очишает таблицу Users
    /// </summary>
    public static void ClearUsers()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<User> users = db.users.ToList();

                db.users.RemoveRange(users);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ClearUsers: " + ErrorMessage.Unknown);
            }
        }
    }

    /// <summary>
    /// Очишает таблицу UserStatistics
    /// </summary>
    public static void ClearUserStatistics()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<UserStatistic> userStatisitcs = db.userstatistics.ToList();

                db.userstatistics.RemoveRange(userStatisitcs);
                db.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error ClearUserStatistics: " + ErrorMessage.Unknown);
            }
        }
    }

    /// <summary>
    /// Очишает таблицу GameStatistics
    /// </summary>
    public static void ClearGameStatistics()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<GameStatistic> gameStatisitcs = db.gamestatistics.ToList();

                db.gamestatistics.RemoveRange(gameStatisitcs);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ClearGameStatistics: " + ErrorMessage.Unknown);
            }
        }
    }
    #endregion



    // Report System

    // Возвращает таблицу Event по определенный период  
    public static List<LogEvent> GetEvents(DateTime start, DateTime end)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var parameters = new { StartDate = start.ToString("yyyy-MM-dd HH:mm:ss"), EndDate = end.ToString("yyyy-MM-dd HH:mm:ss") };
                string sqlQuery = $"SELECT * FROM logevents WHERE Date BETWEEN '{parameters.StartDate}' AND '{parameters.EndDate}';";
                return db.ExecuteSqlQuery<LogEvent>(sqlQuery);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<LogEvent>();
            }
        }
    }

    public static async Task<List<LogEvent>> GetEventsAsync(DateTime start, DateTime end)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var parameters = new { StartDate = start.ToString("yyyy-MM-dd HH:mm:ss"), EndDate = end.ToString("yyyy-MM-dd HH:mm:ss") };
                string sqlQuery = $"SELECT * FROM logevents WHERE Date BETWEEN '{parameters.StartDate}' AND '{parameters.EndDate}';";
                return await db.ExecuteSqlQueryAsync<LogEvent>(sqlQuery);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<LogEvent>();
            }
        }
    }

    public static List<LogEvent> GetEventsWithTypes(DateTime start, DateTime end, List<LogEventType> typeEvents)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var parameters = new { StartDate = start.ToString("yyyy-MM-dd HH:mm:ss"), EndDate = end.ToString("yyyy-MM-dd HH:mm:ss") };
                string typeEventsString = string.Join(", ", typeEvents.Select(t => $"'{((int)t).ToString()}'"));
                string sqlQuery = $"SELECT * FROM logevents WHERE Date BETWEEN '{parameters.StartDate}' AND '{parameters.EndDate}' AND Type_Event IN ({typeEventsString});";
                
                return db.ExecuteSqlQuery<LogEvent>(sqlQuery);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<LogEvent>();
            }
        }
    }

    public static async Task<List<LogEvent>> GetEventsWithTypesAsync(DateTime start, DateTime end, List<LogEventType> typeEvents)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                var parameters = new { StartDate = start.ToString("yyyy-MM-dd HH:mm:ss"), EndDate = end.ToString("yyyy-MM-dd HH:mm:ss") };
                string typeEventsString = string.Join(", ", typeEvents.Select(t => $"'{((int)t).ToString()}'"));
                string sqlQuery = $"SELECT * FROM logevents WHERE Date BETWEEN '{parameters.StartDate}' AND '{parameters.EndDate}' AND Type_Event IN ({typeEventsString});";

                return await db.ExecuteSqlQueryAsync<LogEvent>(sqlQuery);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<LogEvent>();
            }
        }
    }


    public static void IDK_how_fix_this_bug(int userId)
    {
        // Когда пользователь начинает поиск, в БД появляются 2 записи о том 
        // Что он авторизовался
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                DateTime threeSecondsAgo = DateTime.Now.AddSeconds(-3);

                var logsToDelete = db.logevents
                    .Where(log => log.Date >= threeSecondsAgo &&
                                  log.Type_Event == LogEventType.Authorization)
                    .ToList();


                if (logsToDelete.Any())
                {
                    db.logevents.RemoveRange(logsToDelete);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }








    public static int? AddEntity<T>(T entity) where T : class
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                db.Set<T>().Add(entity);
                db.SaveChanges();
                return entity.GetEntityId();
            }
            catch
            {
                return null;
            }
        }
    }



    public static User? GetUser(string userName)
    {
        using (var db = new ColorChessContext())
        {
            return db.ExecuteWithHandling(
                ctx => ctx.users.FirstOrDefault(b => b.Name == userName),
                ErrorType.NotFound);
        }
    }


    public static async Task<User?> GetUserAsync(string userName)
    {
        using (var db = new ColorChessContext())
        {
            return await db.users.FirstOrDefaultAsync(b => b.Name == userName);
        }
    }

    public static UserStatistic? GetUserStatistic(string userName)
    {
        using (var db = new ColorChessContext())
        {
            User? user = GetUser(userName);

            if (user == null) 
                return null;

            return GetUserStatistic(user.Id);
        }
    }

    public static UserStatistic? GetUserStatistic(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            return db.ExecuteWithHandling(
                ctx => ctx.userstatistics.FirstOrDefault(b => b.UserId == userId),
                ErrorType.NotFound);
        }
    }

    public static async Task<UserStatistic?> GetUserStatisticAsync(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            return await db.ExecuteWithHandlingAsync(
                async ctx => await ctx.userstatistics.FirstOrDefaultAsync(b => b.UserId == userId),
                ErrorType.NotFound);
        }
    }




}
public static class DbContextExtensions
{
    public static T? ExecuteWithHandling<T>(this ColorChessContext db, Func<ColorChessContext, T> func, ErrorType errorType = ErrorType.UnknownError) 
    {
        try
        {
            var result = func(db);

            if (result == null)
                Console.WriteLine($"Null object {typeof(T)}");

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {Tools.Convert(errorType)}");
            Console.WriteLine($"Details: {e}");
            return default;
        }
    }

    public static async Task<T?> ExecuteWithHandlingAsync<T>(this ColorChessContext db, Func<ColorChessContext, Task<T>> func, ErrorType errorType = ErrorType.UnknownError)
    {
        try
        {
            var result = await func(db);

            if (result == null)
                Console.WriteLine($"Null object {typeof(T)}");

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {Tools.Convert(errorType)}");
            Console.WriteLine($"Details: {e}");
            return default;
        }
    }


    public static int GetEntityId<T>(this T entity) where T : class
    {
        var propertyInfo = entity.GetType().GetProperty("Id");
        if (propertyInfo == null)
        {
            throw new InvalidOperationException("Entity does not have an Id property.");
        }

        return (int)propertyInfo.GetValue(entity);
    }
}



//public static User GetUser(string userName)
//{
//    using (ColorChessContext db = new ColorChessContext())
//    {
//        try
//        {
//            var user = db.users.Where(b => b.Name == userName).ToList();

//            if (user.Count == 0)
//            {
//                Console.WriteLine("Error GetUser: " + Error.NotFound);
//                return null;
//            }

//            return user[0];
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            Console.WriteLine("Error GetUser: " + Error.NotFound);
//            return null;
//        }
//    }
//}





public class Pair<T1, T2>
{
    public T1 First { get; set; }
    public T2 Second { get; set; }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}

