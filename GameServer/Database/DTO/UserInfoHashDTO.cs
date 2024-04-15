﻿namespace GameServer.Database.DTO
{
    public class UserInfoHashDTO
    {
        public string Name { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int Win { get; set; }
        public int Lose { get; set; }
        public int MaxScore { get; set; }
        public int Draw { get; set; }
        public int Rate { get; set; }
    }
}