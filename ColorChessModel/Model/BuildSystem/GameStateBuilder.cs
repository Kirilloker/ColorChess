using System.Collections.Generic;


namespace ColorChessModel
{

    public class GameStateBuilder
    {
        private Map gameState;

        private FigureBuilderDirector figureBuilder;
        private PlayerBuilder playerBuilder;
        private CellBuilder cellBuilder;


        private PlayersDescription playersDescription;
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
            gameState = new Map();

            //Создаем игроков
            for (int i = 0; i < playersDescription.PlayerNumbers.Count; i++)
            {
                gameState.Players.Add(playerBuilder.MakePlayer(
                    playersDescription.PlayerCorners[i],
                    playersDescription.PlayerColors[i],
                    playersDescription.PlayerTypes[i],
                    playersDescription.PlayerNumbers[i]));
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
            Cell[,] cells = new Cell[board.length, board.width];
            

            //Создаем игровое поле
            for (int i = 0; i < board.length; i++)
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

        public void SetCustomGameState(PlayersDescription _playersDescription, CellDescription _board, List<FigureSetDescription> _figureSets)
        {
            playersDescription = _playersDescription;
            board = _board;
            figureSets = _figureSets;
        }

        private void SetDefaultBoard() { SetBoard(9, 9); }
        private void SetBoard(int _length, int _width)
        {
            board = new CellDescription();
            board.length = _length;
            board.width = _width;
            board.CellTypes = new CellType[board.length, board.width];

            for (int i = 0; i < board.length; i++)
                for (int j = 0; j < board.width; j++)
                    board.CellTypes[i, j] = CellType.Empty;
        }

        private FigureSetDescription ConvertFigurePosToCorner(FigureSetDescription figureSet, CornerType corner)
        {
            for (int i = 0; i < figureSet.positions.Count; i++)
            {
                switch (corner)
                {
                    case CornerType.UpLeft:
                        figureSet.positions[i].Y = board.length - 1 - figureSet.positions[i].Y;
                        break;
                    case CornerType.UpRight:
                        figureSet.positions[i].X = board.length - 1 - figureSet.positions[i].X;
                        figureSet.positions[i].Y = board.length - 1 - figureSet.positions[i].Y;
                        break;
                    case CornerType.DownLeft:
                        //Считаем стандартным, зеркалим относительно него
                        break;
                    case CornerType.DownRight:
                        figureSet.positions[i].X = board.length - 1 - figureSet.positions[i].X;
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

            for (int i = 0; i < playersDescription.PlayerNumbers.Count; i++)
            {
                DefaultFigureSet defaultFigSet = new DefaultFigureSet();
                figureSets.Add(
                    ConvertFigurePosToCorner(
                        new FigureSetDescription(defaultFigSet.positions, defaultFigSet.figureTypes),
                        playersDescription.PlayerCorners[i]));
            }
        }

        public void SetDefaultHotSeatGameState()
        {
            playersDescription = new PlayersDescription();

            playersDescription.PlayerNumbers.Add(0);
            playersDescription.PlayerTypes.Add(PlayerType.Human);
            playersDescription.PlayerCorners.Add(CornerType.DownLeft);
            playersDescription.PlayerColors.Add(ColorType.Blue);

            playersDescription.PlayerNumbers.Add(1);
            playersDescription.PlayerTypes.Add(PlayerType.Human);
            playersDescription.PlayerCorners.Add(CornerType.UpRight);
            playersDescription.PlayerColors.Add(ColorType.Red);

            SetDefaultBoard();
            SetDefaultFigureSets();
        }

        public void SetDefaultAIGameState()
        {
            playersDescription = new PlayersDescription();

            playersDescription.PlayerNumbers.Add(0);
            playersDescription.PlayerTypes.Add(PlayerType.Human);
            playersDescription.PlayerCorners.Add(CornerType.DownLeft);
            playersDescription.PlayerColors.Add(ColorType.Blue);

            playersDescription.PlayerNumbers.Add(1);
            playersDescription.PlayerTypes.Add(PlayerType.AI);
            playersDescription.PlayerCorners.Add(CornerType.UpRight);
            playersDescription.PlayerColors.Add(ColorType.Red);


            SetDefaultBoard();
            SetDefaultFigureSets();
        }

        public void SetDefaultOnlineGameState()
        {
            playersDescription = new PlayersDescription();

            playersDescription.PlayerNumbers.Add(0);
            playersDescription.PlayerTypes.Add(PlayerType.Human);
            playersDescription.PlayerCorners.Add(CornerType.DownLeft);
            playersDescription.PlayerColors.Add(ColorType.Blue);

            playersDescription.PlayerNumbers.Add(1);
            playersDescription.PlayerTypes.Add(PlayerType.Online);
            playersDescription.PlayerCorners.Add(CornerType.UpRight);
            playersDescription.PlayerColors.Add(ColorType.Red);

            SetDefaultBoard();
            SetDefaultFigureSets();
        }

        public void SetCustomGameState(int sizeMap, PlayerType[] typePlayer, CornerType[] cornerPlayer, ColorType[] colorPlayer) 
        {
            playersDescription = new PlayersDescription();

            for (int i = 0; i < typePlayer.Length; i++)
            {
                if (typePlayer[i] == PlayerType.None) continue;
                playersDescription.PlayerNumbers.Add(playersDescription.PlayerNumbers.Count);
                playersDescription.PlayerTypes.Add(typePlayer[i]);
                playersDescription.PlayerCorners.Add(cornerPlayer[i]);
                playersDescription.PlayerColors.Add(colorPlayer[i]);
            }

            SetBoard(sizeMap, sizeMap);
            SetDefaultFigureSets();
        }

        public void ClearData() 
        {
            playersDescription = new PlayersDescription();
        }

    };
}