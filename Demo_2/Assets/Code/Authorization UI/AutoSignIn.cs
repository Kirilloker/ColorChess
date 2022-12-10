using System.Collections;
using UnityEngine;

public class AutoSignIn : MonoBehaviour
{

    [SerializeField]
    GameObject AuthorizationMenu;
    [SerializeField]
    GameObject MainMenu;

    private Hashtable gameData;

    public void Start()
    {
        BinarySerializer serializer = new BinarySerializer();
        serializer.LoadData();
        GameData = serializer.GetData();

        string login = TryGetValueInHashTable("login");
        string password = TryGetValueInHashTable("password");
        
        //if (UserExist(login, password)
        if (false)
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

        return "Error";
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
