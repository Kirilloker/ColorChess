using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board_UI : MonoBehaviour
{
    /*
    // ������� ����� - 1 �������� ����� ������
    //               - 2 �������� ���-�� �����
    Dictionary<int, int> score = new Dictionary<int, int>();
    // ������ ���������
    List<string> nicknames = new List<string>();

    // ����� �� ��������� �����
    public Text[] corner = new Text[4];

    // ������ �� ����� ����� ��� ����� �����
    Text[] corner_player = new Text[4];

    // ����� ���������� ������ ������
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
        // 0 ����� - ��� ���������� ������ ������

        // ���� ������ ������ ��� �� ����, ��������� ��� � �������
        if (score.ContainsKey(number_player) == false) score.Add(number_player, _score);
        else score[number_player] = _score;
    }

    public void update_UI()
    {
        // ���-�� ������ ������
        score_1.text = score[0].ToString();
        score_2.text = score[0].ToString();

        for (int i = 0; i < players_discription.Count; i++)
        {
            corner_player[i].text = score[i + 1].ToString();
        }

        // ���� ��� ����� ����������� ������������, �� ������� ���������� ���
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
