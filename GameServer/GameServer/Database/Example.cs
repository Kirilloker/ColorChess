namespace GameServer.Database
{
    public class Example
    {
        public bool SignIn(string login, string password) 
        {
            User user = DB.GetUser(login);

            if (user != null && user.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddNewUser(string login, string password) 
        {
            User user = DB.GetUser(login);

            if (user == null)
            {
                User newUser = new User { Name = login, Password = password };
                DB.AddUser(newUser);

                newUser = DB.GetUser(newUser.Name);

                UserStatistic userStatistic = new UserStatistic { Win = 0, Lose = 0, Draw = 0, MaxScore = 0, Rate = 0, UserId = newUser.Id };
                DB.AddUserStatistic(userStatistic);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
