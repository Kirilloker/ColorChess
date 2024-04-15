using Newtonsoft.Json;
using System.IO;

namespace GameServer
{
    internal class ConfigData
    {
        [JsonProperty("ip-server")]
        public string? IpServer { get; set; }
        [JsonProperty("port-server")]
        public string? PortServer { get; set; }

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
            var conf = JsonConvert.DeserializeObject<ConfigData>(json);

            IpServer = Environment.GetEnvironmentVariable("IPSERVER") ?? conf.IpServer;
            PortServer = Environment.GetEnvironmentVariable("PORTSERVER") ?? conf.PortServer;
            IpDB = Environment.GetEnvironmentVariable("IPDB") ?? conf.IpDB;
            UserDB = Environment.GetEnvironmentVariable("USERDB") ?? conf.UserDB;
            PasswordDB = Environment.GetEnvironmentVariable("PASSWORDDB") ?? conf.PasswordDB;
            NameDB = Environment.GetEnvironmentVariable("NAMEDB") ?? conf.NameDB;
        }

        public static string? IpServer { get; set; }
        public static string? PortServer { get; set; }
        public static string? IpDB { get; set; }
        public static string? UserDB { get; set; }
        public static string? PasswordDB { get; set; }
        public static string? NameDB { get; set; }
    }
}
