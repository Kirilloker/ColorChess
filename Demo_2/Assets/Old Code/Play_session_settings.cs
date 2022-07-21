using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_session_settings : MonoBehaviour
{
    /*
    #region Заполняемые поля
    [SerializeField]  int  width = 0;
    [SerializeField]  int  lenght = 0;
    [SerializeField]  bool camera_swap = false;
    List<Player_description> players_discription = new List<Player_description>();

    #endregion

    // Объект Игровой сессии
    GameObject play_session;   

    // Декоративная доска (скрывается в начале партии)
    public GameObject board_decor;

    // Позиция камеры (нужна для переворота)
    public GameObject position_camera;

    public void create_play_session(bool _tutorial = false)
    {
        Tutorial._tutorual = _tutorial;

        if (play_session != null) destroy_play_session();

        // Скрываем доску-декор
        board_decor.SetActive(false);

        // Создаем игровую сессию
        play_session = new GameObject();
        play_session.name = "play_session";
        play_session.tag = "play_session";
        play_session.transform.SetParent(GameObject.FindWithTag("Game").transform);
        play_session.AddComponent<Play_session>();

        // Настраиваем игровую сессию
        StartCoroutine(customization_session());
    }
    IEnumerator customization_session() 
    {
        Play_session _play_session = play_session.GetComponent<Play_session>();

        GameObject.FindWithTag("Materials").GetComponent<Get_materials>().set_play_session(_play_session);
        yield return new WaitForSeconds(0.01f);

        _play_session.set_players_discription(copy(players_discription));
        yield return new WaitForSeconds(0.01f);
        _play_session.create_board(width, lenght);

        yield return new WaitForSeconds(0.01f);
        _play_session.create_players();//

        yield return new WaitForSeconds(0.01f);
        _play_session.change_color_board();

        yield return new WaitForSeconds(0.01f);
        GameObject.FindWithTag("Game").transform.localScale = new Vector3(Mathf.Pow(0.9f, (float)(width - 9)), Mathf.Pow(0.9f, (float)(width - 9)), Mathf.Pow(0.9f, (float)(lenght - 9)));

        yield return new WaitForSeconds(0.01f);
        _play_session.set_camera(position_camera, camera_swap);

        yield return new WaitForSeconds(0.01f);
        _play_session.set_online_number(online_number);

        yield return new WaitForSeconds(0.7f);
        _play_session.start_game_cycle();
    }

    public bool correct_data() 
    {
        if (lenght == 0 || width == 0 || players_discription.Count <= 1 || players_discription.Count > 4)
            return false;
        else
            return true;
    }

    public void surrender() 
    {
        play_session.GetComponent<Play_session>().surrender();
    }

    public void clear_player_description() 
    {
        players_discription.Clear();
    }
    public void destroy_play_session()
    {
        board_decor.SetActive(true);
        GameObject.FindWithTag("Game").transform.localScale = new Vector3(1f, 1f, 1f);
        play_session.GetComponent<Play_session>().play_session_destroy();
    }
    public void set_players_discription(List<Player_description> _players_discription)
    {
        players_discription = copy(_players_discription);

        for (int i = players_discription.Count - 1; i >= 0; i--)
        {
            if (players_discription[i].player_type == "None") players_discription.RemoveAt(i);
        }
    }
    public void set_width(int _width)         {width = _width;}
    public void set_lenght(int _lenght)       {lenght = _lenght;}
    public void set_camera_swap(bool _switch) {camera_swap = _switch;}


    // ONLINE =============================================================================================
    public int online_number = 0;
    public void set_online_number(int number) 
    {
        online_number = number;
    }

    public List<Player_description> copy(List<Player_description> _players_discription) 
    {
        List<Player_description> pl_discript = new List<Player_description>();

        for (int i = 0; i < _players_discription.Count; i++)
        {
            pl_discript.Add(_players_discription[i]);
        }

        return pl_discript;
    }

    public void set_status_game(Step_description _step_description) 
    {
        play_session.GetComponent<Play_session>().set_status_game(_step_description);
    }
    */
}
