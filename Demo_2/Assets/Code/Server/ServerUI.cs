using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;

public class ServerUI : MonoBehaviour
{
    // Online Game
    // идет ли поиск игры
    bool search = false;
    // Режим 2 игрока
    bool twoHuman = true;
    // Режим 4 игрока
    bool fourHuman = false;
    // Рейтинговая игра
    bool rateGame = false;

    // Зеленая картинка на режиме 2 игроков
    [SerializeField]
    GameObject greenTwoHuman;
    // Зеленая картинка на режиме 4 игроков
    [SerializeField]
    GameObject greenFourHuman;
    // Зеленая картинка на Рейтинговой игре
    [SerializeField]
    GameObject greenRate;

    [SerializeField]
    CameraController cameraController;
    [SerializeField]
    Server server;
    [SerializeField]
    GameObject startBut;
    [SerializeField]
    GameObject backBut;
    [SerializeField]
    TextMeshProUGUI topText;
    [SerializeField]
    TextMeshProUGUI searchText;
    [SerializeField]
    GameObject searchUI;
    [SerializeField]
    GameObject startUI;

    public void ClickTwoHuman()
    {
        if (search == true) return;
        twoHuman = !twoHuman;
        greenTwoHuman.SetActive(twoHuman);
    }

    public void ClickFourHuman()
    {
        if (search == true) return;
        fourHuman = !fourHuman;
        greenFourHuman.SetActive(fourHuman);
    }

    public void ClickRate()
    {
        if (search == true) return;
        rateGame = !rateGame;
        greenRate.SetActive(rateGame);
    }

    public void StartSearch()
    {
        Debug.Log("Поиск игры");
        if (twoHuman == false && fourHuman == false) return;
        search = true;

        startBut.SetActive(false);
        backBut.SetActive(false);

        searchUI.SetActive(true);
        startUI.SetActive(false);

        cameraController.CameraToDesktop();
        RefreshTopList(GetTopList());

        StartCoroutine(AnimationSearch());

        server.ConnectToDefaultGame();
    }

    IEnumerator AnimationSearch() 
    {
        while (search) 
        {
            yield return new WaitForSeconds(1f);

            if (searchText.text == "Search.") searchText.text = "Search..";
            else if (searchText.text == "Search..") searchText.text = "Search...";
            else if (searchText.text == "Search...") searchText.text = "Search.";
        }

        // Если нажали отмену поиска или нашлась игра
        yield return new WaitForSeconds(2f);

        searchUI.SetActive(false);
        startUI.SetActive(true);
    }

    public void StopSearch()
    {
        search = false;
        startBut.SetActive(true);
        backBut.SetActive(true);

        cameraController.CameraToMenu();
    }

    public int GetNumberPlaceUserInTop()
    {
        string jsonString = server.GetNumberPlaceUserInTop(GetNameUser());
        return int.Parse(jsonString);
    }

    public string GetNameUser()
    {
        string name = "???";
        try
        {
            name = PlayerPrefs.GetString("Login");
        }
        catch {}
        return name;
    }

    public List<Pair<string, int>> GetTopList()
    {
        string jsonString = server.GetTopList(GetNameUser());
        List<Pair<string, int>> deserializedPairList = JsonSerializer.Deserialize<List<Pair<string, int>>>(jsonString);
        return deserializedPairList;
    }

    public void RefreshTopList(List<Pair<string, int>> top)
    {
        string Text = "";

        for (int i = 0; i < top.Count; i++)
        {
            // Если человек не входит в топ 5
            if (i == 5)
            {
                Text += "..." + "\n";
                Text += GetNumberPlaceUserInTop().ToString() + ". ";
                Text += GetNameUser() + " - ";
                Text += top[i].Second.ToString();
                break;
            }

            Text += (i+1).ToString() + ". " + top[i].First + " - " + top[i].Second.ToString() + "\n";
        }

        topText.text = Text;
    }
}

