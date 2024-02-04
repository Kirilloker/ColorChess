using System.Collections.Generic;
using ColorChessModel;

public class Step
{
    private Figure figure;
    private Cell cell;

    public Step(Figure _figure, Cell _cell)
    {
        // Тут надо что-то сделать но пока не очень понятно
        this.figure = _figure;
        this.cell = _cell;
    }

    public Step() 
    {
        this.figure = null;
        this.cell = null;
    }

    public Figure Figure { get { return figure; } set { figure = value; } }
    public Cell Cell { get { return cell; } set { cell = value; } }

    public bool IsReal(Map map)
    {
        List<Cell> allSteps = WayCalcSystem.CalcAllSteps(map, figure);

        return allSteps.Contains(cell);
    }
}