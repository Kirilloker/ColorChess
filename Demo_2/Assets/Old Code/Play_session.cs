using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Play_session : MonoBehaviour
{
    /*
   
    IEnumerator moveCamera()
    {
        // ��������� ������ � ����������� �� ����

        yield return new WaitForSeconds(1f);

        if (end_game == true) yield break;

        Corner _corner = Corner.Default;

        for (int i = 0; i < players_discription.Count; i++)
        {
            if (number_player_step == players_discription[i].player_number)
            {
                _corner = players_discription[i].corner;
            }
        }  

        if (_corner == Corner.Down_left || _corner == Corner.Down_right)
        {
            //cameraManager.SwitchCamera(ViewCamera.inGame1);
        }
        else if (_corner == Corner.Up_right || _corner == Corner.Up_left)
        {
            //cameraManager.SwitchCamera(ViewCamera.inGame2);
        }
        else
        {
            Debug.LogWarning("��������������: ���-�� �� ��� � ��������� ������");
        }

    }



    public List<Player_description> cheack_colission(List<Player_description> _players_discription)
    {
        // ���� ��� ������ ����� ������ � ����� ��� ����
        // ��� ������� ������ ��������� ���� �� ������� � ������� �� ���� ������
        // �� ���� ���� �� ��������� ������� � ������� ���������� ����� ������
        // ��� ���������� ���� �����
        // ��� ���� ���� ������ ���, �� �������� ��������� ����

        List<Player_description> normal_players_disctription = new List<Player_description>();

        // ������� ����
        List<Corner> busy_corner = new List<Corner>();

        // ������� ������
        List<int> busy_number = new List<int>();

        // ������� �����
        List<Color> busy_color = new List<Color>();

        // �������� ��� ��������
        for (int i = 0; i < _players_discription.Count; i++)
        {
            busy_corner.Add(Corner.Default);
            busy_number.Add(-1);
            busy_color.Add(Color.Default);
        }

        for (int i = 0; i < _players_discription.Count; i++)
        {
            // ���� � ������� ��� �� ���� ������ ������ ������, �� �������� ���� ���,
            // ������ ��� ����� �������� � ������
            if (!busy_number.Contains(_players_discription[i].player_number)
                && _players_discription[i].player_number <= _players_discription.Count
                && _players_discription[i].player_number > 0)
            {
                busy_number[i] = _players_discription[i].player_number;
            }
            else
            {
                // ���� ����� ����� ������ ��� �����
                // �� ���� ������ �� ������, ��������� ����� 0
            }

            // ���� ����� ��� ����
            if (!busy_corner.Contains(_players_discription[i].corner)
                && (int) _players_discription[i].corner <= 4
                && _players_discription[i].corner > 0)
            {
                busy_corner[i] = _players_discription[i].corner;
            }

            // ���� ����� ��� �����
            if (!busy_color.Contains(_players_discription[i].index_material)
                && (int) _players_discription[i].index_material < System.Enum.GetValues(typeof(Color)).Length
                && _players_discription[i].index_material > 0)
            {
                busy_color[i] = _players_discription[i].index_material;
            }
        }
        

        for (int i = 0; i < _players_discription.Count; i++)
        {
            // ���� ���� �����, � �������� ��� ������ 
            if (busy_number[i] <= 0)
            {
                // ��������� ���� � ���� ��� ��������� �����
                for (int j = 1; j <= busy_number.Count; j++)
                {
                    if (!busy_number.Contains(j))
                    {
                        busy_number[i] = j;
                    }
                }
            }

            // ���� ���� �����, � �������� ��� ����
            if (busy_corner[i] <= 0)
            {
                // ��������� ���� � ���� ��� ��������� �����
                for (Corner j = Corner.Down_left; (int) j <= busy_corner.Count; j++)
                {
                    if (!busy_corner.Contains(j))
                    {
                        busy_corner[i] = j;
                    }
                }
            }

            // ���� ���� �����, � �������� ��� �����
            if (busy_color[i] <= 0)
            {
                // ��������� ���� � ���� ��� ��������� ����
                for (Color j = Color.Red; (int) j < 5; j++)
                {
                    if (!busy_color.Contains(j))
                    {
                        busy_color[i] = j;
                    }
                }
            }
        }

        // ���� ��� ������� ������ 
        while (!busy_number.Contains(1))
        {
            for (int i = 0; i < busy_number.Count; i++)
            {
                busy_number[i]--;
            }
        }

        for (int i = 0; i < _players_discription.Count; i++)
        {
            Player_description _player = new Player_description(_players_discription[i].figures, busy_color[i], _players_discription[i].player_type, busy_number[i], _players_discription[i].nickname, busy_corner[i]);
            normal_players_disctription.Add(_player);
        }

        return normal_players_disctription;
    }


    */
}


