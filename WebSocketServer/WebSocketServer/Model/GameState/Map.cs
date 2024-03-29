﻿using System;
using System.Collections.Generic;


namespace ColorChessModel
{
    public class Map
    {
        private Cell[,] cells = null;
        private List<Player> players = new List<Player>();
        private int countStep = 1;

        // Сколько очков приносит тип клетки
        private int OneScorePaint = 1;
        private int OneScoreDark = 1;

        public Dictionary<int, Dictionary<CellType, int>> score = new Dictionary<int, Dictionary<CellType, int>>();

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

        public Map ConvertMapToPlayer(int playerNum)
        {
            Map map = new Map(this);
            for(int i = 0; i < map.players.Count; i++)
            {
                if (i == playerNum) continue;
                map.players[i].type = PlayerType.Online;
            }
            return map;
        }

        public void KillFigure(Figure deadFigure)
        {
            foreach (Figure figure in players[deadFigure.Number].figures)
            {
                if (deadFigure.Equals(figure) == true)
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
            if (numberPlayer < 0 || numberPlayer >= PlayersCount)
                return ColorType.Default;
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

        public PlayerType GetPlayerType(int numberPlayer)
        {
            return players[numberPlayer].type;
        }
        public CornerType GetPlayerCorner(int numberPlayer)
        {
            return players[numberPlayer].corner;
        }
        public int GetPlayerFiguresCount(int numberPlayer)
        {
            return players[numberPlayer].figures.Count;
        }

        public int Width { get { return cells.GetLength(0); } }
        public int Length { get { return cells.GetLength(1); } }
        public int NumberPlayerStep { 
            get 
            {
                return (countStep - 1) % players.Count; 
            } 
        }
        public int PlayersCount { get { return players.Count; } }
        public int CountEmptyCell { 
            get 
            {
                if (score.ContainsKey(-1) == false) return 100;
                return score[-1][CellType.Empty];
            } 
        }
        public bool EndGame { get { return CountEmptyCell == 0; } }
        public Dictionary<int, Dictionary<CellType, int>> Score  { get { return score; } }



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
            if (map1.CountStep != map2.CountStep)
                return false;
            // Не полностью готовый оператор 
            if (map1.CountEmptyCell != map2.CountEmptyCell)
                return false;

            if (map1.PlayersCount != map2.PlayersCount)
                return false;

            for (int i = 0; i < map1.PlayersCount; i++)
            {
                if (map1.players[i].figures.Count != map2.players[i].figures.Count)
                    return false;
            }


            for (int i = 0; i < map1.players.Count; i++)
            {
                for (int j = 0;  j < map1.players[i].figures.Count;  j++)
                {
                    if (map1.players[i].figures[j].Equals(map2.players[i].figures[j]) == false)
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
        public string GetStringForHash()
        {
            string stringForHash = "";

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    stringForHash += cells[i, j].GetStringForHash();
                }
            }

            for (int i = 0; i < PlayersCount; i++)
            {
                stringForHash += players[i].GetStringForHash();
            }

            return stringForHash;
        }

        // Свойства для сериализицаии

        public List<Player> Players
        {
            get { return players; }
            set
            {
                this.players = new List<Player>(value.Count);

                foreach (var player in value)
                {
                    this.players.Add(new Player(player));
                }

            }
        }

        public Cell[,] Cells
        {
            get { return cells; }
            set
            {
                cells = new Cell[value.GetLength(0), value.GetLength(1)];

                for (int i = 0; i < value.GetLength(0); i++)
                {
                    for (int j = 0; j < value.GetLength(1); j++)
                    {
                        cells[i, j] = new Cell(value[i, j]);
                    }
                }

                foreach (Player player in this.players)
                {
                    foreach (Figure figure in player.figures)
                    {
                        this.cells[figure.pos.X, figure.pos.Y].figure = figure;
                    }
                }

            }
        }

        public int CountStep
        {
            get { return countStep; }
            set { countStep = value; }
        }

    };
}
