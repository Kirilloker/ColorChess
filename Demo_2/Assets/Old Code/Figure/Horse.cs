using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Figure
{
    /*
    private void Start()
    {

        step_animation = 0.2f;

        this.name = "Horse" + (this.transform.position.x) + " " + (this.transform.position.z); 

        abstract_fig_reqire = new List<string>()
        {
            "void",
            "selfpaint",
            "selfdark",
            "anotherpaint"
        };

        create_figure();
    }

    public override List<Vector3> calculate_way(Vector3 end_position, Vector3 start_position)
    {
        List<Vector3> way = new List<Vector3>();

        int x_coor = (int)start_position.x;
        int z_coor = (int)start_position.z;

        way.Add(start_position);

        if ((Mathf.Abs(end_position.x - x_coor)) > (Mathf.Abs(end_position.z - z_coor)))
        {
            if (jump_horse(new Vector3(x_coor + ((end_position.x - x_coor) / 2), 0f, z_coor)))
            {
                way.Add(new Vector3(x_coor + ((end_position.x - x_coor) / 2), 0f, z_coor));
            }

            if (jump_horse(new Vector3(x_coor + ((end_position.x - x_coor)), 0f, z_coor)))
            {
                way.Add(new Vector3(x_coor + ((end_position.x - x_coor)), 0f, z_coor));
            }

            if (jump_horse(new Vector3(end_position.x, 0f, end_position.z)))
            {
                way.Add(new Vector3(end_position.x, 0f, end_position.z));
            }
        }
        else
        {
            if (jump_horse(new Vector3(x_coor, 0f, z_coor + ((end_position.z - z_coor) / 2))))
            {
                way.Add(new Vector3(x_coor, 0f, z_coor + ((end_position.z - z_coor) / 2)));
            }

            if (jump_horse(new Vector3(x_coor, 0f, z_coor + ((end_position.z - z_coor)))))
            {
                way.Add(new Vector3(x_coor, 0f, z_coor + ((end_position.z - z_coor))));
            }

            if (jump_horse(new Vector3(end_position.x, 0f, end_position.z)))
            {
                way.Add(new Vector3(end_position.x, 0f, end_position.z));
            }
        }

        return way;
    }
    bool jump_horse(Vector3 cell_vector)
    {
        Cell cell = _Board.get_cell(cell_vector.x, cell_vector.z);

        if (cell.get_figure_in_cell() || !cell.avalible(fig_reqire)) { return false;}

        return true;
    }


    public override List<Vector3> get_available()
    {
        List<Vector3> avail = new List<Vector3>();

        float x_coor = this.transform.localPosition.x;
        float z_coor = this.transform.localPosition.z;

        for (float i = (x_coor - 2f); i <= x_coor + 2f; i++)
        {
            if (i < 0 || i > _Board.get_lenght() - 1) { continue; }

            for (float j = (z_coor - 2f); j <= z_coor + 2f; j++)
            {
                if (j < 0 || j > _Board.get_width() - 1) { continue; }

                if (((Mathf.Abs(i - x_coor) == 1) && (Mathf.Abs(j - z_coor) == 2)) || ((Mathf.Abs(i - x_coor) == 2) && (Mathf.Abs(j - z_coor) == 1)))
                {
                    Cell cell = _Board.get_cell(i, j);

                    if (cell.get_figure_in_cell()) { continue; }
                    else if (!cell.avalible(fig_reqire)) { continue; }
                    avail.Add(cell.get_localpos());
                }
            }
        }
        return avail;
    }

    public override bool check_enemy_block()
    {
        bool another_figure_close = false;

        for (float i = (transform.localPosition.x - 2f); i <= (transform.localPosition.x + 2f); i++)
        {
            if (i < 0 || i > _Board.get_lenght() - 1) { continue; }

            for (float j = (transform.localPosition.z - 2f); j <= (transform.localPosition.z + 2f); j++)
            {
                if (j < 0 || j > _Board.get_width() - 1) { continue; }

                if (((Mathf.Abs(i - transform.localPosition.x) == 1) && (Mathf.Abs(j - transform.localPosition.z) == 2)) || ((Mathf.Abs(i - transform.localPosition.x) == 2) && (Mathf.Abs(j - transform.localPosition.z) == 1)))
                {
                    if (bool_enemy_block(i, j)) return false;

                    if (!another_figure_close && (_Board.get_cell(i, j).get_who_in_cell() != null)) another_figure_close = _Board.get_cell(i, j).get_who_in_cell().number_player != number_player;
                }
            }
        }

        if (another_figure_close) return true;
        else return false;
    }

    public override void ability_figure(Vector3 position)
    {
        return;
    }
    */
}
