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
    private const string GameServerHubUrl = "http://192.168.1.116:11000/Game";
    private const string LoginInUrl = "http://192.168.1.116:11000/login";

    private string UserName = "kirillok";
    private string Password = "qwerty01";

    public void ConnectToDefaultGame()
    {
        ConnectToGameServerHubAndFindTheRoom();
    }
    public void SendStep(Step clientStep)
    {
        //ws.Send("2" + TestServerHelper.ConvertToJSON(clientStep));
    }
    //________________________________________________________________
    public void CloseConnection()
    {
        connection.StopAsync();
    }
    private void ServerStartGame(string gameState)
    {
        Map map = TestServerHelper.ConvertJSONtoMap(gameState);
        StartGame(map);
    }
    //_________________________________________

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
    //_________________________________________________________________
    private async void StartGame(Map map)
    {
       await Task.Run(() => { gameController.StartGame(map); });
    }

    private async void ApplyPlayerStep(Step step)
    {
        await Task.Run(() => { gameController.ApplyStepView(step); });
    }  
}
