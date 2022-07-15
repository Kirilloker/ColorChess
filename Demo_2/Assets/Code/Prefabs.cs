using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour 
{
    [SerializeField]
    private GameObject Cell;
    [SerializeField]
    private GameObject Prompt;
    [SerializeField]
    private GameObject Board;



    [SerializeField]
    private GameObject King;
    [SerializeField]
    private GameObject Queen;
    [SerializeField]
    private GameObject Castle;
    [SerializeField]
    private GameObject Bishop;
    [SerializeField]
    private GameObject Horse;
    [SerializeField]
    private GameObject Pawn;



    //[SerializeField]
    //private List<Dictionary<string, Material>> player_material;   // ������ �� ������� ����� ������� ������
    //[SerializeField]
    //private Material eating_material;
    //[SerializeField]
    //private Material base_cell;
    //[SerializeField]
    //private Material finished;
    //[SerializeField]

    //private Material material_players_base;
    //[SerializeField]
    //private Material base_figure_default;
    //[SerializeField]
    //private List<Material> dark_colors;
    //[SerializeField]
    //private List<Material> colors;

    [SerializeField]
    private Material red;
    [SerializeField]
    private Material yellow;
    [SerializeField]
    private Material blue;
    [SerializeField]
    private Material green;
    [SerializeField]
    private Material purple;
    [SerializeField]
    private Material defaultColor;


    [SerializeField]
    private Material redDark;
    [SerializeField]
    private Material yellowDark;
    [SerializeField]
    private Material blueDark;
    [SerializeField]
    private Material greenDark;
    [SerializeField]
    private Material purpleDark;
    [SerializeField]
    private Material defaultColorDark;

    public GameObject GetBoard() { return Board; }
    public GameObject GetCell() { return Cell; }

    public Material GetColor(ColorType color) 
    {
        switch (color)
        {
            case ColorType.Red:
                return red;
            case ColorType.Blue:
                return blue; 
            case ColorType.Yellow:
                return yellow;
            case ColorType.Green:
                return green;
            case ColorType.Purple:
                return purple;
            default:
                Debug.Log("������: ����������� ����");
                return defaultColor;
        }
    }

    public Material GetColorCell(ColorType color, CellType cellType)
    {
        if (cellType == CellType.Empty)
        {
            return defaultColor;
        }
        else if(cellType == CellType.Paint)
        {
            switch (color)
            {
                case ColorType.Red:
                    return red;
                case ColorType.Blue:
                    return blue;
                case ColorType.Yellow:
                    return yellow;
                case ColorType.Green:
                    return green;
                case ColorType.Purple:
                    return purple;
                default:
                    Debug.Log("������: ����������� ����");
                    return defaultColor;
            }
        }
        else if (cellType == CellType.Dark)
        {
            switch (color)
            {
                case ColorType.Red:
                    return redDark;
                case ColorType.Blue:
                    return blueDark;
                case ColorType.Yellow:
                    return yellowDark;
                case ColorType.Green:
                    return greenDark;
                case ColorType.Purple:
                    return purpleDark;
                default:
                    Debug.Log("������: ����������� ����");
                    return defaultColorDark;
            }
        }
        else
        {
            return defaultColorDark;
        }

    }
}
