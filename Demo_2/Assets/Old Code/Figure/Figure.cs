using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    /*
    // ������ ��� �������
    [SerializeField] Cell cell_under; 

    // ����������� ������ ���� ����� ������� ������
    protected List<string> abstract_fig_reqire;

    // ���������� ������ ���� ����� ������� ������ ������
    protected List<cell_description> fig_reqire;

    protected Play_session _play_session;
    protected Board _Board;
    protected Player _player;

    // ��� �� �����
    [SerializeField] protected bool live = true;

    // �� ����� ���� ������ ������
    protected int count_die = -1;

    //����� ������
    public int number_player;

    // �������� ��������
    protected float step_animation = 0.1f;

    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider>();
    }

    public void create_figure() 
    {
        // �������� ������ 

        // ���������� ������ �� �������
        _Board = GameObject.FindWithTag("Board").GetComponent<Board>();
        _play_session = GameObject.FindWithTag("play_session").GetComponent<Play_session>();
        _player = transform.parent.GetComponent<Player>();

        

        // ������������� ����� ������
        set_number_player();

        // ��������� ������ ��������� ������
        convert_to_fig_reqire();

        // ������� ������ ��� ������� � ������ �� ������
        find_cell_under();
        cell_description _status = new cell_description("paint", number_player);
        change_status_cell_under(_status);
    }

    private void OnMouseUpAsButton()
    {
        if (_play_session.get_number_player_step() != number_player) return;

        // ���� �����-�� ������ ���� �������, �� �������� �
        if (_play_session.pressed_figure != null) 
        {
            _play_session.pressed_figure.figure_down();   
        }

        // ������� play_session, ��� ���� ������ ������
        _play_session.pressed_figure = this;
        figure_up();

        // �������� ��������� ��� ���� ��������, ���� ����� �������
        _Board.ON_prompt(get_available());
    }

    public void move_figure(Vector3 end_position)
    {
        Vector3 start_position = transform.localPosition;
        List<Vector3> way_massive = calculate_way(end_position, start_position);

        // ����������� ����������� ������
        ability_figure(end_position);

        // �������, ������ ��� �������, ��� �� ��� ������ �� ����� 
        _Board.get_cell(start_position.x, start_position.z).set_figure_in_cell(false);

        // ��������� �������� � ������������� � ����� ������
        _Board.OFF_prompt();
        _player.off_box_cool_figures();

        // �������� ������������
        StartCoroutine(animate_move_figure(end_position, true));
        //animate_figure(figure, end_position, true);

        // ����� ��������� ������������� � ������ ����������
        //figure.transform.localPosition = new Vector3(end_position.x, figure.transform.localPosition.y, end_position.z);

        figure_down();

        _player.test_step(this, end_position);
    }

    public virtual IEnumerator animate_move_figure(Vector3 end_position, bool usual_step)
    {
        Vector3 start_position = transform.localPosition;
        List<Vector3> way_massive = new List<Vector3>();
       
        if (usual_step == true)
        {
                way_massive = calculate_way(end_position, start_position);
        }
        else
        {
            if (this.get_live() == false)
            {
                way_massive.Add(start_position);
                way_massive.Add(end_position);
            }
            else
            {
                way_massive = calculate_way(start_position, end_position);

                if (way_massive.Contains(start_position) == false) way_massive.Add(start_position);
                way_massive.Reverse();
            }
        }


        // ��������
        float progress;

        end_position.y = transform.localPosition.y;


        for (int i = 0; i < way_massive.Count; i++)
        {
            progress = 0f;

            if (usual_step == true)
            {
                cell_description new_status_cell = new cell_description("paint", number_player);
                set_status_cell(way_massive[i], new_status_cell);
            }

            if (ChangeSettingGraphics.LowGraphics == true) { continue; }

            if (i != 0)
            {
                start_position.x = way_massive[i - 1].x;
                start_position.z = way_massive[i - 1].z;
                start_position.y = end_position.y;
            }

            end_position.x = way_massive[i].x;
            end_position.z = way_massive[i].z;

            while (transform.localPosition != end_position)
            {
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                transform.localPosition = Vector3.Lerp(start_position, end_position, progress);
                progress += (step_animation * 1f);
            }
        }

        if (ChangeSettingGraphics.LowGraphics == true) { transform.localPosition = end_position; }

        yield return new WaitForSeconds(0.02f * Time.deltaTime);

        
        figure_down();
        find_cell_under();
        boxCollider.enabled = false; // ��� �� ������� ��-�� ���� ���� ���

        if (usual_step == true) _play_session.step();
    }


    public void delete_figure(Figure _delete_figure = null)
    {
        // �������� ������, ���� ������ �� ��������, �� ��������� ������, ��������� ��� ������
        // ���� �������� ������ �� ������, �� ��� � ��������

        if (_delete_figure == null) _delete_figure = this;

        _delete_figure.cell_under.set_figure_in_cell(false);

        _player.remove_figure(this);

        GameObject.Destroy(_delete_figure); 
    }

    public void kill_figure() 
    {
        // ������� ������

        cell_under.set_figure_in_cell(false);
        live = false;
        count_die = _play_session.counter_step;

        //this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 100f, this.transform.localPosition.z);
        this.transform.localPosition = new Vector3(-3 - ((_play_session.count_die_figure() - 1) / 5), -0.4f, 2 + ((_play_session.count_die_figure() - 1) % 5));
        boxCollider.enabled = false;
        
    }

    public List<cell_description> convert_to_fig_reqire()
    {
        // ����������� ������������� ������ ���������� ������ � ����������� ������
        //int Player_Count = transform.parent.parent.GetComponent<Play_session>().get_amount_player();
        int Player_Count = _play_session.get_amount_player();

        fig_reqire = new List<cell_description>();

        if (Tutorial._tutorual == true) 
        {
            fig_reqire.Add(new cell_description("finished", 10));
            fig_reqire.Add(new cell_description("finished", 1));
        }
        

        for (int i = 0; i < abstract_fig_reqire.Count; i++)
        {
            if (abstract_fig_reqire[i] == "void")
            {
                fig_reqire.Add(new cell_description("void", 0));
            }
            else if ((abstract_fig_reqire[i]).Remove(4) == "self")
            {
                fig_reqire.Add(new cell_description(abstract_fig_reqire[i].Substring(4), number_player));
            }
            else if ((abstract_fig_reqire[i]).Remove(7) == "another")
            {
                for (int j = 1; j <= Player_Count; j++)
                {
                    if (number_player != j)
                    {
                        fig_reqire.Add(new cell_description(abstract_fig_reqire[i].Substring(7), j));
                    }
                }
            }
            else
            {
                Debug.LogWarning("������: �� ������� ��� ������: " + abstract_fig_reqire[i]);
            }
        }
        return fig_reqire;
    }

    protected bool bool_enemy_block(float i, float j)
    {
        // ������� ����������� ����� �� ������ �������� �� ������ ������ 
        Cell cell = _Board.get_cell(i, j);

        // ���� � ������ ��� ������
        if (cell.get_figure_in_cell() == false)
        {
            // ���� ���� ������ ����� ��������� �� ��� ������ �� �������
            if (cell.avalible(fig_reqire) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (cell.get_who_in_cell().number_player != number_player)
        {
            return false;
        }
        else  return false; 
        // ��� ����� ��������� �������� ������
    }

    public void find_cell_under() 
    {
        // ������� ������ ��� �������
        cell_under = _Board.get_cell(transform.localPosition.x, transform.localPosition.z);
        cell_under.set_figure_in_cell(true, this);
    }

    // 0 ������?
    public void change_color_cell_under()
    {
        cell_under.change_color();
    }

    public void change_status_cell_under(cell_description _status)
    {
        cell_under.set_status(_status);
    }

    public void set_number_player() { number_player = _player.player_number; }

    private void set_status_cell(Vector3 _cell, cell_description _status)
    {
        Cell cell = _Board.get_cell((_cell.x), (_cell.z));

        if (cell.get_status().status != "dark")
        {
            cell.set_status(_status);
        }
    }

    public virtual List<Vector3> get_available()
    {
        // ������� ��� ��������� �����
        Debug.LogWarning("������: �� ������������� ����� Get_Available");

        return new List<Vector3>();
    }

    public virtual void ability_figure(Vector3 position)
    {
        // ����������� ������
        Debug.LogWarning("������: �� ������������� ����� ability_figure");

        return;
    }


    public virtual List<Vector3> calculate_way(Vector3 end_position, Vector3 start_position)
    {
        // ������� ���� �� ������ �� ������� ������� ������, ����� ����� ��������� � ����
        Debug.LogWarning("������: �� ������������� ����� calculate_way");

        return new List<Vector3>();
    }

    public virtual bool check_enemy_block()
    {
        // ����������� ��� ������������� �� ������ 
        Debug.LogWarning("������: �� ������������� ����� check_enemy_block");

        return false;
    }

    private void figure_up()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0.5f, this.transform.localPosition.z);
        off_box_coll();
    }

    public void figure_down()
    {
        // �������� ������, �������� � �� ������������, ��������� ���������
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0f, this.transform.localPosition.z);
        on_box_coll();
        _Board.OFF_prompt();
        _play_session.pressed_figure = null;
    }

    public virtual void on_box_coll() 
    { 
        if(live == true) boxCollider.enabled = true; 
    }
    public void off_box_coll()        { boxCollider.enabled = false; }
    public bool get_live()            { return live; }
    public void set_live(bool _live)  { live =  _live; }
    public int get_count_die()        { return count_die; }
    */
}


