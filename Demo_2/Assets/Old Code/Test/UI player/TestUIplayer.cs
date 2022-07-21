using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIplayer : MonoBehaviour
{
    /*
    public Text nickname;

    public Image corner_up_left;
    public Image corner_up_right;
    public Image corner_down_left;
    public Image corner_down_right;

    public GameObject image_hotseat;
    public GameObject image_bot;

    public GameObject up_hierarchy;
    public GameObject down_hierarchy;

    Material gray;
    Get_materials get_Materials;
    UI notebook;

    public int player_number = 1;

    public Corner corner_player;

    public string type_player;


    public void Start_player()
    {
        get_Materials = GameObject.FindWithTag("Materials").GetComponent<Get_materials>();
        notebook = GameObject.FindWithTag("Notebook").GetComponent<UI>();
        gray = get_Materials.get_color(Color.Default);
        clear_color_corner();
        if (notebook.GetCountPlayer() == player_number) down_hierarchy.SetActive(false);
        if (player_number == 1) up_hierarchy.SetActive(false);
    }

    public void change_type(string _type_player)
    {
        // —делать смену иконочек игрока!
        if (_type_player == "HotSeat") 
        {
            image_hotseat.SetActive(true);
            image_bot.SetActive(false);
        }
        else if (_type_player == "AI") 
        {
            image_hotseat.SetActive(false);
            image_bot.SetActive(true);
        }
        else 
        {
            Debug.LogWarning("ќшибка типа игрока: " + _type_player);
        }

        type_player = _type_player;

        notebook.ChangePlayerType(player_number, type_player);
    }
    public void change_nickname(string _nickname) 
    {
        nickname.text = _nickname;
        //notebook.ChangePlayerNickname(player_number, _nickname);
    }
    
    public void change_corner(Corner _corner) 
    {
        corner_player = _corner;
    }
    public void change_color_corner(Color color) 
    {
        clear_color_corner();

        switch (corner_player)
        {
            case Corner.Down_left:
                corner_down_left.material = get_Materials.get_color(color);
                break;
            case Corner.Down_right:
                corner_down_right.material = get_Materials.get_color(color);
                break;
            case Corner.Up_right:
                corner_up_right.material = get_Materials.get_color(color);
                break;
            case Corner.Up_left:
                corner_up_left.material = get_Materials.get_color(color);
                break;
            default:
                Debug.LogWarning("Ќе известный угол!");
                break;
        }

        //notebook.ChangePlayerCorner(player_number, corner_player);
    }

    public void clear_color_corner() 
    {
        corner_down_left.material = gray;
        corner_up_left.material = gray;
        corner_up_right.material = gray;
        corner_down_right.material = gray;
    }

    public void change_hierarchy(int number) 
    {
        notebook.ChangeHierarchy(player_number, number);
    }

    public void delete_self() 
    {
        Debug.Log("я что должен себ€ заху€рить?");
        notebook.DeletePlayer(player_number);
    }

    public void destroy() 
    {
        Destroy(gameObject);
    }
    */
}
