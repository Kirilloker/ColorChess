using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Play_session_settings pss;
    CameraManager cameraManager;

    int minScaleBoard = 3;
    int maxScaleBoard = 15;

    #region UI элементы
    public Text ScaleBoard;
    public GameObject PrefabsPlayer;
    public Transform Players;

    public Image[] images = new Image[4];
    public Image[] imagesCorner = new Image[4];

    public GameObject StartButton;

    public GameObject EditUI;
    public GameObject GameNote;
    public GameObject InGameUI;

    #endregion

    #region Расположение и набор фигур по умолчанию 
    Dictionary<Vector3, string> DefaultKitFigureAndPosition = new Dictionary<Vector3, string>
    {
        {new Vector3(0f, 0f, 3f), "Pawn"},
        {new Vector3(1f, 0f, 2f), "Pawn"},
        {new Vector3(2f, 0f, 1f), "Pawn"},
        {new Vector3(3f, 0f, 0f), "Pawn"},
        {new Vector3(0f, 0f, 2f), "Castle"},
        {new Vector3(0f, 0f, 1f), "Queen"},
        {new Vector3(1f, 0f, 1f), "Horse"},
        {new Vector3(1f, 0f, 0f), "King"},
        {new Vector3(2f, 0f, 0f), "Bishop"},
    };
    #endregion

    #region Поля которые нужно заполнить
    public List<Player_description> playersDiscription = new List<Player_description>();
    int lenght = 0;
    int width = 0;

    #endregion

    #region Дефолтные настройки для HotSeat и BOT
    public void DefaultSetting2Player() 
    {
        ClearPlayers();
        SetScaleBoard(9);
        SetCameraSwap(true);

        Player_description player1 = new Player_description(DefaultKitFigureAndPosition, Color.Red,  "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        SetPlayersDiscription();

        //UpdateUISettingGame();

        //StartGame();
    }

    public void DefaultSetting4Player()
    {
        ClearPlayers();
        SetScaleBoard(11);
        SetCameraSwap(true);

        Player_description player1 = new Player_description(DefaultKitFigureAndPosition, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "HotSeat", 2, "Player2", Corner.Down_right);
        Player_description player3 = new Player_description(DefaultKitFigureAndPosition, Color.Purple, "HotSeat", 3, "Player3", Corner.Up_right);
        Player_description player4 = new Player_description(DefaultKitFigureAndPosition, Color.Yellow, "HotSeat", 4, "Player4", Corner.Up_left);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);
        playersDiscription.Add(player3);
        playersDiscription.Add(player4);

        SetPlayersDiscription();

        UpdateUISettingGame();

        //StartGame();
    }

    public void DefaultSettingPlayerVSAI()
    {
        ClearPlayers();
        SetScaleBoard(9);
        SetCameraSwap(false);

        Player_description player1 = new Player_description(DefaultKitFigureAndPosition, Color.Red, "HotSeat", 1, "Player", Corner.Down_left);
        Player_description player2 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "AI", 2, "AI", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        SetPlayersDiscription();

        UpdateUISettingGame();

        //StartGame();
    }

    public void DefaultSettingPlayerVS3AI()
    {
        ClearPlayers();
        SetScaleBoard(13);
        SetCameraSwap(false);

        Player_description player1 = new Player_description(DefaultKitFigureAndPosition, Color.Red, "HotSeat", 1, "Player", Corner.Down_left);
        Player_description player2 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "AI", 2, "AI", Corner.Down_left);
        Player_description player3 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "AI", 3, "AI", Corner.Up_right);
        Player_description player4 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "AI", 4, "AI", Corner.Up_left);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);
        playersDiscription.Add(player3);
        playersDiscription.Add(player4);

        SetPlayersDiscription();

        UpdateUISettingGame();

        //StartGame();
    }

    public void DefaultSetting4but2Player()
    {
        ClearPlayers();
        SetScaleBoard(11);
        SetCameraSwap(true);

        Player_description player1 = new Player_description(DefaultKitFigureAndPosition, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(DefaultKitFigureAndPosition, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);
        Player_description player3 = new Player_description(DefaultKitFigureAndPosition, Color.Default, "None", 3, "Player3", Corner.Down_right);
        Player_description player4 = new Player_description(DefaultKitFigureAndPosition, Color.Default, "None", 4, "Player4", Corner.Up_left);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);
        playersDiscription.Add(player3);
        playersDiscription.Add(player4);
    }

    #endregion

    private void Awake()
    {
        //pss = GameObject.FindWithTag("play_session_setting").GetComponent<Play_session_settings>();
        cameraManager =  GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
        DefaultSetting4but2Player();
    }

    public void UpdateUISettingGame() 
    {
        ScaleBoard.text = lenght + "x" + width;

        for (int i = 1; i <= playersDiscription.Count; i++)
        {
            if (playersDiscription[i - 1].player_type != "None") 
            {
                if (playersDiscription[i - 1].index_material == Color.Default) ChangeColorButton(i);

                images[(int)playersDiscription[i - 1].corner - 1].color = GetColor(playersDiscription[i - 1].index_material);
                imagesCorner[(int)playersDiscription[i - 1].corner - 1].color = GetColor(playersDiscription[i - 1].index_material);
            }
            else 
            {
                images[(int)playersDiscription[i - 1].corner - 1].color = GetColor(Color.Default);
                imagesCorner[(int)playersDiscription[i - 1].corner - 1].color = GetColor(Color.Default);
            }
        }
    }

    public void StartGame()
    {
        #region Проверка на количество игроков

        int CounterPlayer = 0;

        for (int i = 0; i < playersDiscription.Count; i++)
        {
            if (playersDiscription[i].player_type != "None") CounterPlayer++;
        }

        if (CounterPlayer <= 1) 
        {
            //StartButton.a
            Debug.Log("Мала! А еще представьте что что-то там подсветилось красным");
            return;
        }

        #endregion

        SetPlayersDiscription();

        if (pss.correct_data() == false)
        {
            Debug.LogWarning("Не все поля заполнены!");
            return;
        }

        EditUI.SetActive(false);
        GameNote.SetActive(true);
        InGameUI.SetActive(true);


        pss.create_play_session();
    }

    public void SetPlayersDiscription() 
    {
        pss.clear_player_description();

        pss.set_players_discription(playersDiscription);
    }

    public void SetScaleBoard(int scale) 
    {
        if (scale < minScaleBoard || scale > maxScaleBoard) { Debug.LogWarning("Не корректный размер"); }

        lenght = scale;
        width = scale;

        pss.set_lenght(scale);
        pss.set_width(scale);

        UpdateUISettingGame();
    }

    public void SetLenght(int _lenght) 
    {
        if (_lenght < minScaleBoard || _lenght > maxScaleBoard) { Debug.LogWarning("Не корректный размер"); }

        lenght = _lenght;

        pss.set_lenght(_lenght);
    }

    public void SetWidth(int _width)
    {
        if (_width < minScaleBoard || _width > maxScaleBoard) { Debug.LogWarning("Не корректный размер"); }

        width = _width;

        pss.set_width(_width);
    }

    public void SetCameraSwap(bool cameraSwap)
    {
        pss.set_camera_swap(cameraSwap);
    }

    public void ClearPlayers() 
    {
        playersDiscription.Clear();
        Debug.Log(pss);
        pss.clear_player_description();
    }

    public void AddNewPlayer() 
    {
        if (playersDiscription.Count >= 4)
        {
            Debug.LogWarning("Превышено максимальное количество игроков");
            return;
        }

        #region Находим угол который не занят
        Corner corner = Corner.Default;

        bool busyCorner;

        for (Corner _corner = Corner.Down_left; _corner <= Corner.Up_left; _corner++)
        {
            busyCorner = false;

            for (int j = 0; j < playersDiscription.Count; j++)
            {
                if (playersDiscription[j].corner == _corner) 
                {
                    busyCorner = true;
                }
            }

            if (busyCorner == false) 
            {
                corner = _corner;
                break;
            }
        }
        #endregion

        #region Находим цвет который не занят
        Color color = Color.Default;

        bool busyColor;

        for (Color _color = Color.Red; _color <= Color.Purple; _color++)
        {
            busyColor = false;

            for (int j = 0; j < playersDiscription.Count; j++)
            {
                if (playersDiscription[j].index_material == _color)
                {
                    busyColor = true;
                }
            }

            if (busyColor == false)
            {
                color = _color;
                break;
            }
        }
        #endregion

        Player_description player = new Player_description(DefaultKitFigureAndPosition, color, "HotSeat", 1, "Player", corner);
        playersDiscription.Add(player);

        SetPlayersDiscription();

        UpdateUISettingGame();
    }

    public void DeletePlayer(int numberPlayer) 
    {
        if ((numberPlayer - 1) >= playersDiscription.Count || numberPlayer <= 0)
        {
            Debug.LogWarning("Нет игрока с таким номером: " + numberPlayer);
            return;
        }

        playersDiscription.RemoveAt(numberPlayer - 1);

        SetPlayersDiscription();

        UpdateUISettingGame();
    }

    public void ChangeScaleBoard(int number) 
    {
        number += lenght;

        if (number < minScaleBoard) number = minScaleBoard;
        if (number > maxScaleBoard) number = maxScaleBoard;

        SetScaleBoard(number);
    }

    public void ChangeHierarchy(int NumberPlayer, int number) 
    {
        #region Проверка
        if (NumberPlayer + number <= 0) 
        {
            Debug.LogWarning("Перебор");
            return;
        }

        if (NumberPlayer + number > playersDiscription.Count) 
        {
            Debug.LogWarning("Перебор");
            return;
        }
        #endregion

        Player_description buf = playersDiscription[NumberPlayer + number - 1];
        playersDiscription[NumberPlayer + number - 1] = playersDiscription[NumberPlayer - 1];
        playersDiscription[NumberPlayer - 1] = buf;

        UpdateUISettingGame();
    }

    public int GetCountPlayer() 
    { 
        return playersDiscription.Count; 
    }
        
    #region ВАЖНАЯ ПОМЕТКА
    // У вас возник вопрос, что за фигня происходит во время изменения параметров у описания игрока?
    // А дело в том, что я пока не знаю как написать ебучий сеттер для одного поля в этой структуру
    // Поэтому приходится делать танцы с бубном
    // Если это читает человек, который знает как это сделать, прошу исправь, это выглядит ужасно!!!

    #endregion

    public void ChangePlayerColor(int numberPlayer, Color color) 
    {
        if ((numberPlayer - 1) >= playersDiscription.Count || numberPlayer <= 0) 
        {
            Debug.LogWarning("Нет игрока с таким номером");
            return;
        }

        Player_description player = new Player_description
            (
                playersDiscription[numberPlayer - 1].figures, 
                color, 
                playersDiscription[numberPlayer - 1].player_type,
                playersDiscription[numberPlayer - 1].player_number, 
                playersDiscription[numberPlayer - 1].nickname, 
                playersDiscription[numberPlayer - 1].corner
            );

        playersDiscription[numberPlayer - 1] = player;

        SetPlayersDiscription();
    }

    public void ChangePlayerType(int numberPlayer, string playerType)
    {
        if ((numberPlayer - 1) >= playersDiscription.Count || numberPlayer <= 0)
        {
            Debug.LogWarning("Нет игрока с таким номером");
            return;
        }

        if ((playerType == "HotSeat" || playerType == "AI" || playerType == "Network" || playerType == "None") == false) 
        {
            Debug.LogWarning("Нет такого типа игрока: " + playerType);
            return;
        }

        Player_description player = new Player_description
            (
                playersDiscription[numberPlayer - 1].figures,
                playersDiscription[numberPlayer - 1].index_material,
                playerType,
                playersDiscription[numberPlayer - 1].player_number,
                playersDiscription[numberPlayer - 1].nickname,
                playersDiscription[numberPlayer - 1].corner
            );

        playersDiscription[numberPlayer - 1] = player;

        SetPlayersDiscription();
    }

    public void ChangePlayerNumber(int numberPlayer, int playerNumber)
    {
        if ((numberPlayer - 1) >= playersDiscription.Count || numberPlayer <= 0)
        {
            Debug.LogWarning("Нет игрока с таким номером");
            return;
        }

        if (playerNumber <= 0 || playerNumber > playersDiscription.Count)
        {
            Debug.LogWarning("Не правильный номер игрока");
            return;
        }

        Player_description player = new Player_description
            (
                playersDiscription[numberPlayer - 1].figures,
                playersDiscription[numberPlayer - 1].index_material,
                playersDiscription[numberPlayer - 1].player_type,
                playerNumber,
                playersDiscription[numberPlayer - 1].nickname,
                playersDiscription[numberPlayer - 1].corner
            );

        playersDiscription[numberPlayer - 1] = player;

        SetPlayersDiscription();
    }

    public void ChangePlayerNickname(int numberPlayer, string nickname)
    {
        if ((numberPlayer - 1) >= playersDiscription.Count || numberPlayer <= 0)
        {
            Debug.LogWarning("Нет игрока с таким номером");
            return;
        }

        if (nickname == "")
        {
            Debug.LogWarning("Пустой никнейм нельзя!");
            return;
        }

        Player_description player = new Player_description
            (
                playersDiscription[numberPlayer - 1].figures,
                playersDiscription[numberPlayer - 1].index_material,
                playersDiscription[numberPlayer - 1].player_type,
                playersDiscription[numberPlayer - 1].player_number,
                nickname,
                playersDiscription[numberPlayer - 1].corner
            );

        playersDiscription[numberPlayer - 1] = player;

        SetPlayersDiscription();
    }

    public void ChangePlayerCorner(int numberPlayer, Corner corner)
    {
        if ((numberPlayer - 1) >= playersDiscription.Count || numberPlayer <= 0)
        {
            Debug.LogWarning("Нет игрока с таким номером");
            return;
        }

        Player_description player = new Player_description
            (
                playersDiscription[numberPlayer - 1].figures,
                playersDiscription[numberPlayer - 1].index_material,
                playersDiscription[numberPlayer - 1].player_type,
                playersDiscription[numberPlayer - 1].player_number,
                playersDiscription[numberPlayer - 1].nickname,
                corner
            );

        playersDiscription[numberPlayer - 1] = player;

        SetPlayersDiscription();
    }

    public List<Player_description> copy(List<Player_description> _players_discription)
    {
        List<Player_description> pl_discript = new List<Player_description>();

        for (int i = 0; i < _players_discription.Count; i++)
        {
            pl_discript.Add(_players_discription[i]);
        }

        return pl_discript;
    }

    public void SelectedBotType(int numberPlayer) 
    {
        ChangePlayerType(numberPlayer, "AI");
        UpdateUISettingGame();
    }

    public void SelectedPlayerType(int numberPlayer)
    {
        ChangePlayerType(numberPlayer, "HotSeat");
        UpdateUISettingGame();
    }

    public void UnSelectedType(int numberPlayer) 
    {
        Debug.Log("test");
        ChangePlayerType(numberPlayer, "None");
        UpdateUISettingGame();
    }

    public UnityEngine.Color GetColor(Color color) 
    {
        switch (color)
        {
            case Color.Default:
                return UnityEngine.Color.gray;
            case Color.Red:
                return UnityEngine.Color.red;
            case Color.Blue:
                return UnityEngine.Color.blue;
            case Color.Yellow:
                return UnityEngine.Color.yellow;
            case Color.Purple:
                return UnityEngine.Color.magenta;
            default:
                return UnityEngine.Color.gray;
        }
    }

    public void ChangeColorButton(int numberPlayer)
    {
        #region Находим цвет который не занят
        Color color = Color.Default;

        bool busyColor;
        bool SecurityInfinityWhile = false;

        for (Color _color = playersDiscription[numberPlayer - 1].index_material; _color <= Color.Purple; _color++)
        {
            busyColor = false;

            for (int j = 0; j < playersDiscription.Count; j++)
            {
                if (playersDiscription[j].index_material == _color &&
                    playersDiscription[j].player_type != "None")
                {
                    busyColor = true;
                }
            }

            if (busyColor == false)
            {
                color = _color;
                break;
            }

            if (_color == Color.Purple)
            { 
                if (SecurityInfinityWhile == true) 
                    return;
                else
                    _color = Color.Default;

                SecurityInfinityWhile = true;
            }
        }
        #endregion

        ChangePlayerColor(numberPlayer, color);

        UpdateUISettingGame();
    }
}
