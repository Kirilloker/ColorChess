using System.Collections.Generic;
namespace ColorChessModel
{
    //Описывает всех игроков для создания
    public class PlayersDiscription
    {
        public List<CornerType> PlayerCorners;
        public List<PlayerType> PlayerTypes;
        public List<ColorType> PlayerColors;
        public List<int> PlayerNumbers;
        public PlayersDiscription()
        {
            PlayerCorners = new List<CornerType>();
            PlayerTypes = new List<PlayerType>();
            PlayerColors = new List<ColorType>();
            PlayerNumbers = new List<int>();
        }
    }

    //Описывает игровую доску для создания
    public struct CellDiscription
    {
        public int width;
        public int lenght;
        public CellType[,] CellTypes;
    }

    //Описывает набор фигур ОДНОГО игрока
    public class FigureSetDiscription
    {
        public List<Position> positions;
        public List<FigureType> figureTypes;

        public FigureSetDiscription()
        {
            positions = new List<Position>();
            figureTypes = new List<FigureType>();
        }

        public FigureSetDiscription(List<Position> _positions, List<FigureType> _figureTypes)
        {
            positions = _positions;
            figureTypes = _figureTypes;
        }
    }



    //Стандартный набор фигур, позиции указаны для нижнего левого угла,
    //остальные конвертируем
    //public class DefaultFigureSet
    //{
    //    public List<Position> positions;
    //    public List<FigureType> figureTypes;

    //    public DefaultFigureSet()
    //    {
    //        positions = new List<Position>()
    //    {
    //        new Position(0,3),//Пешка
    //        new Position(1,2),//Пешка
    //        new Position(2,1),//Пешка
    //        new Position(3,0),//Пешка
    //        new Position(0,2),//Ладья
    //        new Position(0,1),//Королева
    //        new Position(1,1),//Конь
    //        new Position(1,0),//Король
    //        new Position(2,0),//Слон
    //    };

    //        figureTypes = new List<FigureType>()
    //    {
    //        FigureType.Pawn,
    //        FigureType.Pawn,
    //        FigureType.Pawn,
    //        FigureType.Pawn,
    //        FigureType.Castle,
    //        FigureType.Queen,
    //        FigureType.Horse,
    //        FigureType.King,
    //        FigureType.Bishop
    //    };
    //    }
    //}

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


}