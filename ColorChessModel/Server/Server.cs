using ColorChessModel;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Http;


public class Server
{
    public MainController mainController;
    private HubConnection connection;

    private bool isLoginIn = false;

    private const string baseIP = "192.168.0.18";

    private const string GameServerHubUrl = "http://" + baseIP + ":11000/Game";
    private const string LoginInUrl = "http://" + baseIP + ":11000/login";
    private const string TopUrl = "http://" + baseIP + ":11000/top"; 
    private const string PlaceInTopUrl = "http://" + baseIP + ":11000/placeInTop";
    private const string RegistrationUrl = "http://" + baseIP + ":11000/registry";

    private string userName;
    private string password;


    private IServerSender serverSender;

    public void SetServerSender(IServerSender serverSender)
    {
        this.serverSender = serverSender;
    }



    //Публичный интерфейс класса_______________________________________
    public void ConnectToDefaultGame(List<string> args)
    {
        ConnectToGameServerHubAndFindTheRoom(args);
    }
    public void SendStep(Step clientStep)
    {
        SendStepToServer(JSONConverter.ConvertToJSON(clientStep));
    }

    public async void SendLastStep(Step clientStep) 
    {
        await SendStepToServer(JSONConverter.ConvertToJSON(clientStep));
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
        return isLoginIn;
    }

    public async Task<bool> TryRegistry(string name, string password)
    {
        return await Registry(name, password);
    }


    public string GetTopList(string nameUser)
    {
        HttpClient client = new HttpClient();
        UriBuilder uriBuilder = new UriBuilder(TopUrl);

        // Создание коллекции параметров query string
        var queryParameters = new NameValueCollection();

        // Добавление параметров в коллекцию query string
        queryParameters["name"] = nameUser;

        // Преобразование коллекции параметров в строку query string
        string queryString = string.Join("&", Array.ConvertAll(queryParameters.AllKeys, key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(queryParameters[key])}"));

        // Присоединение параметров query string к URL-адресу
        uriBuilder.Query = queryString;

        // Получение полного URL-адреса с параметрами query string
        string fullUrl = uriBuilder.ToString();
        HttpResponseMessage response = client.GetAsync(fullUrl).Result;
        string result = response.Content.ReadAsStringAsync().Result;

        return result;
    }

    public string GetNumberPlaceUserInTop(string nameUser)
    {
        HttpClient client = new HttpClient();
        UriBuilder uriBuilder = new UriBuilder(PlaceInTopUrl);

        // Создание коллекции параметров query string
        var queryParameters = new NameValueCollection();

        // Добавление параметров в коллекцию query string
        queryParameters["name"] = nameUser;

        // Преобразование коллекции параметров в строку query string
        string queryString = string.Join("&", Array.ConvertAll(queryParameters.AllKeys, key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(queryParameters[key])}"));

        // Присоединение параметров query string к URL-адресу
        uriBuilder.Query = queryString;

        // Получение полного URL-адреса с параметрами query string
        string fullUrl = uriBuilder.ToString();
        HttpResponseMessage response = client.GetAsync(fullUrl).Result;
        string result = response.Content.ReadAsStringAsync().Result;

        return result;
    }

    //Методы вызываемы сервером во время игры________________________
    private void ServerSendStep(string opponentStep)
    {
        Step step = JSONConverter.ConvertJSONtoSTEP(opponentStep);
        serverSender.SendStep(step);
    }
    private void ServerStartGame(string gameState)
    {
        Print.Log(gameState);
        Map map = JSONConverter.ConvertJSONtoMap(gameState);
        serverSender.StartGame(map);
    }
    private void ServerEndGame()
    {
        serverSender.EndGame();
    }
   
    //Методы для обращений к серверу__________________________________
    private async void ConnectToGameServerHubAndFindTheRoom(List<string> args)
    {
        var _connection = new HubConnectionBuilder()
               .WithUrl(GameServerHubUrl, options =>
               {
                   options.AccessTokenProvider = async () =>
                   {
                       HttpClient client = new HttpClient();
                       HttpContent content = new StringContent(userName + " " + password);
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
        IHttpClientForServer client = httpClient;
        client.Timeout = TimeSpan.FromSeconds(5); // Устанавливаем таймаут в 5 секунд

        HttpContent content = new StringContent(_name + " " + _password);
        HttpResponseMessage response;

        {
            response = await client.PostAsync(LoginInUrl, content);
            string result = response.StatusCode.ToString();

            if (result == "OK")
            {
                userName = _name;
                password = _password;
                isLoginIn = true;
            }
            else if (result == "Unauthorized")
            {
                isLoginIn = false;
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


    public string UserName { get => userName; }
    public string Password { get => password; }
    public bool IsLoginIn { get => isLoginIn; }



    private IHttpClientForServer httpClient;

    public void SetHttpClient(IHttpClientForServer httpClient)
    {
        this.httpClient = httpClient;
    }


    private static Server instance;
    private static readonly object lockObject = new object();

    private Server()
    {
        SetHttpClient(new StandardHttpClient());
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

public interface IHttpClientForServer
{
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    Task<HttpResponseMessage> GetAsync(string requestUri);
    public TimeSpan Timeout { get; set; }
}

public class StandardHttpClient : IHttpClientForServer
{
    private readonly HttpClient httpClient;

    public StandardHttpClient()
    {
        this.httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        return await httpClient.PostAsync(requestUri, content);
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return await httpClient.GetAsync(requestUri);
    }

    public TimeSpan Timeout
    {
        get => httpClient.Timeout;
        set
        {
            httpClient.Timeout = TimeSpan.FromSeconds(value.TotalSeconds);
        }
    }
}