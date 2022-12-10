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
                       HttpContent content = new StringContent("kirillok qwerty01");
                       HttpResponseMessage response = await client.PostAsync("http://192.168.1.38:11000/login", content);
                       string contentText = await response.Content.ReadAsStringAsync();
                       string token = JsonConvert.DeserializeObject<AccessToken>(contentText).access_token;
                       return token;    
                   };
                   options.UseDefaultCredentials= true;
               })
               .Build();
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
}