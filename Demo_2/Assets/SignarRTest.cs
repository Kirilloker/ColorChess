using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;
using UnityEngine.Networking.Types;
using Microsoft.AspNetCore.Connections;

public class SignarRTest : MonoBehaviour
{
    void Start()
    {
        test();
    }

    private async void test()
    {
        var connection = new HubConnectionBuilder()
               .WithUrl("http://192.168.1.38:11000/Game", options => 
               {
                   options.AccessTokenProvider = async () =>
                   {
                       HttpClient client = new HttpClient();
                       HttpContent content = new StringContent("kirillok");
                       HttpResponseMessage response = await client.PostAsync("http://192.168.1.38:11000/login", content);
                       string contentText = await response.Content.ReadAsStringAsync();
                       string token = JsonConvert.DeserializeObject<AccessToken>(contentText).access_token;
                       return token;    
                   };
                   options.UseDefaultCredentials= true;
               })
               .Build();

        while (connection.State != HubConnectionState.Connected)
        {

        }
        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        await connection.InvokeAsync("SendSomeStr", "hello");
    }

    private async void Authoriazation()
    {
        HttpClient httpClient = new HttpClient();
        StringContent stringContent = new StringContent("");
        var response = await httpClient.PostAsync("http://192.168.1.38:11000/login", stringContent);
        Debug.Log(response);
        string responseText = await response.Content.ReadAsStringAsync();

        Debug.Log(responseText);


        //test();

    }
        
}
