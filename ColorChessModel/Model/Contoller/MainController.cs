using System.Numerics;
using ColorChessModel;

namespace ColorChessModel
{
    public class MainController
    {
        private List<Map> gameStates = new List<Map>();
        private GameStateBuilder gameStateBuilder = new GameStateBuilder();
        private IAI[] ai;
        private IGameController gameController;
        private Server server;

        public void SetGameController(IGameController gameController)
        {
            this.gameController = gameController;
        }

        public void StartGame()
        {
            StartGame(gameStateBuilder.CreateGameState());
        }

        public void StartGame(Map map)
        {
            server = Server.Instance;
            gameStates.Add(map);

            gameController.PreparingForStartGame(CurrentGameState);

            InitAI();
            StartNewStep();
        }

        public void StartNewStep()
        {
            if (CurrentGameState.EndGame == true)
            {
                EndGame();
                return;
            }

            gameController.PreparingForNewStep(CurrentGameState);

            PlayerType playerType = CurrentGameState.GetPlayerType(CurrentGameState.NumberPlayerStep);

            switch (playerType)
            {
                case PlayerType.Human:
                    gameController.HumanPlayerStartStep(CurrentGameState);
                    break;
                case PlayerType.AI:
                    AIStep(playerType);
                    break;
                case PlayerType.AI2:
                    AIStep(playerType);
                    break;
                case PlayerType.Online:
                    break;
            }
        }

        public void CellSelected(Position pos)
        {
            Cell cell = CurrentGameState.GetCell(pos);
            Figure figure = CurrentGameState.GetCell(gameController.GetPositionSelectedFigure()).Figure;
            Step step = new Step(figure, cell);

            ApplyStepView(step);
        }

        public void FigureSelected(Position pos)
        {
            // Получаем фигуру по которой нажали, считаем для неё все возможные пути
            Figure selectFigure = CurrentGameState.GetCell(pos).Figure;

            if (selectFigure != null)
                gameController.FigureSelected(CurrentGameState, selectFigure);
            else
                Print.Log("Такой фигуры не нашлось");
        }


        private async void AIStep(PlayerType AIType)
        {
            Step step = new();

            await Task.Run(() =>
            {
                step = ai[CurrentGameState.NumberPlayerStep].getStep(CurrentGameState);
            });

            if (gameStates.Count == 0) return;

            gameController.AICalcComplete(step.Figure, CurrentGameState);
            ApplyStepView(step);
        }

        public void ApplyStepView(Step step)
        {
            Figure figure = step.Figure;
            Cell cell = step.Cell;

            List<Cell> way = WayCalcSystem.CalcWay(CurrentGameState, figure.Pos, cell.Pos, figure);

            if (IsServer && CurrentGameState.Players[CurrentGameState.NumberPlayerStep].Type == PlayerType.Human)
            {
                if (CurrentGameState.EndGame == true)
                    server.SendLastStep(step);
                else
                    server.SendStep(step);
            }

            Map map = GameStateCalcSystem.ApplyStep(CurrentGameState, figure, cell);
            gameStates.Add(map);

            // В клетке стоит фигура -> её хотят съесть
            if (cell.Figure != null)
                gameController.EatFigure(cell.Figure, CurrentGameState);

            gameController.PreparingForEndStep(figure, map, way);

            gameController.DrawNewGameState(PreviousGameState, CurrentGameState);
            StartNewStep();
        }
        public void SelectGameMode(GameModeType gameMode)
        {
            switch (gameMode)
            {
                case GameModeType.HumanTwo:
                    gameStateBuilder.SetDefaultHotSeatGameState();
                    break;
                case GameModeType.HumanFour:
                    //
                    break;
                case GameModeType.AI:
                    gameStateBuilder.SetDefaultAIGameState();
                    break;
                case GameModeType.Network:
                    gameStateBuilder.SetDefaultOnlineGameState();
                    break;
                default:
                    break;
            }

        }

        public void SelectCustomGameMode(int sizeMap, PlayerType[] typePlayer, CornerType[] cornerPlayer, ColorType[] colorPlayer)
        {
            gameStateBuilder.SetCustomGameState(sizeMap, typePlayer, cornerPlayer, colorPlayer);
        }

        public void EndGame()
        {
            if (IsServer == true)
            {
                //uiController.OnlineGameExit();
                server.CloseConnection();
            }

            Print.Log("Конец игры");

            gameController.EndGame();

            gameStateBuilder = new GameStateBuilder();
            gameStates = new List<Map>();
        }

        void InitAI()
        {
            ai = new IAI[CurrentGameState.PlayersCount];

            for (int i = 0; i < CurrentGameState.PlayersCount; i++)
            {
                if (CurrentGameState.GetPlayerType(i) == PlayerType.AI)
                    ai[i] = new MinMaxAI();
                else if (CurrentGameState.GetPlayerType(i) == PlayerType.AI2)
                    ai[i] = new MonteCarloAI();
            }
        }


        public bool GetBoolFigureInCell(Position position)
        {
            return CurrentGameState.GetCell(position).Figure != null;
        }

        public Map PreviousGameState { get { return gameStates[gameStates.Count - 2]; } }
        public Map CurrentGameState { get { return gameStates[gameStates.Count - 1]; } }
        private bool IsServer
        {
            get
            {
                foreach (var player in CurrentGameState.Players)
                    if (player.Type == PlayerType.Online) return true;

                return false;
            }
        }

        private static MainController instance;
        private static readonly object lockObject = new object();

        private MainController()
        {
        }

        public static MainController Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new MainController();
                    }
                }
                return instance;
            }
        }
    }
}