/*
    public void move_figure(Vector3 newposition, GameObject figure)
    {
        // �� ���� �������� �������� �� ������ ������� ������-��
        // ������� �������� ������� ����� ����������
        animate_move_figure(newposition, figure);
        _play_session.step();
    }

    public void animate_move_figure(Vector3 newposition, GameObject figure)
    {
        StartCoroutine(_move_figure(figure, newposition));
    }

    public virtual IEnumerator _move_figure(GameObject figure, Vector3 end_position) 
    {
        // ������������ ������ + ��������

        Vector3 start_position = figure.transform.localPosition;
        List<Vector3> way_massive = calculate_way(end_position);

        // ����������� ����������� ������
        ability_figure(end_position);

        // �������, ������ ��� �������, ��� �� ��� ������ �� ����� 
        _Board.get_cell(start_position.x, start_position.z).GetComponent<Cell>().set_figure_in_cell(false);

        // ��������� �������� � ������������� � ����� ������
        _Board.OFF_prompt();
        _player.off_box_cool_figures();


        // �������� =======================================================================================
        Vector3 i_start_position = start_position;
        Vector3 i_end_position = end_position;
        float progress = 0f;

        end_position.y = figure.transform.localPosition.y;

        step_animation = step_animation * 1.5f;

        for (int i = 0; i < way_massive.Count; i++)
        {
            progress = 0f;
            cell_description new_status_cell = new cell_description("paint", number_player);
            set_status_cell(way_massive[i], new_status_cell);

            if (i != 0) 
            {
                i_start_position.x = way_massive[i - 1].x;
                i_start_position.z = way_massive[i - 1].z;
            }
            i_end_position.x = way_massive[i].x;
            i_end_position.z = way_massive[i].z;

            while (transform.localPosition != i_end_position)
            {
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                transform.localPosition = Vector3.Lerp(i_start_position, i_end_position, progress);
                progress += step_animation;
            }
        }


        yield return new WaitForSeconds(0.002f);

        // ����� ��������� ������������� � ������ ����������
        figure.transform.localPosition = new Vector3(end_position.x, figure.transform.localPosition.y, end_position.z); 

        figure.GetComponent<Figure>().figure_down(); 
        find_cell_under();
        figure.GetComponent<BoxCollider>().enabled = false; // ��� �� ������� ��-�� ���� ���� ���
    }

*/























