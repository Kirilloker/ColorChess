
public static class ConfServ
{
    public const string baseIP = "192.168.0.201";
    public const string basePort = "11000";
    
    public const string GameServerHubUrl = "http://" + baseIP + ":" + basePort + "/Game";
    public const string LoginInUrl = "http://" + baseIP + ":" + basePort + "/login";
    public const string TopUrl = "http://" + baseIP + ":" + basePort + "/api/Info/get_top"; 
    public const string PlaceInTopUrl = "http://" + baseIP + ":" + basePort + "/api/Info/get_number_place_top";
    public const string RegistrationUrl = "http://" + baseIP + ":" + basePort + "/registry";
}

