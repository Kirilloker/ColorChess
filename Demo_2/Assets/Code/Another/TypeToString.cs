
namespace ColorChessModel
{
    public static class TypeToString
    {
        // На вход поступает Енуменатор - возвращает строку 
        public static string ToString(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Human:
                    return "Human";
                case PlayerType.AI:
                    return "AI";
                case PlayerType.AI2:
                    return "AI2";
                case PlayerType.Online:
                    return "Online";
                default:
                    return "WARNING!";
            }
        }

        public static string ToString(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.Empty:
                    return "Empty";
                case CellType.Paint:
                    return "Paint";
                case CellType.Dark:
                    return "Dark";
                default:
                    return "WARNING!";
            }
        }


        public static string ToString(FigureType figureType)
        {
            switch (figureType)
            {
                case FigureType.Empty:
                    return "Empty";
                case FigureType.Pawn:
                    return "Pawn";
                case FigureType.King:
                    return "King";
                case FigureType.Bishop:
                    return "Bishop";
                case FigureType.Castle:
                    return "Castle";
                case FigureType.Horse:
                    return "Horse";
                case FigureType.Queen:
                    return "Queen";
                default:
                    return "WARNING!";
            }
        }

        public static string ToString(CornerType corner)
        {
            switch (corner)
            {
                case CornerType.DownLeft:
                    return "DownLeft";
                case CornerType.DownRight:
                    return "DownRight";
                case CornerType.UpRight:
                    return "UpRight";
                case CornerType.UpLeft:
                    return "UpLeft";
                default:
                    return "WARNING!";
            }
        }

        public static string ToString(ColorType color)
        {
            switch (color)
            {
                case ColorType.Red:
                    return "Red";
                case ColorType.Blue:
                    return "Blue";
                case ColorType.Yellow:
                    return "Yellow";
                case ColorType.Green:
                    return "Green";
                case ColorType.Purple:
                    return "Purple";
                default:
                    return "WARNING!";
            }
        }

        // На вход получает строку - возращает Енуменатор
        public static CellType ToType(string strCellType)
        {
            switch (strCellType)
            {
                case "void":
                    return CellType.Empty;
                case "empty":
                    return CellType.Empty;
                case "paint":
                    return CellType.Paint;
                case "dark":
                    return CellType.Dark;
                default:
                    return CellType.Empty;
            }
        }


    }



}
