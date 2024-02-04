using System.Collections.Generic;


namespace ColorChessModel
{
    //Описывает всех игроков для создания
    public class PlayersDescription
    {
        public List<CornerType> PlayerCorners;
        public List<PlayerType> PlayerTypes;
        public List<ColorType> PlayerColors;
        public List<int> PlayerNumbers;
        public PlayersDescription()
        {
            PlayerCorners = new List<CornerType>();
            PlayerTypes = new List<PlayerType>();
            PlayerColors = new List<ColorType>();
            PlayerNumbers = new List<int>();
        }
    }

    //Описывает игровую доску для создания
    public struct CellDescription
    {
        public int width;
        public int lenght;
        public CellType[,] CellTypes;
    }

    //Описывает набор фигур ОДНОГО игрока
    public class FigureSetDescription
    {
        public List<Position> positions;
        public List<FigureType> figureTypes;

        public FigureSetDescription()
        {
            positions = new List<Position>();
            figureTypes = new List<FigureType>();
        }

        public FigureSetDescription(List<Position> _positions, List<FigureType> _figureTypes)
        {
            positions = _positions;
            figureTypes = _figureTypes;
        }
    }


    public class DefaultFigureSet
    {
        public List<Position> positions;
        public List<FigureType> figureTypes;

        public DefaultFigureSet()
        {
            positions = new List<Position>()
            {
                new Position(0,2),//Ладья
                new Position(1,1),//Конь
                new Position(0,1),//Королева
                new Position(2,0),//Слон
                new Position(0,3),//Пешка
                new Position(1,2),//Пешка
                new Position(2,1),//Пешка
                new Position(3,0),//Пешка
                new Position(1,0),//Король
            };

            figureTypes = new List<FigureType>()
            {
                FigureType.Castle,
                FigureType.Horse,
                FigureType.Queen,
                FigureType.Bishop,
                FigureType.Pawn,
                FigureType.Pawn,
                FigureType.Pawn,
                FigureType.Pawn,
                FigureType.King,
            };
        }
    }

    public class GameDefaultSetDescription 
    {
        int sizeMap;
        PlayerType[] typePlayer = new PlayerType[4];
        CornerType[] cornerPlayer = new CornerType[4];
        ColorType[] colorPlayer = new ColorType[4];

        public GameDefaultSetDescription() 
        {
            sizeMap = 9;
            typePlayer = new PlayerType[4]
                { PlayerType.None, PlayerType.None, PlayerType.None, PlayerType.None};

            cornerPlayer = new CornerType[4]
                { CornerType.DownLeft, CornerType.UpRight, CornerType.DownRight, CornerType.UpLeft};

            colorPlayer = new ColorType[4]
                { ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow};
        }
    }


}