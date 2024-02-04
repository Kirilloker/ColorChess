namespace ColorChessModel
{
    public interface IServerSender
    {
        public void StartGame(Map map);
        public void SendStep(Step step);
        public void EndGame();
    }
}
