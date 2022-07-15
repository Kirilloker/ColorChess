using System.Collections.Generic;
namespace ColorChessModel
{
    public class Map
    {
        public Cell[,] cells = null;
        public List<Player> players = new List<Player>();
        public List<int> scorePlayer = new List<int>();
        public int countStep = 0;

        // Добавить пустые клетки

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

        public Cell GetCell(Position posCell)
        {
            return cells[posCell.X, posCell.Y];
        }

        public Cell GetCell(int x, int y)
        {
            return cells[x, y];
        }

        public ColorType GetColorType(int numberPlayer)
        {
            return players[numberPlayer].color;
        }

        public ColorType GetColorTypeCell(int x, int y)
        {
            return GetColorType(cells[x, y].numberPlayer);
        }

        public CellType GetCellType(int x, int y)
        {
            return cells[x, y].type;
        }

        public int Width { get { return cells.GetLength(0); } }
        public int Length { get { return cells.GetLength(1); } }
        public int numberPlayerStep { get { return (countStep - 1) % players.Count; } }
        public List<Player> PLayers { get { return players; } }

        public Map(Map anotherMap)
        {
            this.players = new List<Player>();

            for (int i = 0; i < anotherMap.players.Count; i++)
            {
                this.players.Add(new Player(anotherMap.players[i]));
            }

            cells = new Cell[anotherMap.Width, anotherMap.Length];

            for (int i = 0; i < anotherMap.Width; i++)
            {
                for (int j = 0; j < anotherMap.Length; j++)
                {
                    cells[i, j] = new Cell(anotherMap.cells[i, j]);
                }
            }

            foreach (Player player in this.players)
            {
                foreach (Figure figure in player.figures)
                {
                    this.cells[figure.pos.X, figure.pos.Y].figure = figure;
                }
            }

            this.countStep = anotherMap.countStep;
        }

        public override string ToString()
        {
            string Logs = "";

            Logs += "Cells:\n";

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    Logs += cells[i, j].ToString() + "\n";
                }
            }

            Logs += "Players:\n";

            for (int i = 0; i < players.Count; i++)
            {
                Logs += players[i].ToString() + "\n";
            }

            return Logs;
        }
    };
}