using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct cell_description
{
    public string status;
    public int number_player;
    public bool eating;

    public cell_description(string _status, int _number_player, bool _eating = false)
    {
        status = _status;
        number_player = _number_player;
        eating = _eating;
    }
    public cell_description(bool _eating = false)
    {
        status = null;
        number_player = int.MaxValue;
        eating = _eating;
    }
}


[System.Serializable]
public struct Player_description
{
    public Dictionary<Vector3, string> figures;
    public Color index_material;
    public string player_type;
    public int player_number;
    public string nickname;
    public Corner corner;

    public Player_description(Dictionary<Vector3, string> _figures, Color _index_material, string _player_type, int _player_number, string _nickname, Corner _corner = Corner.Down_left)
    {
        figures = _figures;
        index_material = _index_material;
        player_type = _player_type;
        player_number = _player_number;
        nickname = _nickname;
        corner = _corner;
    }
}

[System.Serializable]
public struct Step_description
{
    // Структура описания хода
    // Она состоит из Двумерного массива клеток
    // И списка сколько ходов уже заблокирована фигура 

    public Сostil_cell[,] massive_cell;
    public Dictionary<Figure, int> blocked_figure;

    public Step_description(Сostil_cell[,] _massive_cell, Dictionary<Figure, int> _blocked_figure)
    {
        massive_cell = _massive_cell;
        blocked_figure = _blocked_figure;
    }
}

public struct Сostil_cell
{
    // Описание клетки, костыльное потому что уже написано описание
    // но туда не получается запихнуть who_in_cell
    // поэтому вот такая костыльная вещь
    public cell_description cell;
    public Figure who_in_cell;

    public Сostil_cell(cell_description _cell, Figure _who_in_cell)
    {
        cell = _cell;
        who_in_cell = _who_in_cell;
    }
}

public enum Corner 
{
    Default = 0,
    Down_left = 1,
    Down_right = 2,
    Up_right = 3,
    Up_left = 4
}


public enum Color
{
    Default,
    Red,
    Blue,
    Yellow,
    Purple,
}


public class Struct : MonoBehaviour
{
}
