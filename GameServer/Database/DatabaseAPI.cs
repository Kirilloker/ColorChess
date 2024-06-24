using FirstEF6App;
using GameServer.Enum;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Database
{
    public static class DB
    {
        public static async Task<T?> GetAsync<T>(int id) where T : class, IId
        {
            using ColorChessContext db = new();
            try
            {
                return await db.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return null;
            }
        }

        public static async Task<string?> GetNameUserAsync(int userID)
        {
            using ColorChessContext db = new();
            User? user;

            try
            {
                user = await db.users.FirstOrDefaultAsync(b => b.Id == userID);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return null;
            }

            if (user == null)
                return null;

            return user.Name;
        }

        public static async Task<int?> GetRateUserAsync(int userID)
        {
            using ColorChessContext db = new();
            UserStatistic? userStatistic;

            try
            {
                userStatistic = await db.userstatistics.Where(b => b.UserId == userID).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return null;
            }

            if (userStatistic == null)
                return null;

            return userStatistic.Rate;
        }

        public static async Task<int?> GetRateUserAsync(string userName)
        {
            User? user = await GetUserAsync(userName);

            if (user == null)
                return null;

            return await GetRateUserAsync(user.Id);
        }

        public static async Task<List<KeyValuePair<string, int>>> GetListTopRateAsync(int countTop = 5)
        {
            using ColorChessContext db = new();
            List<KeyValuePair<string, int>> top = new(countTop);

            for (int i = 0; i < countTop; i++)
                top.Add(new KeyValuePair<string, int>(i.ToString(), 0));

            try
            {
                var userStatistics = await db.userstatistics.OrderByDescending(u => u.Rate).Take(countTop).ToListAsync();

                for (int i = 0; i < userStatistics.Count; i++)
                {
                    User? user = await GetAsync<User>(userStatistics[i].UserId);
                    top[i] = new(user != null ? user.Name : "?", userStatistics[i].Rate);
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
            }

            return top;
        }

        public static async Task<int?> GetNumberPlaceUserByRateAsync(string userName)
        {
            using ColorChessContext db = new();
            try
            {
                var userRank = await db.userstatistics.OrderByDescending(u => u.Rate).ToListAsync();

                var user = userRank
                    .Select(async (userStat, index) => new { UserName = await GetNameUserAsync(userStat.UserId), Rank = index + 1 })
                    .FirstOrDefault(u => u.Result.UserName == userName);

                if (user == null)
                    return null;

                return user.Result.Rank;
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return null;
            }
        }

        public static async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            using ColorChessContext db = new();
            try
            {
                return await db.Set<T>().ToListAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return new List<T>();
            }
        }

        public static void AddGameStatistic(GameStatistic statistic)
        {
            using ColorChessContext db = new();
            db.gamestatistics.Add(statistic);
            db.SaveChanges();
        }

        public static void AddLogEvent(LogEvent logEvent)
        {
            using ColorChessContext db = new();
            db.logevents.Add(logEvent);
            db.SaveChanges();
        }

        public static void ChangeUserStatistic(int userId, AttributeUS attribute, int value)
        {
            using ColorChessContext db = new();
            UserStatistic? userStatistic = default;
            try
            {
                userStatistic = db.userstatistics.Where(b => b.UserId == userId).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (userStatistic == null)
                return;

            switch (attribute)
            {
                case AttributeUS.Win:
                    userStatistic.Win += value;
                    if (userStatistic.Win < 0) userStatistic.Win = 0;
                    break;
                case AttributeUS.Lose:
                    userStatistic.Lose += value;
                    if (userStatistic.Lose < 0) userStatistic.Lose = 0;
                    break;
                case AttributeUS.MaxScore:
                    userStatistic.MaxScore = value;
                    if (userStatistic.MaxScore < 0) userStatistic.MaxScore = 0;
                    break;
                case AttributeUS.Draw:
                    userStatistic.Draw += value;
                    if (userStatistic.Draw < 0) userStatistic.Draw = 0;
                    break;
                case AttributeUS.Rate:
                    userStatistic.Rate += value;
                    if (userStatistic.Rate < 0) userStatistic.Rate = 0;
                    break;
                default:
                    Console.WriteLine("Error ChangeUserStatistic: " + ErrorMessage.UnknownAttribute);
                    break;
            }

            db.SaveChanges();
        }

        public static async Task<List<LogEvent>> GetEventsAsync(DateTime start, DateTime end)
        {
            using ColorChessContext db = new();
            try
            {
                return await db.logevents
                              .Where(e => e.Date >= start && e.Date <= end)
                              .ToListAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return new List<LogEvent>();
            }
        }

        public static async Task<List<LogEvent>> GetEventsWithTypesAsync(DateTime start, DateTime end, List<LogEventType> typeEvents)
        {
            using ColorChessContext db = new();
            try
            {
                return await db.logevents
                              .Where(e => e.Date >= start && e.Date <= end && typeEvents.Contains(e.Type_Event.Value))
                              .ToListAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return new();
            }
        }

        public static async Task<int?> AddEntityAsync<T>(T entity) where T : class
        {
            using ColorChessContext db = new();
            try
            {
                await db.Set<T>().AddAsync(entity);
                await db.SaveChangesAsync();
                return entity.GetEntityId();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return null;
            }
        }

        public static User? GetUser(string userName)
        {
            using var db = new ColorChessContext();
            try
            {
                return db.users.FirstOrDefault(b => b.Name == userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static async Task<User?> GetUserAsync(string userName)
        {
            using var db = new ColorChessContext();
            return await db.users.FirstOrDefaultAsync(b => b.Name == userName);
        }

        public static UserStatistic? GetUserStatistic(int userId)
        {
            using var db = new ColorChessContext();
            try
            {
                return db.userstatistics.FirstOrDefault(b => b.UserId == userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static async Task<UserStatistic?> GetUserStatisticAsync(int userId)
        {
            using var db = new ColorChessContext();
            try
            {
                return await db.userstatistics.FirstOrDefaultAsync(b => b.UserId == userId);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return null;
            }
        }

        public static void IDK_how_fix_this_bug()
        {
            // Когда пользователь начинает поиск, в БД появляются 2 записи о том 
            // Что он авторизовался
            using ColorChessContext db = new();
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
    public static class DbContextExtensions
    {
        public static int? GetEntityId<T>(this T entity) where T : class
        {
            var propertyInfo = entity.GetType().GetProperty("Id") ?? throw new InvalidOperationException("Entity does not have an Id property.");
            var id = propertyInfo.GetValue(entity);

            if (id == null)
                return null;

            return (int)id;
        }
    }
}