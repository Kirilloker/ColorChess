using ColorChessModel;
using GameServer.Database;
using GameServer.GameServer.GameServerModel.Room;

namespace GameServer.GameServer.GameServerModel
{
    public static class GameLobby
    {
        //Rooms: GameMode -> MaxPlayersInRoom -> Rooms   
        private static readonly Dictionary<GameModeType, Dictionary<int, List<GameRoom>>> _rooms = new();
        private static readonly Dictionary<int, GameRoom> _playersInRooms = new();

        private static readonly object locker = new();

        public static bool TryFindRoom(int playerId, GameModeType gameMode, int numOfPlayers, out Map? map)
        {
            lock (locker)
            {
                map = null;

                if (_playersInRooms.ContainsKey(playerId))
                    return false;

                var room = GetRelevanceRoom(gameMode, numOfPlayers, playerId);

                _playersInRooms[playerId] = room;
                room.AddPlayer(playerId);

                if (room.IsFull)
                {
                    map = room.InitMap();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void ApplyStep(int playerId, string step)
        {
            if (_playersInRooms.TryGetValue(playerId, out GameRoom? room) == false)
                throw new Exception("Not found room when trying to apply step");

            bool isCorrectStep = room.TryApplyStep(JSONConverter.ConvertJSONtoSTEP(step));

            if (isCorrectStep == false || room.EndGame == true)
            {
                room.FinishTheGame();
                RemoveRoom(room);

                AddLog(LogEventType.EndGame, GetAllPlayersInRoomByPlayerId(playerId), "end game");
            }
        }

        public static void PlayerLeftTheGame(int playerId)
        {
            if (_playersInRooms.TryGetValue(playerId, out GameRoom? room) == false)
                return;

            RemoveRoom(room);
        }

        public static List<int> GetAllPlayersInRoomByPlayerId(int playerId)
        {
            if (_playersInRooms.TryGetValue(playerId, out GameRoom? room))
                return room.PlayersIdInRoom;
            else
                return new();
        }


        public static int GetCountPlayersInGame() => _playersInRooms.Count;



        private static GameRoom GetRelevanceRoom(GameModeType gameMode, int numOfPlayers, int playerId)
        {
            GameRoom room;
            var listOfRelevanceRooms = GetListOfRelevanceRooms(gameMode, numOfPlayers);

            if (listOfRelevanceRooms.Count == 0)
            {
                GameRoom newRoom = CreateNewRoom(gameMode, numOfPlayers);
                _rooms[gameMode][numOfPlayers].Add(newRoom);
            }

            if (gameMode == GameModeType.Default)
            {
                room = FindDefaultRelevanceRoom(listOfRelevanceRooms);
            }
            else if (gameMode == GameModeType.Rating)
            {
                UserStatistic? userStatistic = DB.GetUserStatistic(playerId) ?? throw new Exception("Not found user statistics");
                room = FindRatingRelevanceRoom(listOfRelevanceRooms, userStatistic.Rate);
            }
            else
            {
                throw new Exception("Unknown game mode");
            }

            return room;
        }

        private static GameRoom CreateNewRoom(GameModeType gameMode, int numOfPlayers) => gameMode switch
        {
            GameModeType.Default => new GameRoom(numOfPlayers, new List<int>() { }, gameMode),
            GameModeType.Rating => new RatingGameRoom(numOfPlayers, new List<int>() { }, gameMode),
            _ => throw new Exception("Unknown game mode")
        };

        private static List<GameRoom> GetListOfRelevanceRooms(GameModeType gameMode, int numOfPlayers)
        {
            if (_rooms.TryGetValue(gameMode, out var gameModeDict) == false)
            {
                _rooms[gameMode] = new Dictionary<int, List<GameRoom>>();
                gameModeDict = _rooms[gameMode];
            }

            if (gameModeDict.TryGetValue(numOfPlayers, out var relevanceRoomList) == false)
            {
                _rooms[gameMode][numOfPlayers] = new List<GameRoom>();
                relevanceRoomList = _rooms[gameMode][numOfPlayers];
            }

            return relevanceRoomList;
        }

        private static GameRoom FindRatingRelevanceRoom(List<GameRoom> relevanceRoomList, int rate)
        {
            int bestRateDelta = int.MaxValue;
            int rateDelta;
            GameRoom? relevanceRoom = null;

            foreach (RatingGameRoom room in relevanceRoomList.Cast<RatingGameRoom>())
            {
                if (room.IsFull)
                    continue;

                rateDelta = Math.Abs(rate - room.AverageRating);

                if (rateDelta < bestRateDelta)
                {
                    relevanceRoom = room;
                    bestRateDelta = rateDelta;
                }
            }

            if (relevanceRoom == null)
                throw new Exception("Not found relevance room from Rating GameMode");

            return relevanceRoom;
        }

        private static GameRoom FindDefaultRelevanceRoom(List<GameRoom> relevanceRoomList)
        {
            foreach (GameRoom room in relevanceRoomList)
                if (!room.IsFull) return room;

            throw new Exception("Not found default relevance room from Default GameMode");
        }

        private static void RemoveRoom(GameRoom room)
        {
            lock (locker)
            {
                _rooms[room.RoomGameMode][room.MaxPlayers].Remove(room);

                foreach (var player in room.PlayersIdInRoom)
                    _playersInRooms.Remove(player);
            }
        }

        private static void AddLog(LogEventType logEventType, List<int> usersId, string description)
        {
            LogEvent logEvent = new()
            {
                Date = DateTime.Now,
                Type_Event = logEventType,
                UsersId = usersId,
                Description = description
            };

            DB.AddLogEvent(logEvent);
        }
    }
}