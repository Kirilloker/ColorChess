using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System;

public class SignarRTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        test();
    }

    private async void test()
    {
        var connection = new HubConnectionBuilder()
               .WithUrl("http://192.168.1.38:11000/Game")
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
