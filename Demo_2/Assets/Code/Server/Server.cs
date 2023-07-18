using ColorChessModel;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Collections;
using System.IO;
using System.Collections.Generic;

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

    private const string baseIP = "185.209.28.214";

    private const string GameServerHubUrl = "http://" + baseIP + ":11000/Game";
    private const string LoginInUrl = "http://" + baseIP + ":11000/login";
    private const string TopUrl = "http://" + baseIP + ":11000/top"; 
    private const string PlaceInTopUrl = "http://" + baseIP + ":11000/placeInTop";
    private const string RegistrationUrl = "http://" + baseIP + ":11000/registry";

    private string UserName = "";
    private string Password = "";


    //��������� �������� ������_______________________________________
    public void ConnectToDefaultGame(List<string> args)
    {
        ConnectToGameServerHubAndFindTheRoom(args);
    }
    public void SendStep(Step clientStep)
    {
        SendStepToServer(TestServerHelper.ConvertToJSON(clientStep));
    }

    public async void SendLastStep(Step clientStep) 
    {
        await SendStepToServer(TestServerHelper.ConvertToJSON(clientStep));
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

    public async Task<bool> TryRegisry(string name, string password)
    {
        return await Regisry(name, password);
    }


    public string GetTopList(string nameUser) 
    {
        HttpClient client = new HttpClient();
        UriBuilder uriBuilder = new UriBuilder(TopUrl);

        // �������� ��������� ���������� query string
        var queryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);

        // ���������� ���������� � ��������� query string
        queryParameters["name"] = nameUser;
        // ������������� ���������� query string � URL-������
        uriBuilder.Query = queryParameters.ToString();

        // ��������� ������� URL-������ � ����������� query string
        string fullUrl = uriBuilder.ToString();
        HttpResponseMessage response;
        response =  client.GetAsync(fullUrl).Result;
        string result = response.Content.ReadAsStringAsync().Result;

        Debug.Log(result);

        return result;
    }

    public string GetNumberPlaceUserInTop(string nameUser) 
    {
        HttpClient client = new HttpClient();
        UriBuilder uriBuilder = new UriBuilder(PlaceInTopUrl);

        // �������� ��������� ���������� query string
        var queryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);

        // ���������� ���������� � ��������� query string
        queryParameters["name"] = nameUser;
        // ������������� ���������� query string � URL-������
        uriBuilder.Query = queryParameters.ToString();

        // ��������� ������� URL-������ � ����������� query string
        string fullUrl = uriBuilder.ToString();
        HttpResponseMessage response;
        response = client.GetAsync(fullUrl).Result;
        string result = response.Content.ReadAsStringAsync().Result;

        Debug.Log(result);

        return result;
    }

    //������ ��������� �������� �� ����� ����________________________
    private void ServerSendStep(string opponentStep)
    {
        Step step = TestServerHelper.ConvertJSONtoSTEP(opponentStep);
        UnityMainThreadDispatcher.Instance().Enqueue(() => { gameController.ApplyStepView(step); });
    }
    private void ServerStartGame(string gameState)
    {
        Map map = TestServerHelper.ConvertJSONtoMap(gameState);
        UnityMainThreadDispatcher.Instance().Enqueue(() => { gameController.StartGame(map); });
    }
    private void ServerEndGame()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => { gameController.EndGame(); });
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
            Debug.Log(ex.Message);
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

        //try
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
        //catch (TaskCanceledException)
        //{
        //    // ��������� ������, ����� ������ ��� ������� ��-�� ��������� ��������
        //    Debug.Log("������ �� ��������");
        //    IsLoginIn = false;
        //}
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

}