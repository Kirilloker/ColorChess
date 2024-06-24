
 using ColorChessModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    MainController mainController;
    [SerializeField]
    CameraController cameraController;

    //Online
    [SerializeField]
    GameObject PlayUI;
    [SerializeField]
    GameObject OnlineUI;
    [SerializeField]
    GameObject BackToMenuInOnlineButton;
    [SerializeField]
    GameObject StopSearchButton; 

    [SerializeField]
    GameObject GameUI;  
    [SerializeField]
    GameObject MainUI;
    [SerializeField]
    GameObject CustomUI;

    private void Start()
    {
        mainController = MainController.Instance;
    }

    public void SelectFourHumanHotSeat() 
    {
        for (int i = 1; i <= 4; i++)
            SetHumanState(i);
        SizeMap = 10;
        StartCustomGame();
    }

    public void SelectOneHumanThreeAI()
    {
        for (int i = 2; i <= 4; i++)
            SetAIState(i);
        SizeMap = 10;
        StartCustomGame();
    }

    public void SelectStandardHotSeat()
    {
        mainController.SelectGameMode(GameModeType.HumanTwo);
    }

    public void SelectStandardAI()
    {
        mainController.SelectGameMode(GameModeType.AI);
    }

    public void SelectStandardNetwork()
    {
        mainController.SelectGameMode(GameModeType.Network);
    }


    public void StartGame()
    {
        mainController.StartGame();
    }

    public void OnlineGameExit()
    {
        PlayUI.SetActive(true);
        BackToMenuInOnlineButton.SetActive(true);

        OnlineUI.SetActive(false);
    }


    public void ViewUIGame(bool isStartGame) 
    {
        GameUI.SetActive(isStartGame);
        MainUI.SetActive(!isStartGame);
    }


    // Custom Settings
    [SerializeField]    
    Text sizeText;

    int sizeMap = 9;
    PlayerType[] typePlayer = new PlayerType[4] 
        { PlayerType.Human, PlayerType.Human, PlayerType.None, PlayerType.None};

    CornerType[] cornerPlayer = new CornerType[4]
        { CornerType.DownLeft, CornerType.UpRight, CornerType.DownRight, CornerType.UpLeft};

    ColorType[] colorPlayer = new ColorType[4]
        { ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow};

    public void SetHumanState(int num) { ChangeStatePlayer(PlayerType.Human, num); }
    public void SetAIState(int num) { ChangeStatePlayer(PlayerType.AI, num); }
    public void SetNoneState(int num) { ChangeStatePlayer(PlayerType.None, num); }
    public void SetOnlineState(int num) { ChangeStatePlayer(PlayerType.Online, num); }

    void ChangeStatePlayer(PlayerType type, int numberPlayer) 
    {
        typePlayer[--numberPlayer] = type;
    }

    bool CheckCountPlayer() 
    {
        int countPlayers = 0;
        
        for (int i = 0; i < typePlayer.Length; i++)
            if (typePlayer[i] != PlayerType.None) countPlayers++;

        if (countPlayers < 2) return false;
        return true;
    }

    public void IncreaseSize() { SizeMap++; }
    public void DecreaseSize() { SizeMap--; }

    public void StartCustomGame()
    {
        if (CheckCountPlayer() == false) return;

        mainController.SelectCustomGameMode(sizeMap, typePlayer, cornerPlayer, colorPlayer);
        CustomUI.SetActive(false);
        StartGame();
    }

    int SizeMap 
    {
        get { return sizeMap; }
        set 
        {
            sizeMap = value;
            if (sizeMap <= 8) sizeMap = 8;
            else if (sizeMap >= 13) sizeMap = 13;
            sizeText.text = sizeMap + "x" + sizeMap;
        }
    }


   

}

