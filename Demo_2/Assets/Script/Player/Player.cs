using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Ник игрока
    [SerializeField] string nickname;
    // Кол-во очков игрока
    [SerializeField] int score;
    // Угол игрока
    [SerializeField] Corner corner; 

    // Список фигур игрока
    protected List<Figure> figures = new List<Figure>();
    // Номер игрока
    public int player_number;
    // Тип игрока - HotSeat, Networ, AI
    public string player_type;

    public void Player_create(Player_description players_discription) 
    {
        // Создаются фигуры игрока
        nickname      = players_discription.nickname;
        player_number = players_discription.player_number;
        player_type   = players_discription.player_type;
        corner        = players_discription.corner;
        this.name     = "Player" + player_number;

        // Мне не нравится, что так много строчек занимают материалы!!!
        Get_materials get_material = GameObject.FindWithTag("Materials").GetComponent<Get_materials>();
        Material[] material_player = new Material[2];
        material_player[1] = get_material.get_color(players_discription.index_material);
        get_material.add_players_material(players_discription.index_material);

        Quaternion quaternion = Quaternion.identity;

        foreach (var item in players_discription.figures)
        {
            GameObject prefabs_figure = GameObject.FindWithTag("Models").GetComponent<Get_models>().get_figure(item.Value);
            Vector3 position_figure = GameObject.FindWithTag("Board").GetComponent<Board>().convert_corner(item.Key, players_discription.corner);
            material_player[0] = get_material.get_base_figure(prefabs_figure.name);

            // Переворачиваем Коня 
            if ((players_discription.corner == Corner.Up_left || players_discription.corner == Corner.Up_right) && prefabs_figure.name == "Horse") 
            {
                quaternion = Quaternion.AngleAxis(180, Vector3.up);
            }

            if (prefabs_figure.name == "King") 
            {
                quaternion = Quaternion.AngleAxis(90, Vector3.up);
            }

            GameObject new_figure = Instantiate(prefabs_figure, position_figure, quaternion, transform);
            new_figure.GetComponent<MeshRenderer>().materials = material_player;

            figures.Add(new_figure.GetComponent<Figure>());
        }

        off_box_cool_figures();
    }

    public void Player_destroy() 
    {
        // Удаление Игрока
        for (int i = figures.Count - 1; i >= 0; i--) 
        {
            figures[i].delete_figure();
        }

        GameObject.Destroy(gameObject);
    }

    public void find_all_cell_down() 
    {
        for (int i = 0; i < figures.Count; i++)
        {
            figures[i].find_cell_under();
        }
    }

    public virtual void step() 
    {
        // Если не осталось живых фигур, то пропускается ход
        if (get_count_live_figure() != 0)
        {
            on_box_cool_figures(); 
        }
        else 
        {
            GameObject.FindWithTag("play_session").GetComponent<Play_session>().step();
        }
    }

    public void on_box_cool_figures() 
    {
        // Включение боксколайдеров у всех фигур игрока
        for (int i = 0; i < figures.Count; i++)
        {

            figures[i].on_box_coll();
        }
    }

    public void off_box_cool_figures()
    {
        // Выключение боксколайдеров у всех фигур игрока
        for (int i = 0; i < figures.Count; i++)
        {
            figures[i].off_box_coll();
        }
    }

    public int get_count_live_figure() 
    {
        // Возвращяет количество живых фигур игрока
        int count_live = 0;

        for (int i = 0; i < figures.Count; i++)
        {
            if (figures[i].get_live() == true)
                count_live++;
        }

        return count_live;
    }
    public void remove_figure(Figure _figure) { figures.Remove(_figure); }
    public List<Figure> get_figures() {return figures;}
    public int get_count_figure()         {return figures.Count;}
    public Corner get_corner()               {return corner; }
    public Figure get_figure(int i)   {return figures[i];}
    public void set_score(int _score)     {score = _score; }




    public virtual void test_step(Figure _figure, Vector3 _new_position) 
    {
        return;
    }
}
