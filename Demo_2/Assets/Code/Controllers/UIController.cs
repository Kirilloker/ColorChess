using ColorChessModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameController gameController;
    [SerializeField]
    CameraController cameraController;

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

    void ChangeStatePlayer(PlayerType type, int numerPlayer) 
    {
        typePlayer[--numerPlayer] = type;
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

        gameController.SelectCustomGameMode(sizeMap, typePlayer, cornerPlayer, colorPlayer);
        gameController.StartGame();
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

