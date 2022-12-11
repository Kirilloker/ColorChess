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


    private Hashtable gameData;

    public void Start()
    {
        Authorization();
    }

    public async void Authorization() 
    {
        StartMenuDesktop.SetActive(true);

        BinarySerializer serializer = new BinarySerializer();
        serializer.LoadData();
        GameData = serializer.GetData();

        string login = TryGetValueInHashTable("login");
        string password = TryGetValueInHashTable("password");

        if (await server.TryLoginIn(login, password) == true)
        {
            AuthorizationMenu.SetActive(false);
            MainMenu.SetActive(true);

            AccountText.text = "Account: " + login;
        }
        else
        {
            AccountText.text = "Not found Account!";
        }

        //Debug.Log(server.TryLoginIn(login, password).Result);
    }

    private string TryGetValueInHashTable(string key)
    {
        try
        {
            if (GameData.ContainsKey(key) == true)
            {
                return (string)gameData[key];
            }
            else
            {
                Debug.Log("Не удалось достать значение по ключу:" + key);
            }
        }
        catch
        {
            Debug.Log("ОШИБКА не удалось достать значение по ключу:" + key);

        }

        return "";
    }

    public Hashtable GameData
    {
        get
        {
            return gameData;
        }
        set
        {
            gameData = value;
        }
    }
}
