using System;
using System.Collections.Generic;
using ColorChessModel;

namespace ColorChessModel
{
    public class Map
    {
        private Cell[,] cells = null;
        private List<Player> players = new List<Player>();
        private Dictionary<int, Dictionary<CellType, int>> score = new Dictionary<int, Dictionary<CellType, int>>();

        // Сколько очков приносит тип клетки
        private readonly int OneScorePaint = 1;
        private readonly int OneScoreDark = 1;

        private int countStep = 1;

        public Map() { }
        public Map(int x, int y)
        {
            cells = new Cell[x, y];
        }

        public Map(Cell[,] _cells, List<Player> _players)
        {
            this.cells = _cells;
            this.players = _players;
        }

        // Test
        public Map(Map anotherMap)
        {
            this.players = new List<Player>();

            for (int i = 0; i < anotherMap.players.Count; i++)
                this.players.Add(new Player(anotherMap.players[i]));

            cells = new Cell[anotherMap.Width, anotherMap.Length];

            for (int i = 0; i < anotherMap.Width; i++)
                for (int j = 0; j < anotherMap.Length; j++)
                    cells[i, j] = new Cell(anotherMap.cells[i, j]);

            foreach (Player player in this.players)
                foreach (Figure figure in player.Figures)
                    this.cells[figure.Pos.X, figure.Pos.Y].Figure = figure;

            this.countStep = anotherMap.countStep;
        }

        // Test
        public void KillFigure(Figure deadFigure)
        {
            foreach (Figure figure in players[deadFigure.Number].Figures)
            {
                if (deadFigure.Equals(figure) == true)
                {
                    players[deadFigure.Number].Figures.Remove(figure);
                    break;
                }
            }
        }


        public int GetScorePlayer(int numberPlayer)
        {
            if (score.Count == 0) 
                return 0;
            else 
                return score[numberPlayer][CellType.Paint] * OneScorePaint +
                       score[numberPlayer][CellType.Dark]  * OneScoreDark;

        }

        public Map ConvertMapToPlayer(int playerNum)
        {
            Map map = new(this);
            for (int i = 0; i < map.players.Count; i++)
            {
                if (i == playerNum) continue;
                map.players[i].Type = PlayerType.Online;
            }
            return map;
        }

        public Cell GetCell(Position posCell) => cells[posCell.X, posCell.Y];
        public Cell GetCell(int x, int y) => cells[x, y];
        public ColorType GetColorTypeCell(int x, int y) => GetColorType(cells[x, y].NumberPlayer);
        public CellType GetCellType(int x, int y) => cells[x, y].Type;
        public PlayerType GetPlayerType(int numberPlayer) => players[numberPlayer].Type;
        public CornerType GetPlayerCorner(int numberPlayer) => players[numberPlayer].Corner;
        public int GetPlayerFiguresCount(int numberPlayer) => players[numberPlayer].Figures.Count;

        public ColorType GetColorType(int numberPlayer)
        {
            if (numberPlayer < 0 || numberPlayer >= PlayersCount)
                return ColorType.Default;
            return players[numberPlayer].Color;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            Map another = obj as Map;
            if (another == null) return false;

            if (another.Length != Length || another.Width != Width) return false;
            if (CountStep != another.CountStep) return false;
            if (PlayersCount != another.PlayersCount) return false;

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Length; j++)
                    if (cells[i, j].Equals(another.cells[i, j]) == false) return false;

            for (int i = 0; i < players.Count; i++)
                if (players[i].Equals(another.players[i]) == false) return false;

            return true;
        }




        public int Width { get { return cells.GetLength(0); } }
        public int Length { get { return cells.GetLength(1); } }
        public int PlayersCount { get { return players.Count; } }
        public int NumberPlayerStep { get { return (countStep - 1) % players.Count; } }
        public bool EndGame { get { return CountEmptyCell == 0; } }
        public Dictionary<int, Dictionary<CellType, int>> Score  { get { return score; } set { score = value; } }

        public int CountEmptyCell { 
            get 
            {
                if (score.ContainsKey(-1) == false) return 100;
                return score[-1][CellType.Empty];
            } 
        }

        // Свойства для сериализации
        public List<Player> Players
        {
            get { return players; }
            set
            {
                this.players = new List<Player>(value.Count);

                foreach (var player in value)
                    this.players.Add(new Player(player));
            }
        }

        public Cell[,] Cells
        {
            get { return cells; }
            set
            {
                cells = new Cell[value.GetLength(0), value.GetLength(1)];

                for (int i = 0; i < value.GetLength(0); i++)
                    for (int j = 0; j < value.GetLength(1); j++)
                        cells[i, j] = new Cell(value[i, j]);

                foreach (Player player in this.players)
                    foreach (Figure figure in player.Figures)
                        this.cells[figure.Pos.X, figure.Pos.Y].Figure = figure;
            }
        }

        public int CountStep
        {
            get { return countStep; }
            set { countStep = value; }
        }

    }}
