using Newtonsoft.Json;
using System.IO;

namespace ConfigServer
{
    internal class ConfigData
    {
        [JsonProperty("ip-server")]
        public string? IpServer { get; set; }

        [JsonProperty("ip-db")]
        public string? IpDB { get; set; }

        [JsonProperty("user-db")]
        public string? UserDB { get; set; }

        [JsonProperty("password-db")]
        public string? PasswordDB { get; set; }

        [JsonProperty("name-db")]
        public string? NameDB { get; set; }
    }

    public static class Config
    {
        private const string ConfigFilePath = "config.json";

        public static void LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
                throw new Exception("Not found config file");

            var json = File.ReadAllText(ConfigFilePath);
            var conf =  JsonConvert.DeserializeObject<ConfigData>(json);

            IpServer = conf.IpServer;
            IpDB = conf.IpDB;
            UserDB = conf.UserDB;
            PasswordDB = conf.PasswordDB;
            NameDB = conf.NameDB;
        }

        public static string? IpServer { get; set; }
        public static string? IpDB { get; set; }
        public static string? UserDB { get; set; }
        public static string? PasswordDB { get; set; }
        public static string? NameDB { get; set; }
    }
}
