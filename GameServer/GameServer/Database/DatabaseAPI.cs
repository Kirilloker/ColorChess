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
    /// Возвращает пароль Пользователя
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
    static List<GameStatistic> GetGameStatisticUser(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.GameStatistics.Where(b => (b.User2Id == userId || b.User1Id == userId)).ToList();
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
    /// Возврашает строку Lobby
    /// </summary>
    static Lobby GetLobby(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Lobbies.Where(b => (b.UserId == userId)).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetLobby: " + Error.NotFound);
                return null;
            }
        }
    }

    /// <summary>
    /// Возврашает строку Lobby
    /// </summary>
    static Lobby GetLobby(User user)
    {
        return GetLobby(user.Id);
    }


    /// <summary>
    /// Возврашает комнату
    /// </summary>
    static Room GetRoom(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Rooms.Where(b => (b.User2Id == userId || b.User1Id == userId)).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetRoom: " + Error.NotFound);
                return null;
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

    /// <summary>
    /// Возврашает все Lobby
    /// </summary>
    public static List<Lobby> GetAllLobby()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Lobbies.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetAllLobby: " + Error.Unknown);
                return null;
            }
        }
    }

    /// <summary>
    /// Возврашает все комнаты
    /// </summary>
    public static List<Room> GetAllRoom()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Rooms.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetAllRoom: " + Error.Unknown);
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

    public static void AddUserInLobby(int userId, GameMode gameMode)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Lobby> lobbies = db.Lobbies.Where(b => b.UserId == userId).ToList();

                if (lobbies.Count == 0) 
                {
                    Lobby lobby = new Lobby { UserId = userId, GameMode = gameMode };

                    db.Lobbies.Add(lobby);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error AddUserInLobby: " + Error.UserInLobbyExist);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddUserInLobby: " + Error.Unknown);
            }

        }
    }



    /// <summary>
    ///  Добавление информации за игру
    /// </summary>
    public static void AddGameStatistic(int time, int user1Score, int user2Score, DateTime dateTime, GameMode gameMode, User user1, User user2)
    {
        AddGameStatistic(time, user1Score, user2Score, dateTime, gameMode, user1.Id, user2.Id);
    }

    /// <summary>
    ///  Добавление информации за игру
    /// </summary>
    public static void AddGameStatistic(int time, int user1Score, int user2Score, DateTime dateTime, GameMode gameMode, int user1Id, int user2Id)
    {

        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {

                GameStatistic statistic = new GameStatistic
                {
                    Time = time,
                    Player1Score = user1Score,
                    Player2Score = user2Score,
                    Date = dateTime,
                    GameMode = gameMode,
                    User1Id = user1Id,
                    User2Id = user2Id
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

    /// <summary>
    /// Добавление комнаты
    /// </summary>
    public static void AddRoom(int user1Id, int user2Id, string map, GameMode gameMode)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Room> rooms = db.Rooms.Where(b => 
                b.User1Id == user1Id || 
                b.User1Id == user2Id ||
                b.User2Id == user1Id ||
                b.User2Id == user2Id 
                ).ToList();

                if (rooms.Count == 0)
                {
                    Room room = new Room { User1Id = user1Id, User2Id = user2Id, Map = map, GameMode = gameMode };
                    db.Rooms.Add(room);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error AddRoom: " + Error.RoomExist);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error AddRoom: " + Error.Unknown);
            }
        }
    }


    /// <summary>
    /// Добавление комнаты
    /// </summary>
    public static void AddRoom(User user1, User user2, string map, GameMode gameMode) 
    {
        AddRoom(user1.Id, user2.Id, map, gameMode);
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
    /// Изменяет Map в Room
    /// </summary>
    public static void ChangeRoom(int userId, string map)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Room> rooms = db.Rooms
                    .Where(b => b.User1Id == userId || b.User2Id == userId)
                    .ToList();

                if (rooms.Count == 1) 
                {
                    Room room = rooms[0];
                    room.Map = map;
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error ChangeRoom: " + Error.Unknown);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ChangeRoom: " + Error.NotFound);
            }
        }
    }

    /// <summary>
    /// Изменяет Map в Room
    /// </summary>
    public static void ChangeRoom(User user, string map) 
    {
        ChangeRoom(user.Id, map);
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
                        break;
                    case AttributeUS.Lose:
                        userStatistic.Lose += value;
                        break;
                    case AttributeUS.MaxScore:
                        userStatistic.MaxScore += value;
                        break;
                    case AttributeUS.Draw:
                        userStatistic.Draw += value;
                        break;
                    case AttributeUS.Rate:
                        userStatistic.Rate += value;
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

    /// <summary>
    /// Удаляет пользователя из лобби 
    /// </summary>
    public static void DeleteUserInLobby(int userId) 
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Lobby> lobbies = db.Lobbies.Where(b => b.UserId == userId).ToList();

                db.Lobbies.RemoveRange(lobbies);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error DeleteUserInLobby: " + Error.NotFound);
            }
        }
    }

    /// <summary>
    /// Удаляет пользователя из лобби 
    /// </summary>

    public static void DeleteUserInLobby(User user)
    {
        DeleteUserInLobby(user.Id);
    }

    /// <summary>
    /// Удаляет все комнаты связанные с пользователем
    /// </summary>
    public static void DeleteRoom(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Room> rooms = db.Rooms.Where(b => b.User1Id == userId || b.User2Id == userId).ToList();

                db.Rooms.RemoveRange(rooms);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error DeleteRoom: " + Error.NotFound);
            }
        }
    }




    #endregion

    #region ClearDB

    /// <summary>
    /// Очишает таблицу Lobby
    /// </summary>
    public static void ClearLobby()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Lobby> lobbies = db.Lobbies.ToList();

                db.Lobbies.RemoveRange(lobbies);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ClearLobby: " + Error.Unknown);
            }
        }
    }

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

    /// <summary>
    /// Очишает таблицу Room
    /// </summary>
    public static void ClearRoom()
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Room> rooms = db.Rooms.ToList();

                db.Rooms.RemoveRange(rooms);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error ClearRoom: " + Error.Unknown);
            }
        }
    }
    #endregion

    #region Another Function
    /// <summary>
    /// Поиск противника для пользователя
    /// </summary>
    public static int SearchOpponent(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Lobby> lobbyUserlist = db.Lobbies.Where(b => b.UserId == userId).ToList();

                if (
                    (lobbyUserlist.Count == 1)
                    &&
                    (
                    lobbyUserlist[0].GameMode == GameMode.Default
                    ||
                    lobbyUserlist[0].GameMode == GameMode.Rating
                    )
                   )
                {
                    Lobby userLobby = lobbyUserlist[0];

                    List<Lobby> opponentLobbyList = db.Lobbies
                        .Where(
                            b => (
                                (b.UserId != userId)
                                &&
                                (b.GameMode == userLobby.GameMode)
                            )
                            )
                        .ToList();

                    if (opponentLobbyList.Count != 0)
                    {

                        if (userLobby.GameMode == GameMode.Rating)
                        {
                            UserStatistic userStatistic = db.UserStatistics
                                .Where(b => b.UserId == userLobby.UserId).ToList()[0];

                            for (int i = 0; i < opponentLobbyList.Count; i++)
                            {
                                int opponentId = opponentLobbyList[i].Id;

                                UserStatistic opponentStatistic = db.UserStatistics
                                    .Where(b => b.UserId == opponentId).ToList()[0];

                                if (Math.Abs(userStatistic.Rate - opponentStatistic.Rate) <= 10000)
                                {
                                    return opponentLobbyList[i].UserId;
                                }
                            }

                            Console.WriteLine("Error SearchOpponent: " + Error.NotFoundOpponent);
                            return -1;
                        }
                        else if (userLobby.GameMode == GameMode.Default)
                        {
                            return opponentLobbyList[0].UserId;
                        }
                        else
                        {
                            Console.WriteLine("Error SearchOpponent: " + Error.NotFoundEnum);
                            return -1;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Error SearchOpponent: " + Error.NotFoundOpponent);
                        return -1;
                    }
                }
                else if (lobbyUserlist.Count == 1 && lobbyUserlist[0].GameMode == GameMode.Custom)
                {
                    Console.WriteLine("Custom not working!!!");
                    return -1;
                }
                else
                {
                    Console.WriteLine("Error SearchOpponent: " + Error.NotFound);
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error SearchOpponent: " + Error.Unknown);
                return -1;
            }
        }
    }



    #endregion

}