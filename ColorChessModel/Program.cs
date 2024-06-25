using ColorChessModel;
using ColorChessModel.Tools;

class TestClass
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World");

        MainController mainController = MainController.Instance;
        testGameController gameController = new();

        mainController.SelectGameMode(GameModeType.AI);
        mainController.SetGameController(gameController);
        mainController.StartGame();


        Console.WriteLine(mainController.CurrentGameState.Players[0].Type.ToString());
        Console.WriteLine(mainController.CurrentGameState.Players[1].Type.ToString());

        Map map1 = mainController.CurrentGameState;

        Figure figure1 = map1.GetCell(new Position(3, 0)).Figure; 
        Cell cell1 = map1.GetCell(new Position(4, 1));

        Step step1 = new(figure1, cell1);

        Printer.PrintMap(map1);

        mainController.ApplyStepView(step1);





        Map map2 = mainController.CurrentGameState;

        Printer.PrintMap(map2);

        Figure figure2 = map2.GetCell(new Position(0, 3)).Figure;
        Cell cell2 = map2.GetCell(new Position(1, 4));

        Step step2 = new(figure2, cell2);

        mainController.ApplyStepView(step2);



        Map map3 = mainController.CurrentGameState;

        Printer.PrintMap(map3);

        Figure figure3 = map3.GetCell(new Position(1, 0)).Figure;
        Cell cell3 = map3.GetCell(new Position(4, 4));

        Step step3 = new(figure3, cell3);

        mainController.ApplyStepView(step3);



        Map map4 = mainController.CurrentGameState;

        Printer.PrintMap(map4);

        Figure figure4 = map4.GetCell(new Position(4, 4)).Figure;
        Cell cell4 = map4.GetCell(new Position(0, 0));

        Step step4 = new(figure4, cell4);

        mainController.ApplyStepView(step4);


        Map map5 = mainController.CurrentGameState;

        Printer.PrintMap(map5);

        Figure figure5 = map5.GetCell(new Position(0, 0)).Figure;
        Cell cell5 = map5.GetCell(new Position(2, 2));

        Step step5 = new(figure5, cell5);

        mainController.ApplyStepView(step5);



        Map map6 = mainController.CurrentGameState;

        Printer.PrintMap(map6);

        Figure figure6 = map6.GetCell(new Position(2, 0)).Figure;
        Cell cell6 = map6.GetCell(new Position(3, 1));

        Step step6 = new(figure6, cell6);

        mainController.ApplyStepView(step6);





        Map map7 = mainController.CurrentGameState;

        Printer.PrintMap(map7);

        Figure figure7 = map7.GetCell(new Position(2, 1)).Figure;
        Cell cell7 = map7.GetCell(new Position(3, 1));

        Step step7 = new(figure7, cell7);


        TEST.test = true;

        mainController.ApplyStepView(step7);


        Map map8 = mainController.CurrentGameState;

        Printer.PrintMap(map8);
    }


    public class testGameController : IGameController
    {
        public void AICalcComplete(Figure figure, Map map)
        {
        }

        public void DrawNewGameState(Map previousMap, Map currentMap)
        {
        }

        public void EatFigure(Figure figure, Map map)
        {
        }

        public void EndGame()
        {
        }

        public void FigureSelected(Map map, Figure figure)
        {
        }

        public Position GetPositionSelectedFigure()
        {
            return new Position();
        }

        public void HumanPlayerStartStep(Map map)
        {
        }

        public void PreparingForEndStep(Figure figure, Map map, List<Cell> way)
        {
        }

        public void PreparingForNewStep(Map map)
        {
        }

        public void PreparingForStartGame(Map map)
        {
        }
    }
}

public static class TEST
{
    public static bool test = false;
}
