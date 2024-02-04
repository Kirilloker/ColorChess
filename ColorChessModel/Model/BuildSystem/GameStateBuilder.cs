using System.Collections.Generic;


namespace ColorChessModel
{

    public class GameStateBuilder
    {
        private Map gameState;

        private FigureBuilderDirector figureBuilder;
        private PlayerBuilder playerBuilder;
        private CellBuilder cellBuilder;


        private PlayersDescription playersDiscription;
        private CellDescription board;
        private List<FigureSetDescription> figureSets;

        public GameStateBuilder()
        {
            gameState = new Map();

            figureBuilder = new FigureBuilderDirector();
            playerBuilder = new PlayerBuilder();
            cellBuilder = new CellBuilder();
        }

        public Map CreateGameState()
        {
            //Создаем игроков
            for (int i = 0; i < playersDiscription.PlayerNumbers.Count; i++)
            {
                gameState.Players.Add(playerBuilder.MakePlayer(
                    playersDiscription.PlayerCorners[i],
                    playersDiscription.PlayerColors[i],
                    playersDiscription.PlayerTypes[i],
                    playersDiscription.PlayerNumbers[i]));
            }

            //Создаем фигуры и передаем ссылку на них игрокам
            for (int i = 0; i < figureSets.Count; i++)
            {
                for (int j = 0; j < figureSets[i].positions.Count; j++)
                {
                    gameState.Players[i].Figures.Add(
                        figureBuilder.MakeFigure(
                            figureSets[i].positions[j],
                            gameState.Players[i],
                            figureSets[i].figureTypes[j]));
                }
            }

            // Выделяем память под массив
            Cell[,] cells = new Cell[board.lenght, board.width];
            

            //Создаем игровое поле
            for (int i = 0; i < board.lenght; i++)
            {
                for (int j = 0; j < board.width; j++)
                {
                    cells[i, j] = cellBuilder.MakeCell(
                        new Position(i, j),
                        board.CellTypes[i, j]);
                }
            }

            gameState.Cells = cells;


            //Тут мы обращаемся к GameStateCalculator
            //И обновляем состояния клеток (ставим фигуры на игровое поле)
            GameStateCalcSystem.UpdateGameStateForBuilder(gameState);

            return gameState;
        }

        public void SetCustomGameState(PlayersDescription _playersDiscription, CellDescription _board, List<FigureSetDescription> _figureSets)
        {
            playersDiscription = _playersDiscription;
            board = _board;
            figureSets = _figureSets;
        }

        private void SetDefaultBoard() { SetBoard(9, 9); }
        private void SetBoard(int _lenght, int _width)
        {
            board = new CellDescription();
            board.lenght = _lenght;
            board.width = _width;
            board.CellTypes = new CellType[board.lenght, board.width];

            for (int i = 0; i < board.lenght; i++)
            {
                for (int j = 0; j < board.width; j++)
                {
                    board.CellTypes[i, j] = CellType.Empty;
                }
            }
        }

        private FigureSetDescription ConvertFigurePosToCorner(FigureSetDescription figureSet, CornerType corner)
        {
            for (int i = 0; i < figureSet.positions.Count; i++)
            {
                switch (corner)
                {
                    case CornerType.UpLeft:
                        figureSet.positions[i].Y = board.lenght - 1 - figureSet.positions[i].Y;
                        break;
                    case CornerType.UpRight:
                        figureSet.positions[i].X = board.lenght - 1 - figureSet.positions[i].X;
                        figureSet.positions[i].Y = board.lenght - 1 - figureSet.positions[i].Y;
                        break;
                    case CornerType.DownLeft:
                        //Считаем стандратным, зеркалим относително него
                        break;
                    case CornerType.DownRight:
                        figureSet.positions[i].X = board.lenght - 1 - figureSet.positions[i].X;
                        break;
                    default:
                        break;
                }
            }

            return figureSet;
        }

        private void SetDefaultFigureSets()
        {
            figureSets = new List<FigureSetDescription>();

            for (int i = 0; i < playersDiscription.PlayerNumbers.Count; i++)
            {
                DefaultFigureSet defaultFigSet = new DefaultFigureSet();
                figureSets.Add(
                    ConvertFigurePosToCorner(
                        new FigureSetDescription(defaultFigSet.positions, defaultFigSet.figureTypes),
                        playersDiscription.PlayerCorners[i]));
            }
        }

        public void SetDefaultHotSeatGameState()
        {
            playersDiscription = new PlayersDescription();

            playersDiscription.PlayerNumbers.Add(0);
            playersDiscription.PlayerTypes.Add(PlayerType.Human);
            playersDiscription.PlayerCorners.Add(CornerType.DownLeft);
            playersDiscription.PlayerColors.Add(ColorType.Blue);

            playersDiscription.PlayerNumbers.Add(1);
            playersDiscription.PlayerTypes.Add(PlayerType.Human);
            playersDiscription.PlayerCorners.Add(CornerType.UpRight);
            playersDiscription.PlayerColors.Add(ColorType.Red);

            SetDefaultBoard();
            SetDefaultFigureSets();
        }

        public void SetDefaultAIGameState()
        {
            playersDiscription = new PlayersDescription();

            playersDiscription.PlayerNumbers.Add(0);
            playersDiscription.PlayerTypes.Add(PlayerType.Human);
            playersDiscription.PlayerCorners.Add(CornerType.DownLeft);
            playersDiscription.PlayerColors.Add(ColorType.Blue);

            playersDiscription.PlayerNumbers.Add(1);
            playersDiscription.PlayerTypes.Add(PlayerType.AI);
            playersDiscription.PlayerCorners.Add(CornerType.UpRight);
            playersDiscription.PlayerColors.Add(ColorType.Red);


            SetDefaultBoard();
            SetDefaultFigureSets();
        }

        public void SetDefaultOnlineGameState()
        {
            playersDiscription = new PlayersDescription();

            playersDiscription.PlayerNumbers.Add(0);
            playersDiscription.PlayerTypes.Add(PlayerType.Human);
            playersDiscription.PlayerCorners.Add(CornerType.DownLeft);
            playersDiscription.PlayerColors.Add(ColorType.Blue);

            playersDiscription.PlayerNumbers.Add(1);
            playersDiscription.PlayerTypes.Add(PlayerType.Online);
            playersDiscription.PlayerCorners.Add(CornerType.UpRight);
            playersDiscription.PlayerColors.Add(ColorType.Red);

            SetDefaultBoard();
            SetDefaultFigureSets();
        }

        public void SetCustomGameState(int sizeMap, PlayerType[] typePlayer, CornerType[] cornerPlayer, ColorType[] colorPlayer) 
        {
            playersDiscription = new PlayersDescription();

            for (int i = 0; i < typePlayer.Length; i++)
            {
                if (typePlayer[i] == PlayerType.None) continue;
                playersDiscription.PlayerNumbers.Add(playersDiscription.PlayerNumbers.Count);
                playersDiscription.PlayerTypes.Add(typePlayer[i]);
                playersDiscription.PlayerCorners.Add(cornerPlayer[i]);
                playersDiscription.PlayerColors.Add(colorPlayer[i]);
            }

            SetBoard(sizeMap, sizeMap);
            SetDefaultFigureSets();
        }

    };
}