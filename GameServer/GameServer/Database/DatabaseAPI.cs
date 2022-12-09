using FirstEF6App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.Entity;

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
            catch
            {
                Console.WriteLine("Error: " + Error.NotFound);
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
            catch
            {
                Console.WriteLine("Error: " + Error.NotFound);
                return null;
            }
        }
    }


    /// <summary>
    /// Возвращает имя Пользователя
    /// </summary>>
    public static string GetNameUser(int userID)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.Users.Where(b => b.Id == userID).ToList()[0].Name;
            }
            catch 
            {
                Console.WriteLine("Error: " + Error.NotFound);
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
            catch
            {
                Console.WriteLine("Error: " + Error.NotFound);
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
    static List<GameStatistic> GetAllGameStatistic(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                return db.GameStatistics.Where(b => (b.User2Id == userId || b.User1Id == userId)).ToList();
            }
            catch
            {
                Console.WriteLine("Error: " + Error.NotFound);
                return null;
            }
        }
    }

    /// <summary>
    /// Возврашает все статистики игры пользователя
    /// </summary>
    static List<GameStatistic> GetAllGameStatistic(User user)
    {
        return GetAllGameStatistic(user.Id);
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
                    Console.WriteLine("Error: " + Error.UserExist);
                }

            }
            catch 
            {
                Console.WriteLine("Error: " + Error.Unknown);
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
                    Console.WriteLine("Error: " + Error.UserStatisticExist);
                }

            }
            catch
            {
                Console.WriteLine("Error: " + Error.Unknown);
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
                Lobby lobby = new Lobby { UserId = userId, GameMode = gameMode};

                db.Lobby.Add(lobby);
                db.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error: " + Error.Unknown);
            }

        }
    }



    /// <summary>
    ///  Добавление информации за игру
    /// </summary>
    public static void AddGameStatistic(int time, int user1Score, int user2Score, DateTime dateTime, User user1, User user2)
    {
        AddGameStatistic(time, user1Score, user2Score, dateTime, user1.Id, user2.Id);
    }

    /// <summary>
    ///  Добавление информации за игру
    /// </summary>
    public static void AddGameStatistic(int time, int user1Score, int user2Score, DateTime dateTime, int user1Id, int user2Id)
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
                    User1Id = user1Id,
                    User2Id = user2Id
                };

                db.GameStatistics.Add(statistic);
                db.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error: " + Error.Unknown);
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
            catch (Exception)
            {
                Console.WriteLine("Error: " + Error.NotFound);
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
            catch (Exception)
            {
                Console.WriteLine("Error: " + Error.NotFound);
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
                        Console.WriteLine("Error: " + Error.UnknownAttribute);
                        break;
                }

                db.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error: " + Error.Unknown);
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
                List<Lobby> lobbies = db.Lobby.Where(b => b.UserId == userId).ToList();

                db.Lobby.RemoveRange(lobbies);
                db.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error: " + Error.NotFound);
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

    #endregion


    /// <summary>
    /// Поиск противника для пользователя
    /// </summary>
    public static int SearchOpponent(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            try
            {
                List<Lobby> lobbyUserlist = db.Lobby.Where(b => b.UserId == userId).ToList();

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

                    List<Lobby> opponentLobbyList = db.Lobby
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
                        UserStatistic userStatistic = db.UserStatistics
                            .Where(b => b.UserId == userLobby.UserId).ToList()[0];

                        if (userLobby.GameMode == GameMode.Rating)
                        {
                            for (int i = 0; i < opponentLobbyList.Count; i++)
                            {
                                UserStatistic opponentStatistic = db.UserStatistics
                                    .Where(b => b.UserId == opponentLobbyList[i].UserId).ToList()[0];

                                if (Math.Abs(userStatistic.Rate - opponentStatistic.Rate) <= 10000)
                                {
                                    return opponentLobbyList[i].UserId;
                                }
                            }

                            Console.WriteLine("Error:" + Error.NotFoundOpponent);
                            return -1;
                        }
                        else if (userLobby.GameMode == GameMode.Default)
                        {
                            return opponentLobbyList[0].UserId;
                        }
                        else
                        {
                            Console.WriteLine("Error:" + Error.NotFoundEnum);
                            return -1;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Error:" + Error.NotFound);
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
                    Console.WriteLine("Error:" + Error.NotFound);
                    return -1;
                }
            }
            catch
            {
                Console.WriteLine("Error:" + Error.Unknown);
                return -1;
            }
        }
    }





}
