using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Figure
{
    /*
    private void Start()
    {

        step_animation = 0.5f;

        this.name = "Queen" + (this.transform.position.x) + " " + (this.transform.position.z);

        abstract_fig_reqire = new List<string>()
        {
            "void",
            "selfpaint",
            "selfdark",
            //"anotherpaint"
        };

        create_figure();
    }

    public override List<Vector3> calculate_way(Vector3 end_position, Vector3 start_position)
    {
        List<Vector3> way = new List<Vector3>();

        bool Founded_way = false;

        float x_coor = start_position.x;
        float z_coor = start_position.z;

        for (int i = -3; i <= 3; i += 2)
        {
            if (Founded_way) { break; }

            way = new List<Vector3>();

            if (i == -3 || i == 3) 
            {
                for (float j = 0; j < _Board.get_lenght(); j++)
                {
                    way.Add(new Vector3(x_coor + j * (i % 2), 0f, z_coor));

                    if (new Vector3(x_coor + j * (i % 2), 0f, z_coor) == end_position)
                    {
                        Founded_way = true;
                        break;
                    }
                }

                if (Founded_way) { break; }
                way = new List<Vector3>();

                for (float j = 0; j < _Board.get_lenght(); j++)
                {
                    way.Add(new Vector3(x_coor + j * (i % 2), 0f, (z_coor + j * (i % 2))));

                    if (new Vector3(x_coor + j * (i % 2), 0f, (z_coor + j * (i % 2))) == end_position)
                    {
                        Founded_way = true;
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < _Board.get_width(); j++)
                {
                    way.Add(new Vector3(x_coor, 0f, z_coor + j * (i % 2)));

                    if (new Vector3(x_coor, 0f, z_coor + j * (i % 2)) == end_position)
                    {
                        Founded_way = true;
                        break;
                    }
                }

                if (Founded_way) { break; }
                way = new List<Vector3>();

                for (int j = 0; j < _Board.get_width(); j++)
                {
                    way.Add(new Vector3(x_coor + j * (i % 2), 0f, (z_coor - j * (i % 2))));

                    if (new Vector3(x_coor + j * (i % 2), 0f, (z_coor - j * (i % 2))) == end_position)
                    {
                        Founded_way = true;
                        break;
                    }
                }
            }
        }
        return way;
    }

    public override List<Vector3> get_available()
    {
        List<Vector3> avail = new List<Vector3>();

        float x_coor = transform.localPosition.x;
        float z_coor = transform.localPosition.z;

        for (int i = -3; i <= 3; i += 2)
        {

            if (i == -3 || i == 3) 
            {
                for (float j = 0; j < _Board.get_lenght(); j++)
                {
                    if ((x_coor + j * (i % 2)) < 0 || (x_coor + j * (i % 2)) > _Board.get_lenght() - 1) { break; }
                    if ((z_coor + j * (i % 2)) < 0 || (z_coor + j * (i % 2)) > _Board.get_width() - 1) { break; }

                    Cell cell = _Board.get_cell(x_coor + j * (i % 2), z_coor + j * (i % 2));

                    if (!cell.avalible(fig_reqire)) { break; }

                    if (cell.get_figure_in_cell() && (x_coor + j * (i % 2)) != x_coor && (z_coor + j * (i % 2) != z_coor)) { break; }

                    if (x_coor == x_coor + j * (i % 2) && z_coor == z_coor + j * (i % 2)) { continue; }

                    avail.Add(cell.get_localpos());
                }
            }
            else
            {
                for (int j = 0; j < _Board.get_width(); j++)
                {
                    if ((x_coor + j * (i % 2)) < 0 || (x_coor + j * (i % 2)) > _Board.get_lenght() - 1) { break; }
                    if ((z_coor - j * (i % 2)) < 0 || (z_coor - j * (i % 2)) > _Board.get_width() - 1) { break; }

                    Cell cell = _Board.get_cell(x_coor + j * (i % 2), z_coor - j * (i % 2));

                    if (!cell.avalible(fig_reqire)) { break; }

                    if (cell.get_figure_in_cell() && (x_coor + j * (i % 2)) != x_coor && (z_coor - j * (i % 2)) != z_coor) { break; }

                    if (x_coor == x_coor + j * (i % 2) && z_coor == z_coor - j * (i % 2)) { continue; }

                    avail.Add(cell.get_localpos());
                }
            }
            if (i == -3 || i == 3) 
            {
                for (float j = 0; j < _Board.get_lenght(); j++)
                {
                    if ((x_coor + j * (i % 2)) < 0 || (x_coor + j * (i % 2)) > _Board.get_lenght() - 1) { break; }

                    Cell cell = _Board.get_cell(x_coor + j * (i % 2), z_coor);

                    if (!cell.avalible(fig_reqire)) { break; }

                    if (cell.get_figure_in_cell() && (x_coor + j * (i % 2)) != x_coor) { break; }

                    if (x_coor == x_coor + j * (i % 2)) { continue; }

                    avail.Add(cell.get_localpos());
                }
            }
            else
            {
                for (int j = 0; j < _Board.get_width(); j++)
                {
                    if ((z_coor + j * (i % 2)) < 0 || (z_coor + j * (i % 2)) > _Board.get_width() - 1) { break; }

                    Cell cell = _Board.get_cell(x_coor, z_coor + j * (i % 2));

                    if (!cell.avalible(fig_reqire)) { break; }

                    if (cell.get_figure_in_cell() && (z_coor + j * (i % 2)) != z_coor) { break; }

                    if (z_coor == z_coor + j * (i % 2)) { continue; }

                    avail.Add(cell.get_localpos());
                }
            }
        }
        return avail;
    }

    public override bool check_enemy_block()
    {
        int x_coor = (int)transform.localPosition.x;
        int z_coor = (int)transform.localPosition.z;

        bool another_figure_close = false;

        for (float i = (x_coor - 1f); i <= (x_coor + 1f); i++)
        {
            if (i < 0 || i > _Board.get_lenght() - 1) { continue; }

            for (float j = (z_coor - 1f); j <= (z_coor + 1f); j++)
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


    public override void ability_figure(Vector3 position)
    {
        return;
    }
*/
}
