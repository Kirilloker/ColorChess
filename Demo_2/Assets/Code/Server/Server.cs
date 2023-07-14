using ColorChessModel;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Collections;

public enum GameMode
{
    Default = 0,
    Rating = 1,
    Custom = 2,
}

public class Server : MonoBehaviour
{
    public GameController gameController;
    private HubConnection connection;

    private bool IsLoginIn = false;

    private const string baseIP = "192.168.1.100";
    //private const string baseIP = "172.20.10.10";

    private const string GameServerHubUrl = "http://" + baseIP + ":11000/Game";
    private const string LoginInUrl = "http://" + baseIP + ":11000/login";
    private const string RegistrationUrl = "http://" + baseIP + ":11000/registry";

    private string UserName = "";
    private string Password = "";
    

    //Публичный инерфейс класса_______________________________________
    public void ConnectToDefaultGame()
    {
        Debug.Log("ConnectToDefaultGame");
        ConnectToGameServerHubAndFindTheRoom();
    }
    public void SendStep(Step clientStep)        
    {
        SendStepToServer(TestServerHelper.ConvertToJSON(clientStep));
    }
    public void CloseConnection()
    {
        connection.StopAsync();
    }

    public async Task<bool> TryLoginIn(string name, string password)
    {
        await LoginIn(name, password);
        return IsLoginIn;
    }

    public async Task<bool> TryRegisry(string name, string password)
    {
        return await Regisry(name, password);
    }

    public async void DisconectFromServer() 
    {
        await connection.StopAsync();
    }
    
    //Методы вызываемы сервером во время игры________________________
    private void ServerSendStep(string opponentStep)
    {
        Debug.Log("ServerSendStep:" + opponentStep);
        Step step = TestServerHelper.ConvertJSONtoSTEP(opponentStep);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            gameController.ApplyStepView(step);
        });
        //ApplyPlayerStep(step);
    }
    private void ServerStartGame(string gameState)
    {
        Debug.Log("ServerStartGame");
        Map map = TestServerHelper.ConvertJSONtoMap(gameState);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            gameController.StartGame(map);
        });
        //StartGame(map);
    }
    private void ServerEndGame()
    {
        Debug.Log("ServerEndGame");
        gameController.EndGame();
        DisconectFromServer();
        connection = null;
    }
   
    //Методы для обращений к серверу__________________________________
    private async void ConnectToGameServerHubAndFindTheRoom()
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
            Debug.Log(ex.Message);
        }

        this.connection = _connection;
        await connection.InvokeAsync("FindRoom", "Default");
    }
    private async void SendStepToServer(string clientStep)
    {
        await connection.InvokeAsync("SendPlayerStep", clientStep);
    }
    private async Task LoginIn(string _name, string _password)
    {
        Debug.Log("LoginIn");
        HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5); // Устанавливаем таймаут в 5 секунд

        HttpContent content = new StringContent(_name + " " + _password);
        HttpResponseMessage response;

        try
        {
            response = await client.PostAsync(LoginInUrl, content);
            string result = response.StatusCode.ToString();

            Debug.Log(response);

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
        catch (TaskCanceledException)
        {
            // Обработка случая, когда запрос был отменен из-за истечения таймаута
            Debug.Log("Сервер не отвечает");
            IsLoginIn = false;
        }
    }


    private async Task<bool> Regisry(string _name, string _password)
    {
        HttpClient client = new HttpClient();
        HttpContent content = new StringContent(_name + " " + _password);
        HttpResponseMessage response = await client.PostAsync(RegistrationUrl, content);
        string result = response.StatusCode.ToString();
        
        Debug.Log(result);
        
        if (result == "OK")
            return true;
        else if (result == "UnprocessableEntity")
            return false;

        return false;
    }

    //Методы для вызова логики в игре________________________________
    private async void StartGame(Map map)
    {
        Debug.Log("StartGame");
        await Task.Run(() => { gameController.StartGame(map); });
    }
    private async void ApplyPlayerStep(Step step)
    {
        Debug.Log("ApplyPlayerStep");
        await Task.Run(() => { gameController.ApplyStepView(step); }); 
    }

    private IEnumerator StartApplyPlayerStep(Step step)
    {
        gameController.ApplyStepView(step); // Запуск игры

        yield return null; // Ожидание следующего кадра
    }

    private IEnumerator StartGameCoroutine(Map map)
    {
        StartGame(map); // Запуск игры
        yield return null; // Ожидание следующего кадра
    }
}