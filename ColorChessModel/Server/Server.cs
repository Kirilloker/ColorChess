using ColorChessModel;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Collections.Specialized;

public enum GameMode
{
    Default = 0,
    Rating = 1,
    Custom = 2,
}

public class Server
{
    public MainController mainController;
    private HubConnection connection;

    private bool IsLoginIn = false;

    private const string baseIP = "192.168.0.35";

    private const string GameServerHubUrl = "http://" + baseIP + ":11000/Game";
    private const string LoginInUrl = "http://" + baseIP + ":11000/login";
    private const string TopUrl = "http://" + baseIP + ":11000/top"; 
    private const string PlaceInTopUrl = "http://" + baseIP + ":11000/placeInTop";
    private const string RegistrationUrl = "http://" + baseIP + ":11000/registry";

    private string UserName = "";
    private string Password = "";


    private IServerSender serverSender;

    public void SetServerSender(IServerSender serverSender)
    {
        Print.Log("SetServerSender");
        this.serverSender = serverSender;
    }



    //��������� ��������� ������_______________________________________
    public void ConnectToDefaultGame(List<string> args)
    {
        ConnectToGameServerHubAndFindTheRoom(args);
    }
    public void SendStep(Step clientStep)
    {
        SendStepToServer(ServerHelper.ConvertToJSON(clientStep));
    }

    public async void SendLastStep(Step clientStep) 
    {
        await SendStepToServer(ServerHelper.ConvertToJSON(clientStep));
        await CloseConnection();
    }

    public async Task CloseConnection()
    {     
        if (connection != null)
        {
            await connection.StopAsync();
            connection = null;
        }
    }

    public async Task<bool> TryLoginIn(string name, string password)
    {
        await LoginIn(name, password);
        return IsLoginIn;
    }

    public async Task<bool> TryRegistry(string name, string password)
    {
        return await Registry(name, password);
    }


    public string GetTopList(string nameUser)
    {
        HttpClient client = new HttpClient();
        UriBuilder uriBuilder = new UriBuilder(TopUrl);

        // �������� ��������� ���������� query string
        var queryParameters = new NameValueCollection();

        // ���������� ���������� � ��������� query string
        queryParameters["name"] = nameUser;

        // �������������� ��������� ���������� � ������ query string
        string queryString = string.Join("&", Array.ConvertAll(queryParameters.AllKeys, key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(queryParameters[key])}"));

        // ������������� ���������� query string � URL-������
        uriBuilder.Query = queryString;

        // ��������� ������� URL-������ � ����������� query string
        string fullUrl = uriBuilder.ToString();
        HttpResponseMessage response = client.GetAsync(fullUrl).Result;
        string result = response.Content.ReadAsStringAsync().Result;

        Print.Log(result);

        return result;
    }

    public string GetNumberPlaceUserInTop(string nameUser)
    {
        HttpClient client = new HttpClient();
        UriBuilder uriBuilder = new UriBuilder(PlaceInTopUrl);

        // �������� ��������� ���������� query string
        var queryParameters = new NameValueCollection();

        // ���������� ���������� � ��������� query string
        queryParameters["name"] = nameUser;

        // �������������� ��������� ���������� � ������ query string
        string queryString = string.Join("&", Array.ConvertAll(queryParameters.AllKeys, key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(queryParameters[key])}"));

        // ������������� ���������� query string � URL-������
        uriBuilder.Query = queryString;

        // ��������� ������� URL-������ � ����������� query string
        string fullUrl = uriBuilder.ToString();
        HttpResponseMessage response = client.GetAsync(fullUrl).Result;
        string result = response.Content.ReadAsStringAsync().Result;

        Print.Log(result);

        return result;
    }

    //������ ��������� �������� �� ����� ����________________________
    private void ServerSendStep(string opponentStep)
    {
        Print.Log("ServerSendStep");
        Step step = ServerHelper.ConvertJSONtoSTEP(opponentStep);
        serverSender.SendStep(step);
    }
    private void ServerStartGame(string gameState)
    {
        Print.Log("ServerStartGame1");
        Print.Log(gameState);
        Map map = ServerHelper.ConvertJSONtoMap(gameState);
        Print.Log("Create Map");
        serverSender.StartGame(map);
    }
    private void ServerEndGame()
    {
        Print.Log("ServerEndGame");
        serverSender.EndGame();
    }
   
    //������ ��� ��������� � �������__________________________________
    private async void ConnectToGameServerHubAndFindTheRoom(List<string> args)
    {
        var _connection = new HubConnectionBuilder()
               .WithUrl(GameServerHubUrl, options =>
               {
                   options.AccessTokenProvider = async () =>
                   {
                       HttpClient client = new HttpClient();
                       HttpContent content = new StringContent(UserName + " " + Password);
                       HttpResponseMessage response = await client.PostAsync(LoginInUrl, content);
                       string contentText = await response.Content.ReadAsStringAsync();
                       string token = JsonConvert.DeserializeObject<AccessToken>(contentText).access_token;
                       return token;
                   };
                   options.UseDefaultCredentials = true;
               })
               .Build();

        _connection.On<string>("ServerStartGame", ServerStartGame);
        _connection.On<string>("ServerSendStep", ServerSendStep);
        _connection.On("ServerEndGame", ServerEndGame);

        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Print.Log(ex.Message);
        }

        this.connection = _connection;
        await connection.InvokeAsync("FindRoom", args[0], args[1]);
    }
    private async Task SendStepToServer(string clientStep)
    {
        await connection.InvokeAsync("SendPlayerStep", clientStep);
    }

    private async Task LoginIn(string _name, string _password)
    {
        HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5); // ������������� ������� � 5 ������

        HttpContent content = new StringContent(_name + " " + _password);
        HttpResponseMessage response;

        {
            response = await client.PostAsync(LoginInUrl, content);
            string result = response.StatusCode.ToString();

            if (result == "OK")
            {
                UserName = _name;
                Password = _password;
                IsLoginIn = true;
            }
            else if (result == "Unauthorized")
            {
                IsLoginIn = false;
            }
        }
    }


    private async Task<bool> Registry(string _name, string _password)
    {
        HttpClient client = new HttpClient();
        HttpContent content = new StringContent(_name + " " + _password);
        HttpResponseMessage response = await client.PostAsync(RegistrationUrl, content);
        string result = response.StatusCode.ToString();
        
        Print.Log(result);
        
        if (result == "OK")
            return true;
        else if (result == "UnprocessableEntity")
            return false;

        return false;
    }







    private static Server instance;
    private static readonly object lockObject = new object();

    private Server()
    {
    }

    public static Server Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                        instance = new Server();
                }
            }
            return instance;
        }
    }
}