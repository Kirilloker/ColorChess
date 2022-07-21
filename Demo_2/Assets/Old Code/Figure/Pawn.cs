using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Figure
{
    /*
    private void Start()
    {
        step_animation = 0.25f;

        this.name = "Pawn" + (this.transform.position.x) + " " + (this.transform.position.z);

        abstract_fig_reqire = new List<string>()
        {
            "void",
            "selfpaint",
            "selfdark",
            "anotherpaint"

        };

        create_figure();
    }

    public override void ability_figure(Vector3 position)
    {
        // Съедает фигуру

        Cell cell = _Board.get_cell(position.x, position.z);

        if (cell.get_figure_in_cell())
        {
            cell.get_who_in_cell().kill_figure();
        }
    }

    public override List<Vector3> calculate_way(Vector3 end_position, Vector3 start_position)
    {
        List<Vector3> way = new List<Vector3>();

        way.Add(start_position);
        way.Add(end_position);

        return way;
    }
    public override List<Vector3> get_available()
    {
        List<Vector3> avail = new List<Vector3>();

        for (float i = (transform.localPosition.x - 1f); i <= transform.localPosition.x + 1f; i++)
        {
            if (i < 0 || i > _Board.get_lenght() - 1) { continue; }

            for (float j = (transform.localPosition.z - 1f); j <= transform.localPosition.z + 1f; j++)
            {
                if (j < 0 || j > _Board.get_width() - 1) { continue; }

                else if ((i == transform.localPosition.x) && (j == transform.localPosition.z)) { continue; }

                Cell cell = _Board.get_cell(i, j);

                Figure who_take_figure = cell.get_who_in_cell();

                // Чтобы не съесть свою фигуру
                if (who_take_figure != null)
                {
                    if (who_take_figure.number_player == number_player) { continue; }
                }
                if (!cell.avalible(fig_reqire)) { continue; }
                avail.Add(cell.get_localpos());
               
            }
        }

        return avail;
    }

    public override bool check_enemy_block()
    {
        bool another_figure_close = false;

        for (float i = (transform.localPosition.x - 1f); i <= (transform.localPosition.x + 1f); i++)
        {
            if (i < 0 || i > _Board.get_lenght() - 1) { continue; }

            for (float j = (transform.localPosition.z - 1f); j <= (transform.localPosition.z + 1f); j++)
            {
                if (j < 0 || j > _Board.get_width() - 1) { continue; }

                if (bool_enemy_block(i, j)) return false;

                if (!another_figure_close && (_Board.get_cell(i, j).get_who_in_cell() != null)) 
                    another_figure_close = _Board.get_cell(i, j).get_who_in_cell().number_player != number_player;
            }
        }
        if (another_figure_close) return true;
        else return false;
    }
    */

}
