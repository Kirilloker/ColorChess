namespace ColorChessModel.Tools
{
    public static class Printer
    {
        public static void PrintMap(Map map)
        {
            Console.Clear();
            Console.WriteLine("   \t" + string.Join("\t", Enumerable.Range(0, map.Width)));

            for (int i = 0; i < map.Width; i++)
            {
                Console.Write(i + "  \t");

                for (int j = 0; j < map.Length; j++)
                {
                    PrintCell(map.Cells[i, j]);
                    Console.Write("\t");
                }

                Console.WriteLine("\n");
            }
        }

        public static void PrintCell(Cell cell)
        {
            Console.ForegroundColor = GetCellForegroundColor(cell);
            Console.BackgroundColor = GetCellBackgroundColor(cell);
            Console.Write(GetStringCell(cell));
            Console.ResetColor();
        }

        public static string GetStringCell(Cell cell)
        {
            string str = cell.NumberPlayer.ToString();

            switch (cell.Type)
            {
                case CellType.Empty:
                    str += "E";
                    break;
                case CellType.Paint:
                    str += "P";
                    break;
                case CellType.Dark:
                    str += "D";
                    break;
                case CellType.Block:
                    str += "B";
                    break;
                default:
                    str += "!";
                    break;
            }

            switch (cell.FigureType)
            {
                case FigureType.Empty:
                    str += "E";
                    break;
                case FigureType.Pawn:
                    str += "P";
                    break;
                case FigureType.King:
                    str += "K";
                    break;
                case FigureType.Bishop:
                    str += "B";
                    break;
                case FigureType.Castle:
                    str += "C";
                    break;
                case FigureType.Horse:
                    str += "H";
                    break;
                case FigureType.Queen:
                    str += "Q";
                    break;
                default:
                    str += "!";
                    break;
            }

            return str;
        }

        public static ConsoleColor GetCellForegroundColor(Cell cell)
        {
            switch (cell.FigureType)
            {
                case FigureType.Empty:
                    return ConsoleColor.White;
                case FigureType.Pawn:
                    return ConsoleColor.Cyan;
                case FigureType.King:
                    return ConsoleColor.Magenta;
                case FigureType.Bishop:
                    return ConsoleColor.Yellow;
                case FigureType.Castle:
                    return ConsoleColor.Blue;
                case FigureType.Horse:
                    return ConsoleColor.Gray;
                case FigureType.Queen:
                    return ConsoleColor.DarkMagenta;
                default:
                    return ConsoleColor.Yellow;
            }
        }

        public static ConsoleColor GetCellBackgroundColor(Cell cell)
        {
            int numberPlayer = cell.NumberPlayer;
            switch (cell.Type)
            {
                case CellType.Empty:
                    return ConsoleColor.Black;
                case CellType.Paint:
                    if (numberPlayer == 0)
                        return ConsoleColor.Green;
                    else return ConsoleColor.Red;
                case CellType.Dark:
                    if (numberPlayer == 0)
                        return ConsoleColor.Yellow;
                    else return ConsoleColor.Blue;
                case CellType.Block:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Black;
            }
        }
    }
}
