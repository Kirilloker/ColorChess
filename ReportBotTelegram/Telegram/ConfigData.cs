﻿using Newtonsoft.Json;

internal class ConfigData
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("serverURL")]
    public string ServerURL { get; set; }

    [JsonProperty("idAdmins")]
    public List<string> IdAdmins { get; set; }
}

public static class Config
{
    private const string ConfigFilePath = "config.json";
    public static string Token { get; private set; }
    public static string ServerURL { get; private set; }
    public static List<string> IdAdmins { get; private set; }


    public static void LoadConfig()
    {
        if (!File.Exists(ConfigFilePath))
            throw new FileNotFoundException("Config file not found.");

        var json = File.ReadAllText(ConfigFilePath);
        var conf = JsonConvert.DeserializeObject<ConfigData>(json);

        Token = Environment.GetEnvironmentVariable("TOKEN") ?? conf.Token;
        ServerURL = Environment.GetEnvironmentVariable("SERVER_URL") ?? conf.ServerURL;

        if (Environment.GetEnvironmentVariable("IDAMINS") != null) 
            IdAdmins = new List<string>() { Environment.GetEnvironmentVariable("IDAMINS") };
        else
            IdAdmins = conf.IdAdmins;
    }
}