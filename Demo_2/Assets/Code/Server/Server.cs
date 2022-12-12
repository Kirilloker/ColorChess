using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using Newtonsoft.Json;
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

    private const string baseIP = "192.168.1.116";
    //private const string baseIP = "172.20.10.10";

    private const string GameServerHubUrl = "http://" + baseIP + ":11000/Game";
    private const string LoginInUrl = "http://" + baseIP + ":11000/login";
    private const string RegistrationUrl = "http://" + baseIP + ":11000/registry";

    private string UserName = "";
    private string Password = "";
    

    //Публичный инерфейс класса_______________________________________
    public void ConnectToDefaultGame()
    {
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
        ApplyPlayerStep(step);
    }
    private void ServerStartGame(string gameState)
    {
        Map map = TestServerHelper.ConvertJSONtoMap(gameState);
        StartGame(map);
    }
    private void ServerEndGame()
    {
        Debug.Log("ServerEndGame");
        gameController.EndGame();
        connection.StopAsync();
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
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(_name + " " + _password);
            HttpResponseMessage response = await client.PostAsync(LoginInUrl, content);
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

    private async Task<bool> Regisry(string _name, string _password)
    {
        HttpClient client = new HttpClient();
        HttpContent content = new StringContent(_name + " " + _password);
        HttpResponseMessage response = await client.PostAsync(RegistrationUrl, content);
        string result = response.StatusCode.ToString();
        Debug.Log(result);
        if (result == "OK")
        {
            return true;
        }
        else if (result == "UnprocessableEntity")
        {
            return false;
        }

        return false;
    }

    //Методы для вызова логики в игре________________________________
    private async void StartGame(Map map)
    {
       await Task.Run(() => { gameController.StartGame(map); });
    }
    private async void ApplyPlayerStep(Step step)
    {
        await Task.Run(() => { gameController.ApplyStepView(step); });
    }
}