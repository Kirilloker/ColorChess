using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorChessModel;

public static class GameStateToIntArray
{
    static private byte FreeCellStateMask =         0b_00_000_000;
    static private byte PaintedCellStateMask =      0b_01_000_000;
    static private byte CapturedCellStateMask =     0b_11_000_000;

    static private byte EmptyCellMask =             0b_00_000_000;
    static private byte KingInCellMask =            0b_00_001_000;
    static private byte QueenInCellMask =           0b_00_010_000;
    static private byte PawnInCellMask =            0b_00_011_000;
    static private byte HorseInCellMask =           0b_00_100_000;
    static private byte BishopInCellMask =          0b_00_101_000;
    static private byte CastleInCellMask =          0b_00_110_000;

    static private byte NoOnePlayerInCellMask =     0b_00_000_111; //Если в клетке есть игрок, просто указываем его номер


    public static int[] ConvertMapToIntArray(Map gameState)
    {
        int boardLenght = gameState.Length;
        int boardWidth = gameState.Width;

        int numsOfInt = (boardLenght * boardWidth); 
        if (numsOfInt % 4 == 0) { numsOfInt /= 4; }
        else { numsOfInt = numsOfInt / 4 + 1; }

        int[] result = new int[numsOfInt];
        int curIntNumber = 0;
        int curByteNumberInInt = 0;
        int curCellValue = 0;

        for(int i = 0; i < boardLenght; i++)
        {
            for(int j = 0; j < boardWidth; j++)
            {
                curCellValue = 0;
                Cell curCell = gameState.GetCell(i, j);
                switch (curCell.type)
                {
                    case CellType.Empty:
                        curCellValue = FreeCellStateMask;
                        break;
                    case CellType.Paint:
                        curCellValue = PaintedCellStateMask;
                        break;
                    case CellType.Dark:
                        curCellValue = CapturedCellStateMask;
                        break;
                    default:
                        throw (new Exception("Unknown cell type on ConvertMapToByteArray"));

                }

                switch (curCell.FigureType)
                {
                    case FigureType.Empty:
                        curCellValue = curCellValue | EmptyCellMask;
                        break;
                    case FigureType.Pawn:
                        curCellValue = curCellValue | PawnInCellMask;
                        break;
                    case FigureType.King:
                        curCellValue = curCellValue | KingInCellMask;
                        break;
                    case FigureType.Bishop:
                        curCellValue = curCellValue | BishopInCellMask;
                        break;
                    case FigureType.Castle:
                        curCellValue = curCellValue | CastleInCellMask;
                        break;
                    case FigureType.Horse:
                        curCellValue = curCellValue | HorseInCellMask;
                        break;
                    case FigureType.Queen:
                        curCellValue = curCellValue | QueenInCellMask;
                        break;
                    default:
                        throw (new Exception("Unknown figure type on ConvertMapToByteArray"));
                }

                if(curCell.numberPlayer == -1) { curCellValue = curCellValue | NoOnePlayerInCellMask; }
                else { curCellValue = curCellValue | curCell.numberPlayer; }

                result[curIntNumber] = result[curIntNumber] | (curCellValue << (8 * curByteNumberInInt));

                curByteNumberInInt++;
                if(curByteNumberInInt == 4) 
                {
                    curByteNumberInInt = 0;
                    curIntNumber++;
                }
            }
        }

        return result;
    }
}