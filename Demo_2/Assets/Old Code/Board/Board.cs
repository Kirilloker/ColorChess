using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    /*
    #region Поля класса 
    // Длина доски
    [SerializeField] int lenght;

    // Ширина доски
    [SerializeField] int width;

    // Массив клеток
    [SerializeField] Cell[,] massive_cell;

    // Словарь очков (для каждого игрока свой словарь с каждым типом очков)
    [SerializeField] Dictionary<int, Dictionary<string, int>> score;
    
    // Материал доски
    Material[] material_board = new Material[6];
    
    // Скрипт по выводу очков на экран
    Board_UI _Board_UI;
    
    // Текст очков
    public GameObject Board_UI;
    
    // Ссылка на плей сессию
    Play_session _play_session;


    #endregion

    public void Board_create(int _lenght, int _width)
    {
        // Создание доски, клетки, UI
        _Board_UI = Board_UI.GetComponent<Board_UI>();

        lenght = _lenght;
        width = _width;
        massive_cell = new Cell[_lenght, _width];

        this.transform.localScale = new Vector3(lenght, lenght, width);
        this.transform.localPosition = new Vector3((width - 1f) / 2f, -0.1f, (width - 1f) / 2f); // Левая нижняя грань доски будет в 0 0 0 координатах
        this.tag = "Board";
        this.name = "Board";

        Transform parent = GameObject.FindWithTag("Cell").transform;
        GameObject prefabs_cell = GameObject.FindWithTag("Models").GetComponent<Get_models>().get_cell();
        
        // Создаются клетки
        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject cell = Instantiate(prefabs_cell, new Vector3(i, 0f, j), Quaternion.AngleAxis(-90, Vector3.right), parent) as GameObject;
                massive_cell[i, j] = cell.GetComponent<Cell>();
            }
        }

        _play_session = GameObject.FindWithTag("play_session").GetComponent<Play_session>();
    }

    public void Board_destroy()
    {
        // Удаление доски и всего его содержимого

        for (int i = lenght - 1; i >= 0; i--)
        {
            for (int j = width - 1; j >= 0; j--)
            {
                massive_cell[i, j].Cell_destroy();
            }
        }

        GameObject.Destroy(Board_UI);
        GameObject.Destroy(gameObject);
    }

    public void update_board()
    {
        StartCoroutine(_update_board());
    }

    IEnumerator _update_board()
    {
        yield return new WaitForSeconds(0f);

        // Обновляется состояние доски

        // Подсчет очков
        score = new Dictionary<int, Dictionary<string, int>>();

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // Статус клетки которую сейчас проверяем
                cell_description _status_cell = get_cell(i, j).get_status();

                if (_status_cell.status != "void")
                {
                    check_capture(i, j);
                }

                // Если такого игрока еще не встречали, добавляем его в словарь
                if (!score.ContainsKey(_status_cell.number_player))
                {
                    Dictionary<string, int> _score_player = new Dictionary<string, int>();
                    _score_player.Add(_status_cell.status, 1);

                    score.Add(_status_cell.number_player, _score_player);
                }
                // Если такого игрока уже встречали, но такой статус у него еще не встречали,
                // то добавить его в подсловарь
                else if (!score[_status_cell.number_player].ContainsKey(_status_cell.status))
                {
                    (score[_status_cell.number_player]).Add(_status_cell.status, 1);
                }
                else
                {
                    (score[_status_cell.number_player])[_status_cell.status] += 1;
                }
            }
        }


        foreach (var item in score)
        {
            int score_player = 0;

            foreach (var item1 in item.Value)
            {
                if (item1.Key == "paint") { score_player += 1 * item1.Value; }
                else if (item1.Key == "dark") { score_player += 1 * item1.Value; }
                else { score_player += item1.Value; }
            }

            if (item.Key != 0 && item.Key != 10)
            {
                _play_session.set_score_player(item.Key, score_player);
            }

            _Board_UI.set_score(item.Key, score_player);
        }

        // Если больше нет пустых клеток, то конец игры
        if (score.ContainsKey(0) == false)
        {
            _Board_UI.set_score(0, 0);
            _play_session.end_game_cycle();
        }

        _Board_UI.update_UI();

    }

    public void check_capture(int x_coor, int z_coor)
    {
        // Проверяет есть ли клетка 3 на 3

        // Статус главной клетки
        cell_description _status_cell_main = get_cell(x_coor, z_coor).get_status();

        // Булевая переменная, которая говорит была ли перекращена НОВАЯ клетка в черный цвет
        // Нужна для проигрыша звука 
        bool new_dark = false;

        for (int i = x_coor - 1; i <= x_coor + 1; i++)
        {
            if (i < 0 || i >= lenght) { return; }

            for (int j = z_coor - 1; j <= z_coor + 1; j++)
            {
                if (j < 0 || j >= width) { return; }

                cell_description _status_cell = get_cell(i, j).get_status();

                if (_status_cell_main.number_player != _status_cell.number_player) { return; }
            }
        }

        // Если код дошел до этого момента, значит главная клетка это центр 3х3 клеток с одинаковым номером игрока 

        for (int i = x_coor - 1; i <= x_coor + 1; i++)
        {
            for (int j = z_coor - 1; j <= z_coor + 1; j++)
            {
                cell_description _status_cell = get_cell(i, j).get_status();

                if (_status_cell.status != "dark" && _status_cell.status != "finished") { new_dark = true; }

                _status_cell.status = "dark";

                get_cell(i, j).set_status(_status_cell);
            }
        }

        //Моргнуть одним захваченным полем в туториале 3х3 если захватывается клетка 
        if (new_dark && Tutorial._tutorual)
        {
                StartCoroutine(capture_indicaator(x_coor, z_coor));
        }

         if (new_dark) { GameObject.FindWithTag("Audio").GetComponent<Audio_script>().dark_capture(); }
    }
    IEnumerator capture_indicaator(int x_coor, int z_coor)
    {

        yield return new WaitForSeconds(0.3f);

        for (int i = x_coor - 1; i <= x_coor + 1; i++)
        {
            for (int j = z_coor - 1; j <= z_coor + 1; j++)
            {
                cell_description _status_cell = get_cell(i, j).get_status();

                _status_cell.status = "paint";

                get_cell(i, j).set_status(_status_cell);
            }
        }

        //Wait for 2 seconds
        yield return new WaitForSeconds(0.3f);

        for (int i = x_coor - 1; i <= x_coor + 1; i++)
        {
            for (int j = z_coor - 1; j <= z_coor + 1; j++)
            {
                cell_description _status_cell = get_cell(i, j).get_status();

                _status_cell.status = "dark";

                get_cell(i, j).set_status(_status_cell);
            }
        }
    }

    public void OFF_prompt()
    {
        // Выключает все подсказки
        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < width; j++)
            {
                massive_cell[i, j].hide_promt();
            }
        }
    }

    public void ON_prompt(List<Vector3> massive_prompt)
    {
        // Включает подсказки, координаты которых переданы в массиве
        for (int i = 0; i < massive_prompt.Count; i++)
        {
            get_cell(massive_prompt[i].x, massive_prompt[i].z).show_promt();
        }
    }

    public void change_material_board(List<Player_description> player_Descriptions)
    {
        Get_materials _get_material = GameObject.FindWithTag("Materials").GetComponent<Get_materials>();

        // Цвет доски
        material_board[0] = _get_material.get_color_board(0);  
        // Цвет плоскости очков
        material_board[5] = _get_material.get_color_board(-1); 

        // Изначально все стороны подкрашены в цвет 1 игрока
        for (int i = 1; i <= 4; i++)
        {
            material_board[i] = _get_material.get_color_board(0);
        }

        // Перекрашиваем углы в цвет игрока в зависимости от его угла 
        for (int i = 0; i < player_Descriptions.Count; i++)
        {
            material_board[(int) player_Descriptions[i].corner] = _get_material.get_color_board(player_Descriptions[i].player_number);
        }

        // Если это самое стандартное расположение, то сделать станрадный вид
        if (player_Descriptions.Count == 2 && player_Descriptions[0].corner == Corner.Down_left && player_Descriptions[1].corner == Corner.Up_right)
        {
            material_board[2] = _get_material.get_color_board(2);
            material_board[4] = _get_material.get_color_board(1);
        }

        this.GetComponentInChildren<MeshRenderer>().materials = material_board;
    }

    public Vector3 convert_corner(Vector3 convert_position, Corner corner = Corner.Down_left) // corner - угол
    {
        // Возвращяет конвертированные координаты, в зависимости от угла расположения на доске
        // 1 - лево-нижний угол
        // 2 - право-нижний угол
        // 3 - право-верхний угол
        // 4 - лево-верхний угол

        switch (corner)
        {
            case Corner.Down_left:
                // Ничего не меняем
                break;

            case Corner.Down_right:
                convert_position.x = width - 1 - convert_position.x;
                break;

            case Corner.Up_right:
                convert_position.x = width - 1 - convert_position.x;
                convert_position.z = lenght - 1 - convert_position.z;
                break;

            case Corner.Up_left:
                convert_position.z = lenght - 1 - convert_position.z;
                break;

            default:
                // Ничего не меняем
                break;
        }

        return convert_position;
    }

    public bool check_finish_cells() 
    {
        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Cell cell = massive_cell[i, j];

                if (cell.get_status().status == "finished" &&
                    cell.get_who_in_cell() == null)
                    return false;
            }
        }

        return true;
    }

    public void set_amount_player() { _Board_UI.set_amount_player(); }
    public Cell get_cell(float i, float j) { return massive_cell[(int)i, (int)j]; }
    public Cell get_cell(int i, int j) { return massive_cell[i, j]; }
    public int get_width() { return width; }
    public int get_lenght() { return lenght; }

    public Cell[,]  get_massive_cell() { return massive_cell; }
    */
}
