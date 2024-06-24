using ColorChessModel;
using GameServer.Database;
using GameServer.Enum;

namespace GameServer.GameServer.GameServerModel.Room
{
    public class RatingGameRoom : GameRoom
    {
        private readonly Dictionary<int, int> _playersRate = new();
        private const int _ratingBet = 25;

        public RatingGameRoom(int maxNumOfPlayers, List<int> playersIds, GameModeType gameMode)
            : base(maxNumOfPlayers, playersIds, gameMode) { }

        public override void AddPlayer(int PlayerId)
        {
            base.AddPlayer(PlayerId);

            UserStatistic? userStatistic = DB.GetUserStatistic(PlayerId) ??
                throw new Exception("Not found UserStatistic when add player Rating Mode.");

            _playersRate[PlayerId] = userStatistic.Rate;
        }

        public override void RemovePlayer(int playerId)
        {
            base.RemovePlayer(playerId);

            _playersRate.Remove(playerId);
        }

        public override void FinishTheGame()
        {
            List<int> playersScore = _gameState.GetListScorePlayer();

            AddGameStatistic(playersScore, _gameMode, _playersId);
            ChangeUserStatistic(playersScore, _playersId);

            ChangeRateValue(playersScore, _playersId, _ratingBet);
        }

        public override void PlayerLeaveEndGame(int playerId)
        {
            base.PlayerLeaveEndGame(playerId);

            DB.ChangeUserStatistic(playerId, AttributeUS.Rate, -_ratingBet);

            for (int i = 0; i < _playersId.Count; i++)
            {
                if (_playersId[i] != playerId)
                {
                    if (_maxNumOfPlayers == 2)
                    {
                        DB.ChangeUserStatistic(_playersId[i], AttributeUS.Win, 1);
                        DB.ChangeUserStatistic(_playersId[i], AttributeUS.Rate, _ratingBet);
                    }
                    else
                    {
                        DB.ChangeUserStatistic(_playersId[i], AttributeUS.Rate, 10);
                    }
                }
            }

            List<int> playersScore = _gameState.GetListScorePlayer();

            AddGameStatistic(playersScore, _gameMode, _playersId);
        }


        private void ChangeRateValue(List<int> playersScore, List<int> playersId, int ratingBet)
        {
            var gameResults = CalculateGameResult(playersScore);

            for (int i = 0; i < playersId.Count; i++)
            {
                if (gameResults[i] == AttributeUS.Win) DB.ChangeUserStatistic(playersId[i], AttributeUS.Rate, ratingBet);
                if (gameResults[i] == AttributeUS.Lose) DB.ChangeUserStatistic(playersId[i], AttributeUS.Rate, -ratingBet);
            }
        }



        public int AverageRating => (int)_playersRate.Values.DefaultIfEmpty(0).Average();

    }
}