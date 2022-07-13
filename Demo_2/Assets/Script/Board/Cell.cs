using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class Cell : MonoBehaviour
{
    // ���������
    [SerializeField] Prompt           prompt = null;  
    // ����� ������ ������ ������ (���� null - � ������ �����)
    [SerializeField] Figure               who_in_cell = null;
    // ������ ������
    [SerializeField] cell_description     status = new cell_description("void", 0);

    [SerializeField] GameObject prefabs_prompt;

    [SerializeField] Get_materials getMaterial;

    private void Start()
    {
        Cell_create();
    }

    public void Cell_create() 
    {
        // �������� ������ ������ � ���������   
        this.name = "Cell" + this.transform.localPosition.x + "_" + this.transform.localPosition.z;

        Transform parent = GameObject.FindWithTag("Prompt").transform;
        this.prompt = (Instantiate(prefabs_prompt, transform.localPosition, Quaternion.identity, parent)).GetComponent<Prompt>();

        getMaterial = GameObject.FindWithTag("Materials").GetComponent<Get_materials>();
    }

    public void Cell_destroy() 
    {
        // �������� ������ � ��������� �� ��� 
        prompt.Promt_destroy();
        GameObject.Destroy(gameObject);
    }

    private void OnMouseUpAsButton()
    {
        Figure pressed_figure = GameObject.FindWithTag("play_session").GetComponent<Play_session>().pressed_figure;

        // ���� �����-���� ������ ������ 
        if (pressed_figure != null)
        {

            // ���� ������ �� ������, �� ������� ��������� ������ �������� ������ ����
            if (pressed_figure == who_in_cell)
            {
                pressed_figure.figure_down();
            }
            else
            {

                List<Vector3> Available = pressed_figure.get_available();

                // ���� �� ����� ���� �������, �� �����
                if (Available.Contains(get_localpos()) == true) 
                {
                    pressed_figure.move_figure(get_localpos());
                }
            }
        }
    }

    public void change_color() 
    {
        // ������ ������ ���� ���� � ����������� �� ������� 
        this.GetComponent<MeshRenderer>().material = getMaterial.get_color_cell(status);
    }

    public bool avalible(List<cell_description> fig_reqire)
    {
        for (int i = 0; i < fig_reqire.Count; i++)
        {
            if (fig_reqire[i].status == status.status &&
                fig_reqire[i].number_player == status.number_player) { return true; }
        }
        return false;


        Debug.Log("������:" + status);
        for (int i = 0; i < fig_reqire.Count; i++)
        {
            Debug.Log("reqire:" + fig_reqire[i]);
        }
        // ��������� �����, ����� �� ������ ������ ��������� �� ������
        return fig_reqire.Contains(status);
    }

    public void show_promt()  { prompt.show(get_figure_in_cell()); }
    public void hide_promt()  { prompt.hide(); }
    public void set_status(cell_description _status)
    {
        if (_status.status != null && status.status != "finished") status.status = _status.status;
        if (_status.number_player != int.MaxValue) status.number_player = _status.number_player;
        status.eating = _status.eating;

        change_color();
    }
    public void set_figure_in_cell(bool _figure_in_cell = false, Figure _who_in_cell = null)
    {
        // ���� ������, ��� _figure_in_cell = false, ������������� �� ������ ������ ������
        // ������ who_in_cell = null
     

        if (_figure_in_cell == false)
        {
            who_in_cell = null;

            set_status(new cell_description(false));
        }
        else
        {
            who_in_cell = _who_in_cell;
        }
    }
    public void set_who_in_cell(Figure _who_in_cell) { who_in_cell = _who_in_cell; }
    public Vector3 get_localpos()        { return this.transform.localPosition; }
    public cell_description get_status() { return status; }
    public Figure get_who_in_cell()  { return who_in_cell; }
    public bool get_figure_in_cell()     
    {
        if (who_in_cell == null) return false;
        return true;
    }
}
