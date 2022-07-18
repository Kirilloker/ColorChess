using System;
using System.Collections.Generic;


namespace ColorChessModel
{
    public class Map
    {
        public Cell[,] cells = null;
        public List<Player> players = new List<Player>();
        public Dictionary<int, Dictionary<CellType, int>> score = new Dictionary<int, Dictionary<CellType, int>>();
        public int countStep = 1;

        // Сколько очков приносит тип клетки
        private int OneScorePaint = 1;
        private int OneScoreDark = 1;


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

        public void KillFigure(Figure deadFigure)
        {
            foreach (Figure figure in players[deadFigure.Number].figures)
            {
                if (deadFigure.equals(figure) == true)
                {
                    players[deadFigure.Number].figures.Remove(figure);
                    break;
                }
            }
        }

        public int GetScorePlayer(int numberPlayer)
        {
            return score[numberPlayer][CellType.Paint] * OneScorePaint +
                score[numberPlayer][CellType.Dark] * OneScoreDark;
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
        public int numberPlayerStep { 
            get 
            {
                
                return (countStep - 1) % players.Count; 
            } 
        }
        public List<Player> PLayers { get { return players; } }

        public int CountEmptyCell { 
            get 
            {
                if (score.ContainsKey(-1) == false) return 100;
                return score[-1][CellType.Empty];
            } 
        }
        public bool EndGame { get { return CountEmptyCell == 0; } }
        public Dictionary<int, Dictionary<CellType, int>> Score  { get { return score; } }

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

        public static bool operator !=(Map map1, Map map2)
        {
            return !(map1 == map2);
        }

        public static bool operator ==(Map map1, Map map2)
        {
            for (int i = 0; i < map1.players.Count; i++)
            {
                for (int j = 0;  j < map1.players[i].figures.Count;  j++)
                {
                    if (map1.players[i].figures[j].equals(map2.players[i].figures[j]) == false)
                    {
                        return false;
                    }
                }
            }

            for (int i = 0; i < map1.Length; i++)
            {
                for (int j = 0; j < map1.Width; j++)
                {
                    if (map1.cells[i, j] != map2.cells[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public uint GetHash()
        {
            uint hash = 0;

            int param1 = 0;
            int param2 = 1;
            int param3 = 1;
            int param4 = 1;
            int param5 = 0;
            int param6 = 1;
            int param7 = 1;
            int param8 = 17;
            int param9 = 256;
            int param10 = 128;
            int param11 = 1;
            int param12 = 32;






            foreach (var player in score)
            {
                foreach (var cell in player.Value)
                {
                    if (cell.Key == CellType.Paint)
                    {
                        hash += (uint)MathF.Pow((cell.Value + param1) * param2, (player.Key + param3) * param4);
                    }
                    else if (cell.Key == CellType.Dark)
                    {
                        hash += (uint)MathF.Pow((cell.Value + param5) * param6, (player.Key + param7) * param8);
                    }
                }
            }
            
            foreach (var player in players)
            {
                foreach (var figure in player.figures)
                {
                    hash += (uint)(((int)figure.type + param9) * param10 * (figure.pos.X + param11) * (figure.pos.X + param12) * 10000 * figure.pos.X);
                }
            }

            return hash;
        }
    };
}


/*

public uint GetHash()
        {
            uint hash = 0;


            foreach (var player in score)
            {
                foreach (var cell in player.Value)
                {
                    if (cell.Key == CellType.Paint)
                    {
                        hash += (uint)MathF.Pow(cell.Value, (player.Key + 1));
                    }
                    else if (cell.Key == CellType.Dark)
                    {
                        hash += (uint)MathF.Pow(cell.Value, (player.Key + 1) * 17);
                    }
                }
            }

            foreach (var player in players)
            {
                foreach (var figure in player.figures)
                {
                    hash += (uint)(((int)figure.type + 256) * 128 * (figure.pos.X + 1) * (figure.pos.X + 32));
                }
            }

            return hash;
        }










        public uint GetHash()
        {
            uint hash = 0;

            int param1 = 1;
            int param2 = 2;
            int param3 = 4;
            int param4 = 8;
            int param5 = 16;
            int param6 = 32;
            int param7 = 64;
            int param8 = 128;
            int param9 = 256;
            int param10 = 512;
            int param11 = 1024;
            int param12 = 2048;






            foreach (var player in score)
            {
                foreach (var cell in player.Value)
                {
                    if (cell.Key == CellType.Paint)
                    {
                        hash += (uint)MathF.Pow((cell.Value + param1) * param2, (player.Key + param3) * param4);
                    }
                    else if (cell.Key == CellType.Dark)
                    {
                        hash += (uint)MathF.Pow((cell.Value + param5) * param6, (player.Key + param7) * param8);
                    }
                }
            }

            foreach (var player in players)
            {
                foreach (var figure in player.figures)
                {
                    hash += (uint)(((int)figure.type + param9) * param10 * (figure.pos.X + param11) * (figure.pos.X + param12));
                }
            }

            return hash;
        }
 */