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
    private const string GameServerHubUrl = "http://192.168.1.38:11000/Game";
    private const string LoginInUrl = "http://192.168.1.38:11000/login";

    private string UserName = "tealvl";
    private string Password = "qwerty02";

    private bool IsLoggedIn = false;

    public void ConnectToDefaultGame()
    {
        ConnectToGameServerHubAndFindTheRoom();
    }
    public void SendStep(Step clientStep)        
    {
        SendStepToServer(clientStep);
    }
    public void CloseConnection()
    {
        connection.StopAsync();
    }
    //________________________________________________________________

    private void ServerSendStep(string opponentStep)
    {
        GameObject.Find("DebugUI").GetComponent<DebugConsole>().PrintUI("server send step");
        Step step = TestServerHelper.ConvertJSONtoSTEP(opponentStep);
        ApplyPlayerStep(step);
    }

    private void ServerStartGame(string gameState)
    {
        Map map = TestServerHelper.ConvertJSONtoMap(gameState);
        StartGame(map);
    }
    //_______________________________________________________________

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
        //_connection.On<string>("ServerEndGame", ServerSendStep);

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
    private async void SendStepToServer(Step clientStep)
    {
        await connection.InvokeAsync("SendPlayerStep", clientStep);
    }
    //_______________________________________________________________
    private async void StartGame(Map map)
    {
       await Task.Run(() => { gameController.StartGame(map); });
    }

    private async void ApplyPlayerStep(Step step)
    {
        await Task.Run(() => { gameController.ApplyStepView(step); });
    }
    //________________________________________________________________

    //private async void LoginIn()
    //{
    //    try
    //    {
    //        HttpClient client = new HttpClient();
    //        HttpContent content = new StringContent(UserName + " " + Password);
    //        HttpResponseMessage response = await client.PostAsync(LoginInUrl, content);
    //        string contentText = await response.Content.ReadAsStringAsync();
    //        IsLoggedIn = true;

    //    }
    //}
}
