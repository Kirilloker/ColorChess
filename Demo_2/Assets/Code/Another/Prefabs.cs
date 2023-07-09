using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour 
{
    [SerializeField]
    private GameObject Cell;
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

    public GameObject GetFigure(FigureType figureType)
    {
        switch (figureType)
        {
            case FigureType.Pawn:
                return Pawn;
            case FigureType.King:
                return King;
            case FigureType.Bishop:
                return Bishop;
            case FigureType.Castle:
                return Castle;
            case FigureType.Horse:
                return Horse;
            case FigureType.Queen:
                return Queen;
            default:
                Debug.Log("Нет такого типа фигуры");
                return Pawn;
        }
    }

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
                Debug.Log("Ошибка: неизвестный цвет");
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
                    Debug.Log("Ошибка: неизвестный цвет");
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
                    Debug.Log("Ошибка: неизвестный цвет");
                    return defaultColorDark;
            }
        }
        else
        {
            return defaultColorDark;
        }
    }
}