/*
    public void move_figure(Vector3 end_position, GameObject figure)
    {
        Vector3 start_position = figure.transform.localPosition;
        List<Vector3> way_massive = calculate_way(end_position);

        // ����������� ����������� ������
        ability_figure(end_position);

        // �������, ������ ��� �������, ��� �� ��� ������ �� ����� 
        _Board.get_cell(start_position.x, start_position.z).GetComponent<Cell>().set_figure_in_cell(false);

        // ��������� �������� � ������������� � ����� ������
        _Board.OFF_prompt();
        _player.off_box_cool_figures();


        StartCoroutine(animate_move_figure(figure, end_position, way_massive, start_position, true));


        // ����� ��������� ������������� � ������ ����������
        figure.transform.localPosition = new Vector3(end_position.x, figure.transform.localPosition.y, end_position.z);

        figure.GetComponent<Figure>().figure_down();
        find_cell_under();
        figure.GetComponent<BoxCollider>().enabled = false; // ��� �� ������� ��-�� ���� ���� ���

        _play_session.step();
    }

    public void animate_figure(GameObject figure, Vector3 end_position, bool bool_perem)
    {
        List<Vector3> way_massive = calculate_way(end_position);
        Vector3 start_position = figure.transform.localPosition; 

        StartCoroutine(animate_move_figure(figure, end_position, way_massive, start_position, bool_perem));
    }

    public virtual IEnumerator animate_move_figure(GameObject figure, Vector3 end_position, List<Vector3> way_massive, Vector3 start_position, bool bool_perem)
    {

        // �������� =======================================================================================
        Vector3 i_start_position = start_position;
        Vector3 i_end_position = end_position;
        float progress = 0f;

        end_position.y = figure.transform.localPosition.y;

        step_animation = step_animation * 1.5f;

        for (int i = 0; i < way_massive.Count; i++)
        {
            progress = 0f;

            if (bool_perem == true) 
            {
                cell_description new_status_cell = new cell_description("paint", number_player);
                set_status_cell(way_massive[i], new_status_cell);
            }


            if (i != 0)
            {
                i_start_position.x = way_massive[i - 1].x;
                i_start_position.z = way_massive[i - 1].z;
            }
            i_end_position.x = way_massive[i].x;
            i_end_position.z = way_massive[i].z;

            while (transform.localPosition != i_end_position)
            {
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                transform.localPosition = Vector3.Lerp(i_start_position, i_end_position, progress);
                progress += step_animation;
            }
        }

    }

*/

