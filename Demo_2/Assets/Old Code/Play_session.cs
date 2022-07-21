using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Play_session : MonoBehaviour
{
    /*
    List<Player_description> players_discription;

    List<Step_description> step_description = new List<Step_description>();

    // ����� ������ ������� ������ �����
    [SerializeField] int number_player_step;
    // ���������� �������
    [SerializeField] int amount_player;

    // �����
    GameObject board;
    Board _board;

    // ������ �������
    List<Player> players = new List<Player>();

    // ����
    new Audio_script audio;

    // ��������� �� ����
    bool end_game = false;

    // ������ ��������������� �����
    Dictionary<Figure, int> blocked_figure = new Dictionary<Figure, int>();
    
    // �������� ������
    public Figure pressed_figure = null;
    
    // ������� ������ � ������ �� ������
    GameObject camera;
    CameraManager cameraManager;
    int status_camera = 0;
    public PostProcessVolume volume;

    // ��������� ������
    bool camera_swap;

    // ������� ���������� �����
    public int counter_step = 0;


    // ========================================================================================


    public void start_game_cycle()
    {
        //cameraManager.SwitchCamera(ViewCamera.inGame1);
        
        amount_player = players.Count;

        audio = GameObject.FindWithTag("Audio").GetComponent<Audio_script>();
        _board = GameObject.FindWithTag("Board").GetComponent<Board>();
        _board.set_amount_player();

        number_player_step = 0;

        step();

        //Invoke("print_info_game", 0.5f);
    }

    public void step()
    {
        if (end_game == true) return;

        // ����� ������� ���
        if (one_player_live())
        {
            Debug.Log("������� ������ 1 ����� � �� �������! ����� ����");
            //end_game_cycle();
        }

        //if (Tutorial._tutorual == true && counter_step > 2 && _board.check_finish_cells() == true) 
        //{
        //    Debug.Log("���, � ������ ������ ��� ������, �����, ���� �� ���-�� �������");
        //    GameObject.FindWithTag("Tutorial").GetComponent<Tutorial>().next_tutorial();
        //    return;
        //}

        // ��������� �� ��������
        cheak_eat_figure();

        counter_step++;

        // ���� ��� �� ������ ��� - ����������� ���� ������� � ������� �������� � ������
        if (counter_step > 1)
        {
            cameraManager.Set_Camera_Change_Speed(0f);
            audio.step();
        }

        if (counter_step > step_description.Count) 
        {
            step_description.Add(new Step_description(copy(_board.get_massive_cell()), copy(blocked_figure)));
        }
        else 
        {
            step_description[counter_step] = (new Step_description(copy(_board.get_massive_cell()), copy(blocked_figure)));
        }
        
        StartCoroutine(_step());
    }

    IEnumerator _step()
    {
        audio.void_sound();

        // ��������� ��������� ����� - ������������ ����
        _board.update_board();

        yield return new WaitForSeconds(0f);

        // ���� ���� �� ��������� 
        if (end_game == false)
        {
            // ����������� ����� ������ ������� ������ �����
            if (number_player_step == amount_player) number_player_step = 1;
            else number_player_step++;

            if (Tutorial._tutorual) 
            {
                number_player_step = 1;
            }

            // ���� ��� �� ������ ����
            if (online_number == 0)
            {
                // ���� ��� ������ ������ ������
                for (int i = 0; i < amount_player; i++)
                {
                    if (number_player_step == players_discription[i].player_number)
                    {
                        // ���� ����� ������ ������
                        players[i].step();
                    }
                }

            }
            else
            {
                // ���� ��� ������ ���� � ������ ����� �����
                if (online_number == number_player_step)
                {
                    for (int i = 0; i < amount_player; i++)
                    {
                        if (number_player_step == players_discription[i].player_number)
                        {
                            players[i].step();
                        }
                    }
                }
            }

            // �������������� ������
            swap_camera();
        }
        else
        {
            // ����� ����
        }
    }
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

    private bool one_player_live()
    {
        // ������� ��������� ���, ������� �� ������ ���� ����� �� �����
        int live_player = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].get_count_figure() != 0) live_player++;
        }

        if (live_player <= 1) return true;
        else return false;
    }
    public void end_game_cycle()
    {
        cameraManager.Set_Camera_Change_Speed(2f);
        Debug.Log("����� ����");
        end_game = true;
    }

    public void play_session_destroy()
    {
        _board.Board_destroy();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].Player_destroy();
        }

        cameraManager.Set_Camera_Change_Speed(2f);

        // ������� ������ ������
        GameObject.FindWithTag("Materials").GetComponent<Get_materials>().clear();

        GameObject.Destroy(gameObject);
    }
    private void cheak_eat_figure()
    {
        // �������� �� ����������� ������

        // ���������� �� ���� �������
        for (int i = 0; i < players.Count; i++)
        {
            List<Figure> figure_player = players[i].get_figures();

            // ���������� �� ���� �������
            for (int j = 0; j < figure_player.Count; j++)
            {
                // ���� ������ ������, �� ���������� �������� ��� ������ 
                if (figure_player[j].get_live() == false) { continue; }

#region ������ ����� ������ ����
                //figure_player[j].GetComponent<Figure>().check_enemy_block()
#endregion

                // ���� ������ ������ ������� 
                if (figure_player[j].check_enemy_block() == true)
                {
                    // ���� �� ����� ���� �� ���� �������������
                    if (blocked_figure.ContainsKey(figure_player[j]) == false)
                    {
                        // ��������� � ������ ��������������� �����
                        blocked_figure.Add(figure_player[j], 0);

                        // ������ ������ ������
                        cell_description _status = new cell_description(true);
                        figure_player[j].change_status_cell_under(_status);
                    }

                    // ���������� 1 � ���������� ����� � ��������������� ���������
                    blocked_figure[figure_player[j]] += 1;

                    // ���� ������ �������������� ��� (���-�� ������� + 1) ����� 
                    if (blocked_figure[figure_player[j]] == players.Count + 1)
                    {
                        // ������� � �� ������ ��������������� �����
                        blocked_figure.Remove(figure_player[j]);
                        // ������� ���� ������
                        figure_player[j].kill_figure();
                    }
                }
                // ���� ������ ������������
                else if (blocked_figure.ContainsKey(figure_player[j]) == true)
                {
                    Debug.Log("��� �����-�� ������ ������������!");
                    // ������� � �� ������ ��������������� ����� � ������ ����� ������
                    cell_description _status = new cell_description(false);
                    figure_player[j].change_status_cell_under(_status);
                    blocked_figure.Remove(figure_player[j]);
                }
            }

        }
    }
    public void create_board(int width, int lenght)
    {
        Transform parent = GameObject.FindWithTag("Arena").transform;
        GameObject prefabs_board = GameObject.FindWithTag("Models").GetComponent<Get_models>().get_board();
        this.board = Instantiate(prefabs_board, transform.localPosition, Quaternion.AngleAxis(270, Vector3.up), parent);

        board.GetComponent<Board>().Board_create(width, lenght);
    }
    public void create_players()
    {
        // ������� �������
        players_discription = cheack_colission(players_discription);

        for (int i = 0; i < players_discription.Count; i++)
        {
            GameObject player = new GameObject();
            player.name = "Player" + (i + 1);
            player.transform.SetParent(transform);

            switch (players_discription[i].player_type)
            {
                case "Network":
                    player.AddComponent<Network_player>();
                    break;

                case "HotSeat":
                    player.AddComponent<Player>();
                    break;

                case "AI":
                    player.AddComponent<AI_player>();
                    break;

                default:
                    Debug.LogWarning("������: �� ������ ��� ������: " + players_discription[i].player_type);
                    player.AddComponent<AI_player>();
                    break;
            }

            player.GetComponent<Player>().Player_create(players_discription[i]);

            players.Add(player.GetComponent<Player>());
        }

        amount_player = players.Count;
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

    public void print_info_game() 
    {
        Debug.Log("������ �����: " + _board.get_lenght() + "x" + _board.get_width());

        for (int i = 1; i <= players_discription.Count; i++)
        {
            for (int j = 0; j < players_discription.Count; j++)
            {
                if (players_discription[j].player_number == i)
                {
                    Debug.Log("�����: " + players_discription[j].nickname + " ("  + players_discription[j].player_type+ ")");
                    Debug.Log("����� " + i);
                    Debug.Log("����������� ������������: " + get_string_corner(players_discription[j].corner));
                    Debug.Log("����: " + get_string_color(players_discription[j].index_material));
                }
            }
        }
    }

    public void fake_step(Figure _figure, Cell _cell)
    {
        // ��� ������� ������ �� �������

        if (number_player_step != _figure.number_player) return;

        List<Vector3> Available = _figure.get_available();

        if (Available.Contains(_cell.get_localpos()) == true)
        {
            _figure.move_figure(_cell.get_localpos());
        }
    }
    private void swap_camera() { if (camera_swap) StartCoroutine(moveCamera()); }
    public void change_color_board() { board.GetComponent<Board>().change_material_board(players_discription); }
    public void set_camera(GameObject _camera, bool _camera_swap)
    {
        camera = _camera;
        camera_swap = _camera_swap;
        cameraManager = camera.GetComponent<CameraManager>();
    }
    public void set_score_player(int number_player, int _score)
    {
        players[number_player - 1].set_score(_score);
    }
    public void set_camera_swap(bool _switch) { camera_swap = _switch; }
    public int get_amount_player() { return players.Count; }
    public int get_number_player_step() { return number_player_step; }
    public void set_players_discription(List<Player_description> _players_discription)
    {
        players_discription = _players_discription;
    }
    public List<Player_description> get_players_discription()
    {
        return players_discription;
    }

    public Step_description get_last_step_description() 
    {
        return step_description[step_description.Count - 1];
    }

    public string get_string_corner(Corner corner) 
    {
        switch (corner)
        {
            case Corner.Down_left:
                return "�����-�����";
                
            case Corner.Down_right:
                return "������-�����";
                
            case Corner.Up_right:
                return "������-������";
                
            case Corner.Up_left:
                return "������-�����";
                
            default:
                Debug.LogWarning("�� ��������� ����!");
                return "???";
        }
    }

    public string get_string_color(Color color) 
    {
        switch (color)
        {
            case Color.Default:
                return "���������";

            case Color.Red:
                return "�������";

            case Color.Blue:
                return "�����";

            case Color.Yellow:
                return "������";

            case Color.Purple:
                return "����������";

            default:
                Debug.LogWarning("�� ��������� ����� ����!");
                return "???";
        }
    }

    public Color get_number_color_player(int _number_player)
    {
        for (int i = 0; i < players_discription.Count; i++)
        {
            if (players_discription[i].player_number == _number_player)
            {
                return players_discription[i].index_material;
            }

        }

        return Color.Default;
    }

    public void find_all_cell_down() 
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].find_all_cell_down();
        }
    }

    public void cancel_step(int count_cansel)
    {
        StartCoroutine(_cancel_step(count_cansel));
    }


    public virtual IEnumerator _cancel_step(int count_cansel)
    {
        // ������ ���� 
        int now_step = counter_step - 1 + count_cansel;

        // ���� ��� �� ���� ������� ����� �� ����������� ���������� �������
        if (counter_step <= 1) yield break;
        if (now_step > counter_step) yield break;

        // ���� ���� ���������� �����-���� ������, �������� �
        if (pressed_figure != null) pressed_figure.figure_down();

        // ������� ��������������� ����� ���������� �� �������
        blocked_figure = copy(step_description[now_step].blocked_figure);

        // �������� ���������� ��������� �����
        counter_step--;

        // ���������� �������� ��������� � �������
        for (int i = 0; i < _board.get_lenght(); i++)
        {
            for (int j = 0; j < _board.get_width(); j++)
            {
                _board.get_massive_cell()[i, j].set_status(step_description[now_step].massive_cell[i, j].cell);
                _board.get_massive_cell()[i, j].set_who_in_cell(step_description[now_step].massive_cell[i, j].who_in_cell);

                Figure who_in_cell = step_description[now_step].massive_cell[i, j].who_in_cell;

                if (who_in_cell != null) who_in_cell.GetComponent<BoxCollider>().enabled = false;

                if (who_in_cell != null && who_in_cell.transform.localPosition != new Vector3(i, 0f, j))
                { 
                    yield return StartCoroutine(who_in_cell.animate_move_figure(new Vector3(i, 0f, j), false));
                    
                    // ���� ������ ������ �� ���������� ����, �� ��� ����������� � �� ����, �������� �
                    if (who_in_cell.get_count_die() == counter_step)
                    {
                        who_in_cell.set_live(true);
                    }

                }
            }
        }



        // ������� ��������� ������� � ������� ���������� �����
        step_description.RemoveAt(step_description.Count - 1);

        // ����������� ����
        audio.reverse_step();

        // ���������� ����� ����, ��� ������ ����� 
        if (number_player_step == 1) number_player_step = amount_player;
        else number_player_step--;

        // ��������� ���
        bool AI_step = false;

        for (int i = 0; i < amount_player; i++)
        {
            if (number_player_step == players_discription[i].player_number)
            {
                if (players[i].player_type == "AI")
                {
                    AI_step = true;
                    break;
                }
            }
        }


        if (AI_step == true) cancel_step(-1);
        else
        {
            // ��� ��� ����������, ������ ��� � ������ ������� _step ���� ���������� 
            // � ��� �� �������������� ������
            if (number_player_step == 1) number_player_step = amount_player;
            else number_player_step--;

            StartCoroutine(_step());
        }
            
    }

    public void surrender() 
    {
        end_game = true;


        GameObject.FindWithTag("Materials").GetComponent<Get_materials>().clear();

        cameraManager.Set_Camera_Change_Speed(2f);

        //cameraManager.SwitchCamera(ViewCamera.noteMenu);
    }

    public Dictionary<Figure, int> copy(Dictionary<Figure, int> _blocked_figure)
    {
        // ����������� ������� ��������������� �����
        Dictionary<Figure, int> block = new Dictionary<Figure, int>();

        foreach (var item in _blocked_figure)
        {
            block[item.Key] = item.Value;
        }

        return block;
    }

    public �ostil_cell[,] copy(Cell[,] _massive_cell)
    {
        // ����������� ������� ������
        �ostil_cell[,] massive_cell = new �ostil_cell[_board.get_lenght(), _board.get_width()];

        for (int i = 0; i < _board.get_lenght(); i++)
        {
            for (int j = 0; j < _board.get_width(); j++)
            {
                massive_cell[i, j].who_in_cell = _massive_cell[i, j].get_who_in_cell();
                massive_cell[i, j].cell = _massive_cell[i, j].get_status();
            }
        }

        return massive_cell;
    }

    public void set_status_game(Step_description _step_description) 
    {
        for (int i = 0; i < _board.get_lenght(); i++)
        {
            for (int j = 0; j < _board.get_width(); j++)
            {
                _board.get_massive_cell()[i, j].set_status(_step_description.massive_cell[i, j].cell);
                _board.get_massive_cell()[i, j].set_who_in_cell(_step_description.massive_cell[i, j].who_in_cell);
            }
        }
    }


    public int count_die_figure() 
    {
        int count_die = 0;

        for (int i = 0; i < players.Count; i++)
        {
            count_die += players[i].get_count_figure() - players[i].get_count_live_figure();
        }

        return count_die;
    }

    // ONLINE ===========================================================================
    int online_number = 0;
    public void set_online_number(int number) { online_number = number; }
    public int get_online_number() { return online_number; }
    */
}


