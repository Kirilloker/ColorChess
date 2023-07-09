using System;
using System.Collections;
using System.Net.Http;
using System.Net.Sockets;
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
            // ��� ��������� 
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
            if (await server.TryLoginIn(login, password) == true)
            { 
                AccountText.text = "Account: " + login;
                // �������� ������ ��� ������� ����
                icon_online_game.SetActive(true);
            }    
            else
            {
                AccountText.text = "Not found Account!";
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("������ �� ��������:" + ex);
            AccountText.text = "Server not responding";
        }
        catch (HttpRequestException ex)
        {
            Debug.Log("������ �� ��������:" + ex);
            AccountText.text = "Server not responding";
        }
        catch (Exception ex)
        { 
            // ������
            Debug.Log("��������� ������: " + ex);
            AccountText.text = "Unknown error when connecting to server";
        }
        finally 
        {
            AuthorizationMenu.SetActive(false);
            MainMenu.SetActive(true);
            cameraController.CameraToMenu();
        }

    }

    private string TryGetValueInHashTable(string key)
    {
        try
        {
            if (GameData.ContainsKey(key) == true)
                return (string)gameData[key];
            else
                Debug.Log("�� ������� ������� �������� �� �����:" + key);
        }
        catch
        {
            Debug.Log("������ �� ������� ������� �������� �� �����:" + key);
        }

        return "";
    }

    public Hashtable GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }
}
