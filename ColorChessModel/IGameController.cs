namespace ColorChessModel
{
    public interface IGameController
    {
        public void PreparingForStartGame(Map map);
        public void PreparingForNewStep(Map map);
        public void DrawNewGameState(Map previousMap, Map currentMap);
        public void EndGame();
        public void EatFigure(Figure figure, Map map);
        public void PreparingForEndStep(Figure figure, Map map, List<Cell> way);
        public void HumanPlayerStartStep(Map map);
        public void FigureSelected(Map map, Figure figure);
        public void AICalcComplete(Figure figure, Map map);

        public Position GetPositionSelectedFigure();
    }
}
