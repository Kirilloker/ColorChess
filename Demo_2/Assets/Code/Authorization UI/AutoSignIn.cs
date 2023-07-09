using System;
using System.Collections;
using System.Net.Http;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
 

    private Hashtable gameData;

    public void Start()
    {
        Authorization();
    }

    public async void Authorization() 
    {
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
                // �������� ������������ �� ������ ��� ������� ����
                icon_online_game.SetActive(true);
            }    
            else
            {
                AccountText.text = "Not found Account!";
                return;
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
