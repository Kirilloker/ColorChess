using ColorChessModel;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameController gameController;

    //Online
    [SerializeField]
    GameObject PlayUI;
    [SerializeField]
    GameObject OnlineUI;
    [SerializeField]
    GameObject SearchButton;
    [SerializeField]
    GameObject BackToMenuInOnlineButton;
    [SerializeField]
    GameObject StopSearchButton;

    public void SelectStandartHotSeat()
    {
        gameController.SelectGameMode(GameModeType.HumanTwo);
    }

    public void SelectStandartAI()
    {
        gameController.SelectGameMode(GameModeType.AI);
    }

    public void SelectStandartNetwork()
    {
        gameController.SelectGameMode(GameModeType.Network);
    }


    public void StartGame()
    {
        gameController.StartGame();
    }

    public void OnlineGameExut()
    {
        PlayUI.SetActive(true);
        BackToMenuInOnlineButton.SetActive(true);
        SearchButton.SetActive(true);

        OnlineUI.SetActive(false);
        StopSearchButton.SetActive(false);
    }
}
