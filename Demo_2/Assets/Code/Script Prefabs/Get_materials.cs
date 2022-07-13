using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_materials : MonoBehaviour
{
    public List<Dictionary<string, Material>> player_material;   // Список из наборов цвета каждого игрока

    public Material eating_material;
    public Material base_cell;
    public Material finished;
    public Material material_board;
    public Material material_players_base;

    public Material base_figure_default;

    public List<Material> dark_colors;
    public List<Material> colors;

    Play_session play_Session;

    private void Start()
    {
        player_material = new List<Dictionary<string, Material>>();
        //play_Session = GameObject.FindWithTag("play_session").GetComponent<Play_session>();
    }

    public Material get_color_cell(cell_description status)
    {
        // Выдаёт цвет для ячейки

        if (status.status == "void") return base_cell;

        if (status.status == "finished") return finished;

        if (status.eating == true) return eating_material;

        Color number_player = play_Session.get_number_color_player(status.number_player);
        return (get_color(number_player, status.status));

    }
    public Material get_color_board(int number_player, string key = "paint")
    {
        // Выдаёт цвет игрока для доски
        // Номер игрок 0 - это цвет доски, -1 - это цвет того места, где находится текст
        if (number_player == 0) { return material_board; }
        else if (number_player == -1) { return material_players_base; }
        return (player_material[number_player - 1])[key];
    }

    public void add_players_material(Color _index_material)
    {
        Dictionary<string, Material> _player_material = new Dictionary<string, Material>();

        _player_material["paint"] = get_color(_index_material, "paint");
        _player_material["dark"] = get_color(_index_material, "dark");

        player_material.Add(_player_material);
    }

    public Material get_base_figure(string name_figure = "Pawn")
    {
        return base_figure_default;
    }

    public Material get_color(Color number_color, string type_color = "paint")
    {
        if (type_color == "paint") return colors[(int)number_color];
        else if (type_color == "dark") return dark_colors[(int)number_color];
        else
        {
            Debug.LogWarning("Ошибка: Неизвестный тип цвета: " + type_color);
            return colors[(int)number_color];
        }
    }

    public void clear()
    {
        player_material.Clear();
    }


    public void set_play_session(Play_session _play_Session) 
    {
        play_Session = _play_Session;
    }
}
