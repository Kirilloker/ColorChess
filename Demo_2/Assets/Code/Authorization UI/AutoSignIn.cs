using ColorChessModel;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AutoSignIn : MonoBehaviour
{ 
    [SerializeField]
    GameObject AuthorizationMenu;
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    TMP_Text AccountText;
    [SerializeField]
    GameObject StartMenuDesktop;
    [SerializeField]
    Server server;
    [SerializeField]
    GameObject icon_online_game;
    [SerializeField]
    GameObject icon_unable_online_game;
    [SerializeField]
    CameraController cameraController;
 

    private Hashtable gameData;

    public void Start()
    {
        Authorization();
    }

    public async void Authorization() 
    {
        icon_online_game.SetActive(false);

        if (Application.internetReachability == NetworkReachability.NotReachable) 
        {
            // нет интернета 
            AccountText.text = "Intertnet not working!";
            AuthorizationMenu.SetActive(false);
            MainMenu.SetActive(true);
            return;
        }


        BinarySerializer serializer = new BinarySerializer();
        serializer.LoadData();
        GameData = serializer.GetData();

        string login = TryGetValueInHashTable("login");
        string password = TryGetValueInHashTable("password");

        try
        {
            // Если такой аккаунт существует
            if (await server.TryLoginIn(login, password) == true)
            {
                PlayerPrefs.SetString("Login", login);

                AccountText.text = "Account: " + login;
                iconSetOnline(true);
                AuthorizationMenu.SetActive(false);
                MainMenu.SetActive(true);
            }    
            // Есл логин\пароль не правильный
            else
            {
                AccountText.text = "Not found Account!";
                iconSetOnline(false);
                return;
            }
        }
        // Если произошла ошибка с сервером
        catch (Exception ex)
        {
            Print.Log("Сервер не работает");
            AccountText.text = "Server not responding";
            iconSetOnline(false);
            AuthorizationMenu.SetActive(false);
            MainMenu.SetActive(true);
            return;
        }
    }


    void iconSetOnline(bool IsOnline) 
    {
        icon_online_game.SetActive(IsOnline);
        icon_unable_online_game.SetActive(!IsOnline);
    }

    private string TryGetValueInHashTable(string key)
    {
        try
        {
            if (GameData.ContainsKey(key) == true)
                return (string)gameData[key];
            else
                Print.Log("Не удалось достать значение по ключу:" + key);
        }
        catch
        {
            Print.Log("ОШИБКА не удалось достать значение по ключу:" + key);
        }

        return "";
    }

    public Hashtable GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }
}
