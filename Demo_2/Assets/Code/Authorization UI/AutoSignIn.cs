using ColorChessModel;
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

    private Hashtable gameData;

    public void Start()
    {
        Authorization();
    }

    public void Authorization() 
    {
        StartMenuDesktop.SetActive(true);

        BinarySerializer serializer = new BinarySerializer();
        serializer.LoadData();
        GameData = serializer.GetData();

        string login = TryGetValueInHashTable("login");
        string password = TryGetValueInHashTable("password");

        //if (SignIn(login, password) == true) 
        bool x = true;
        if (x)
        {
            AuthorizationMenu.SetActive(false);
            MainMenu.SetActive(true);

            AccountText.text = "Account: " + login;
        }
        else
        {
            AccountText.text = "Not found Account!";
        }
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
