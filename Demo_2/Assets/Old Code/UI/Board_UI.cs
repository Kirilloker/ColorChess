using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board_UI : MonoBehaviour
{
    /*
    // Словарь очков - 1 значение номер игрока
    //               - 2 значение кол-во очков
    Dictionary<int, int> score = new Dictionary<int, int>();
    // Список никнеймов
    List<string> nicknames = new List<string>();

    // Текст на настоящих углах
    public Text[] corner = new Text[4];

    // Ссылки на текст углов где стоит игрок
    Text[] corner_player = new Text[4];

    // Текст количество пустых клеток
    public Text score_1;
    public Text score_2;

    List<Player_description> players_discription;

    public void set_amount_player()
    {
        players_discription = GameObject.FindWithTag("play_session").GetComponent<Play_session>().get_players_discription();
        normalize_corner();
    }

    public void set_score(int number_player, int _score)
    {
        // 0 игрок - это количество пустых клеток

        // Если такого игрока еще не было, добавляем его в словарь
        if (score.ContainsKey(number_player) == false) score.Add(number_player, _score);
        else score[number_player] = _score;
    }

    public void update_UI()
    {
        // Кол-во пустых клеток
        score_1.text = score[0].ToString();
        score_2.text = score[0].ToString();

        for (int i = 0; i < players_discription.Count; i++)
        {
            corner_player[i].text = score[i + 1].ToString();
        }

        // Если это самое стандартное расположение, то сделать станрадный вид
        if (players_discription.Count == 2 && players_discription[0].corner == Corner.Down_left && players_discription[1].corner == Corner.Up_right) 
        {
            corner[1].text = score[2].ToString();
            corner[3].text = score[1].ToString();
        }
    }

    void normalize_corner()
    {
        for (int i = 0; i < players_discription.Count; i++)
        {
            corner_player[i] = corner[(int) players_discription[i].corner - 1];
        }
    }
    */
}
