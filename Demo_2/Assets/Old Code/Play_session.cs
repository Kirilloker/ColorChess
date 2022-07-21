using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Play_session : MonoBehaviour
{
    /*
    List<Player_description> players_discription;

    List<Step_description> step_description = new List<Step_description>();

    // Номер игрока который сейчас ходит
    [SerializeField] int number_player_step;
    // Количество игроков
    [SerializeField] int amount_player;

    // Доска
    GameObject board;
    Board _board;

    // Список игроков
    List<Player> players = new List<Player>();

    // Звук
    new Audio_script audio;

    // Закончена ли игра
    bool end_game = false;

    // Список заблокированных фигур
    Dictionary<Figure, int> blocked_figure = new Dictionary<Figure, int>();
    
    // Поднятая фигура
    public Figure pressed_figure = null;
    
    // Позиция камеры и скрипт на камере
    GameObject camera;
    CameraManager cameraManager;
    int status_camera = 0;
    public PostProcessVolume volume;

    // Переворот камеры
    bool camera_swap;

    // Подсчет количества ходов
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

        // Когда сделали ход
        if (one_player_live())
        {
            Debug.Log("Остался только 1 игрок и он молодец! Конец игры");
            //end_game_cycle();
        }

        //if (Tutorial._tutorual == true && counter_step > 2 && _board.check_finish_cells() == true) 
        //{
        //    Debug.Log("Опа, а клетки финиша все заняты, круто, надо бы что-то сделать");
        //    GameObject.FindWithTag("Tutorial").GetComponent<Tutorial>().next_tutorial();
        //    return;
        //}

        // Проверяем на съедания
        cheak_eat_figure();

        counter_step++;

        // Если это не первый ход - проигрываем звук нажатия и убираем задержку у камеры
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

        // Обновляем состояние доски - подсчитываем очки
        _board.update_board();

        yield return new WaitForSeconds(0f);

        // Если игра не закончена 
        if (end_game == false)
        {
            // Переключаем номер игрока который сейчас ходит
            if (number_player_step == amount_player) number_player_step = 1;
            else number_player_step++;

            if (Tutorial._tutorual) 
            {
                number_player_step = 1;
            }

            // Если это не онлайн игра
            if (online_number == 0)
            {
                // Ищем кто сейчас должен ходить
                for (int i = 0; i < amount_player; i++)
                {
                    if (number_player_step == players_discription[i].player_number)
                    {
                        // Даем права ходить игроку
                        players[i].step();
                    }
                }

            }
            else
            {
                // Если это онлайн игра и сейчас ходит игрок
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

            // Переворачиваем камеру
            swap_camera();
        }
        else
        {
            // Конец игры
        }
    }
    IEnumerator moveCamera()
    {
        // Переворот камеры в зависимости от угла

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
            Debug.LogWarning("Предупреждение: что-то не так с поворотом камеры");
        }

    }

    private bool one_player_live()
    {
        // Функция возращяет бул, остался ли только один игрок на сцене
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
        Debug.Log("Конец игры");
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

        // Очищаем список цветов
        GameObject.FindWithTag("Materials").GetComponent<Get_materials>().clear();

        GameObject.Destroy(gameObject);
    }
    private void cheak_eat_figure()
    {
        // Проверка на съедаемость фигуры

        // Проходимся по всем игрокам
        for (int i = 0; i < players.Count; i++)
        {
            List<Figure> figure_player = players[i].get_figures();

            // Проходимся по всем фигурам
            for (int j = 0; j < figure_player.Count; j++)
            {
                // Если фигура мертва, то пропустить проверку это фигуры 
                if (figure_player[j].get_live() == false) { continue; }

#region Ошибка здесь должна быть
                //figure_player[j].GetComponent<Figure>().check_enemy_block()
#endregion

                // Если фигура сейчас закрыта 
                if (figure_player[j].check_enemy_block() == true)
                {
                    // Если до этого хода не была заблокирована
                    if (blocked_figure.ContainsKey(figure_player[j]) == false)
                    {
                        // Добавялем её массив заблокированных фигур
                        blocked_figure.Add(figure_player[j], 0);

                        // Меняем статус клетки
                        cell_description _status = new cell_description(true);
                        figure_player[j].change_status_cell_under(_status);
                    }

                    // Прибавляем 1 к количеству ходов в заблокированном состояние
                    blocked_figure[figure_player[j]] += 1;

                    // Если фигура заблокированна уже (кол-во игроков + 1) ходов 
                    if (blocked_figure[figure_player[j]] == players.Count + 1)
                    {
                        // Удаляем её из списка заблокированных фигур
                        blocked_figure.Remove(figure_player[j]);
                        // Удаляем саму фигуру
                        figure_player[j].kill_figure();
                    }
                }
                // Если фигура освободилась
                else if (blocked_figure.ContainsKey(figure_player[j]) == true)
                {
                    Debug.Log("Ого какая-то фигура освободилась!");
                    // Удаляем её из списка заблокированных фигур и меняем стаус клетки
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
        // Создаем игроков
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
                    Debug.LogWarning("Ошибка: не найден тип игрока: " + players_discription[i].player_type);
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
        // Есть тип данных номер игрока и номер его угла
        // Эта функция должна проверить есть ли колизии в массиве из этих данных
        // То есть есть ли несколько игроков у которых одинаковый номер игрока
        // Или одинаковый угол занят
        // Или если угла вообще нет, то добавить свободный угол

        List<Player_description> normal_players_disctription = new List<Player_description>();

        // Занятые углы
        List<Corner> busy_corner = new List<Corner>();

        // Занятые номера
        List<int> busy_number = new List<int>();

        // Занятые цвета
        List<Color> busy_color = new List<Color>();

        // Зануляем все элементы
        for (int i = 0; i < _players_discription.Count; i++)
        {
            busy_corner.Add(Corner.Default);
            busy_number.Add(-1);
            busy_color.Add(Color.Default);
        }

        for (int i = 0; i < _players_discription.Count; i++)
        {
            // Если в массиве еще не было такого номера игрока, то коллизии пока нет,
            // значит его можно добавить в массив
            if (!busy_number.Contains(_players_discription[i].player_number)
                && _players_discription[i].player_number <= _players_discription.Count
                && _players_discription[i].player_number > 0)
            {
                busy_number[i] = _players_discription[i].player_number;
            }
            else
            {
                // Если такой номер игрока уже занят
                // То пока ничего не делаем, оставляем номер 0
            }

            // Тоже самое для угла
            if (!busy_corner.Contains(_players_discription[i].corner)
                && (int) _players_discription[i].corner <= 4
                && _players_discription[i].corner > 0)
            {
                busy_corner[i] = _players_discription[i].corner;
            }

            // Тоже самое для цвета
            if (!busy_color.Contains(_players_discription[i].index_material)
                && (int) _players_discription[i].index_material < System.Enum.GetValues(typeof(Color)).Length
                && _players_discription[i].index_material > 0)
            {
                busy_color[i] = _players_discription[i].index_material;
            }
        }
        

        for (int i = 0; i < _players_discription.Count; i++)
        {
            // Если есть игрок, у которого нет номера 
            if (busy_number[i] <= 0)
            {
                // Запускаем цикл и ищем ему свободное место
                for (int j = 1; j <= busy_number.Count; j++)
                {
                    if (!busy_number.Contains(j))
                    {
                        busy_number[i] = j;
                    }
                }
            }

            // Если есть игрок, у которого нет угла
            if (busy_corner[i] <= 0)
            {
                // Запускаем цикл и ищем ему свободное место
                for (Corner j = Corner.Down_left; (int) j <= busy_corner.Count; j++)
                {
                    if (!busy_corner.Contains(j))
                    {
                        busy_corner[i] = j;
                    }
                }
            }

            // Если есть игрок, у которого нет цвета
            if (busy_color[i] <= 0)
            {
                // Запускаем цикл и ищем ему свободный цвет
                for (Color j = Color.Red; (int) j < 5; j++)
                {
                    if (!busy_color.Contains(j))
                    {
                        busy_color[i] = j;
                    }
                }
            }
        }

        // Если нет первого игрока 
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
        Debug.Log("Размер доски: " + _board.get_lenght() + "x" + _board.get_width());

        for (int i = 1; i <= players_discription.Count; i++)
        {
            for (int j = 0; j < players_discription.Count; j++)
            {
                if (players_discription[j].player_number == i)
                {
                    Debug.Log("Игрок: " + players_discription[j].nickname + " ("  + players_discription[j].player_type+ ")");
                    Debug.Log("Ходит " + i);
                    Debug.Log("Изначальное расположение: " + get_string_corner(players_discription[j].corner));
                    Debug.Log("Цвет: " + get_string_color(players_discription[j].index_material));
                }
            }
        }
    }

    public void fake_step(Figure _figure, Cell _cell)
    {
        // Ход который делает не человек

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
                return "Слева-снизу";
                
            case Corner.Down_right:
                return "Справа-снизу";
                
            case Corner.Up_right:
                return "Справа-сверху";
                
            case Corner.Up_left:
                return "Сверху-слева";
                
            default:
                Debug.LogWarning("Не известный угол!");
                return "???";
        }
    }

    public string get_string_color(Color color) 
    {
        switch (color)
        {
            case Color.Default:
                return "Дефолтный";

            case Color.Red:
                return "Красный";

            case Color.Blue:
                return "Синий";

            case Color.Yellow:
                return "Желтый";

            case Color.Purple:
                return "Фиолетовый";

            default:
                Debug.LogWarning("Не обработан такой цвет!");
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
        // Отмена хода 
        int now_step = counter_step - 1 + count_cansel;

        // Если еще не было сделано ходов то заканчиваем выполнение функции
        if (counter_step <= 1) yield break;
        if (now_step > counter_step) yield break;

        // Если была приподнята какая-либо фигура, опускаем её
        if (pressed_figure != null) pressed_figure.figure_down();

        // Словарь заблокированных фигур подгружаем из массива
        blocked_figure = copy(step_description[now_step].blocked_figure);

        // Отнимаем количество сделанных ходов
        counter_step--;

        // Заменяются значения статустов в клетках
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
                    
                    // Если фигура умерла на предыдущем ходу, то при возвращение её на поля, оживляем её
                    if (who_in_cell.get_count_die() == counter_step)
                    {
                        who_in_cell.set_live(true);
                    }

                }
            }
        }



        // Удаляем последний элемент в массиве запомненых ходов
        step_description.RemoveAt(step_description.Count - 1);

        // Проигрываем звук
        audio.reverse_step();

        // Отматывает номер того, кто сейчас ходит 
        if (number_player_step == 1) number_player_step = amount_player;
        else number_player_step--;

        // Запускаем ход
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
            // Еще раз отматываем, потому что в начале функции _step идет увелечение 
            // И так мы восстанавливем баланс
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
        // Копирование словоря заблокированных фигур
        Dictionary<Figure, int> block = new Dictionary<Figure, int>();

        foreach (var item in _blocked_figure)
        {
            block[item.Key] = item.Value;
        }

        return block;
    }

    public Сostil_cell[,] copy(Cell[,] _massive_cell)
    {
        // Копирование массива клеток
        Сostil_cell[,] massive_cell = new Сostil_cell[_board.get_lenght(), _board.get_width()];

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


