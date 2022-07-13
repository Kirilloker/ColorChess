using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_models : MonoBehaviour
{
    public GameObject Cell_prefab;
    public GameObject Prompt_prefab;
    public GameObject Board_prefab;

    public GameObject King;
    public GameObject Queen;
    public GameObject Castle;
    public GameObject Bishop;
    public GameObject Horse;
    public GameObject Pawn;

    public GameObject Random_custom;
    public GameObject custom1;

    public GameObject get_figure(string name_figure) 
    {
        if (name_figure == "King") return King;
        else if (name_figure == "Queen") return Queen;
        else if (name_figure == "Castle") return Castle;
        else if (name_figure == "Bishop") return Bishop;
        else if (name_figure == "Horse") return Horse;
        else if (name_figure == "Pawn") return Pawn;
        else if (name_figure == "Random_custom") return Random_custom;
        else if (name_figure == "custom1") return custom1;
        else
        {
            Debug.LogWarning("Ошибка: не найдена модель фигуры");
            return Pawn;
        }
    }

    public GameObject get_cell()   {return Cell_prefab;}
    public GameObject get_prompt() {return Prompt_prefab;}
    public GameObject get_board()  {return Board_prefab;}
}
