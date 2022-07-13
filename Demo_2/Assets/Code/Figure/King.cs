using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Figure
{
    private void Start()
    {

        step_animation = 0.05f;

        this.name = "King" + (this.transform.position.x) + " " + (this.transform.position.z); 

        abstract_fig_reqire = new List<string>()
        {
            "void",
            "selfpaint",
            "selfdark"
        };

        create_figure();
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

        for (float i = 0; i < _Board.get_lenght(); i++)
        {

            for (float j = 0; j < _Board.get_width(); j++)
            {
                Cell cell = _Board.get_cell(i, j);

                if ((i == transform.localPosition.x) && (j == transform.localPosition.z)) { continue; }
                else if (cell.get_figure_in_cell()) { continue; }
                else if (!cell.avalible(fig_reqire)) { continue; }

                avail.Add(cell.get_localpos());
            }
        }

        return avail;
    }

    public override bool check_enemy_block()
    {
        return false;
    }

    public override void ability_figure(Vector3 position)
    {
        return;
    }
}
