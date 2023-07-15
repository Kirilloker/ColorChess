using FirstEF6App;

public static class DB 
{
    #region Get

    /// <summary>
    /// Возврашает пользователя
    /// </summary>
    public static User GetUser(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Users.Where(b => b.Id == userID).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetUser: " + Error.NotFound);
                return null;
            }
        }
    }


    /// <summary>
    /// Возврашает пользователя
    /// </summary>
    public static User GetUser(string userName)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Users.Where(b => b.Name == userName).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetUser: " + Error.NotFound);
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
                return db.Users.Where(b => b.Id == userID).ToList()[0].Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetNameUser: " + Error.NotFound);
                return Error.NotFound;
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
                return db.UserStatistics.Where(b => b.Id == userID).ToList()[0].Rate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetNameUser: " + Error.NotFound);
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

    /// <summary>
    /// Возвращает пароль Пользователя
    /// </summary>>
    public static string GetPasswordUser(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Users.Where(b => b.Id == userID).ToList()[0].Password;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetPasswordUser: " + Error.NotFound);
                return Error.NotFound;
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
                return db.Users.Where(b => b.Name == name).ToList()[0].Password;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetPasswordUser: " + Error.NotFound);
                return Error.NotFound;
            }
        }
    }

    /// <summary>
    /// Возврашает статистику пользователя
    /// </summary>
    public static UserStatistic GetUserStatistic(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.UserStatistics.Where(b => b.UserId == userId).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetUserStatistic: " + Error.NotFound);
                return null;
            }
        }
    }



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
                return db.GameStatistics.Where(b => b.UsersId.Contains(userId)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetGameStatisticUser: " + Error.NotFound);
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
    /// Возвращает Список пары - имя игрока - количество очков. CountTop - колчество элементов в массиве
    /// </summary>
    public static List<Pair<string, int>> GetListTopRate(int countTop = 5) 
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Pair<string, int>> top = new();
                
                List<UserStatistic> userStatistics = db.UserStatistics.OrderByDescending(u => u.Rate).Take(5).ToList();

                foreach (UserStatistic userStat in userStatistics)
                {
                    string userName = GetNameUser(userStat.UserId);
                    top.Add(new Pair<string, int>(userName, userStat.Rate));
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
                var userRank = db.UserStatistics.OrderByDescending(u => u.Rate)
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
                Console.WriteLine("Error GetUserRankByRate: " + Error.NotFound);
                return -1;
            }
        }
    }


    #endregion

    #region Get All DB

    /// <summary>
    /// Возврашает всех пользователей
    /// </summary>
    public static List<User> GetAllUser()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Users.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetAllUser: " + Error.Unknown);
                return null;
            }
        }
    }

    /// <summary>
    /// Возврашает статистики всех пользователей
    /// </summary>
    public static List<UserStatistic> GetAllUserStatistic()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.UserStatistics.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetAllUserStatistic: " + Error.Unknown);
                return null;
            }
        }
    }

    /// <summary>
    /// Возврашает игровые статистики всех пользователей
    /// </summary>
    public static List<GameStatistic> GetAllGameStatistic()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.GameStatistics.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetAllGameStatistic: " + Error.Unknown);
                return null;
            }
        }
    }

    #endregion

    #region Add
    /// <summary>
    /// Добавление нового пользователя
    /// </summary>
    public static void AddUser(string username, string password)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<User> users =  db.Users.Where(b => b.Name == username).ToList();

                if (users.Count == 0) 
                {
                    User user = new User { Name = username, Password = password };

                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error AddUser: " + Error.UserExist);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddUser: " + Error.Unknown);
            }
        }
    }

    /// <summary>
    /// Добавление нового пользователя
    /// </summary>
    public static void AddUser(User user)
    {
        AddUser(user.Name, user.Password);
    }

    /// <summary>
    /// Добавление игровой статистики пользователя
    /// </summary>
    public static void AddUserStatistic(int win, int lose, int draw, int maxScore, int rate, int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<UserStatistic> userStatistics = db.UserStatistics.Where(b => b.UserId == userId).ToList();

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

                    db.UserStatistics.Add(statistic);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error AddUserStatistic: " + Error.UserStatisticExist);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddUserStatistic: " + Error.Unknown);
            }
        }
    }


    /// <summary>
    /// Добавление игровой статистики пользователя
    /// </summary>
    public static void AddUserStatistic(UserStatistic userStatistic)
    {
        AddUserStatistic(userStatistic.Win, userStatistic.Lose,
                         userStatistic.Draw, userStatistic.MaxScore,
                         userStatistic.Rate, userStatistic.UserId);
    }

   


    /// <summary>
    ///  Добавление информации за игру
    /// </summary>
    public static void AddGameStatistic(List<int> usersScore, GameMode gameMode, List<int> usersId)
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

                db.GameStatistics.Add(statistic);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddGameStatistic: " + Error.Unknown);
            }
        }
    }

    #endregion

    #region Changes


    /// <summary>
    /// Изменяет имя пользователя
    /// </summary>
    public static void ChangeNameUser(int userId, string newName)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                User user = db.Users.Where(b => b.Id == userId).ToList()[0];
                user.Name = newName;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeNameUser: " + Error.NotFound);
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
                User user = db.Users.Where(b => b.Name == oldName).ToList()[0];
                user.Name = newName;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeNameUser: " + Error.NotFound);
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
                UserStatistic userStatistic = db.UserStatistics.Where(b => b.UserId == userId).ToList()[0];

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
                        Console.WriteLine("Error ChangeUserStatistic: " + Error.UnknownAttribute);
                        break;
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeUserStatistic: " + Error.Unknown);
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
                List<User> users = db.Users.ToList();

                db.Users.RemoveRange(users);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ClearUsers: " + Error.Unknown);
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
                List<UserStatistic> userStatisitcs = db.UserStatistics.ToList();

                db.UserStatistics.RemoveRange(userStatisitcs);
                db.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error ClearUserStatistics: " + Error.Unknown);
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
                List<GameStatistic> gameStatisitcs = db.GameStatistics.ToList();

                db.GameStatistics.RemoveRange(gameStatisitcs);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ClearGameStatistics: " + Error.Unknown);
            }
        }
    }
    #endregion
}


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

