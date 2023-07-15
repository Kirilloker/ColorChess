using ColorChessModel;
using System;

public static class GameLobby
{
    //Rooms: GameMode -> MaxPlayersInRoom -> Rooms   
    private static Dictionary<GameMode, Dictionary<int, List<GameRoom>>> rooms = new();
    private static Dictionary<int, GameRoom> PlayersInRooms = new();
    private static object locker = new();

    //Retun Map if room is full and game is started. Else return null.
    public static Map FindRoomForPlayerAndStartGame(int PlayerId, GameMode gameMode, int NumOfPlayers)
    {
        lock (locker)
        {
            List<GameRoom> relevanceRoomList;
            Dictionary<int, List<GameRoom>> gameModeDict;

            //relevanceRoomList = rooms[gameMode][NumOfPlayers]
            if (!rooms.TryGetValue(gameMode, out gameModeDict))
            {
                rooms[gameMode] = new Dictionary<int, List<GameRoom>>();
                gameModeDict = rooms[gameMode];
            }
            if (!gameModeDict.TryGetValue(NumOfPlayers, out relevanceRoomList))
            {
                rooms[gameMode][NumOfPlayers] = new List<GameRoom>();
                relevanceRoomList = rooms[gameMode][NumOfPlayers];
            }

            //Create new room if we dont find relevance room
            if (relevanceRoomList.Count == 0)
            {
                Console.WriteLine("Player " + PlayerId + " create new room");
                GameRoom newRoom = new GameRoom(NumOfPlayers, new List<int>() { PlayerId }, gameMode);
                rooms[gameMode][NumOfPlayers].Add(newRoom);
                PlayersInRooms[PlayerId] = newRoom;
                return null;
            }

            GameRoom findedRoom;

            if (gameMode == GameMode.Rating)
            {
                int rate = DB.GetUserStatistic(PlayerId).Rate;
                findedRoom = FindRatingRelevanceRoom(relevanceRoomList, rate);
            }
            else if (gameMode == GameMode.Default) findedRoom = FindDefaultRelevanceRoom(relevanceRoomList);
            else throw (new Exception("GameLobby.FindRoomForPlayerAndStartGame() - unknown game mode"));

            //add player and start game if room is full
            findedRoom.AddPlayer(PlayerId);
            if (findedRoom.IsFull) return findedRoom.StartGame();
            else return null;
        }
    }

    //Apply player step and return next game state
    public static Map SendPlayerStepToRoomAndApplyIt(int PlayerId, string step)
    {
        GameRoom room = PlayersInRooms[PlayerId];
        Map GameState = room.ApplyPlayerStep(PlayerId, step);
        if (GameState.EndGame)
        {
            room.EndGame();
            lock (locker)
            {
                rooms[room.RoomGameMode][room.MaxPlayers].Remove(room);
                foreach (var player in room.PlayersInRoom)
                    PlayersInRooms.Remove(player);
            }
        }
        return GameState;
    }

    public static void PlayerLeftTheGame(int PlayerId)
    {
        GameRoom room;
        if (!PlayersInRooms.TryGetValue(PlayerId, out room))
            return;

        lock (locker)
        {
            rooms[room.RoomGameMode][room.MaxPlayers].Remove(room);
            foreach (var player in room.PlayersInRoom)
                PlayersInRooms.Remove(player);
        }
    }

    public static List<int> GetAllPlayersInRoomWithPlayer(int playerId)
    {
        GameRoom room;
        if (PlayersInRooms.TryGetValue(playerId, out room))
            return room.PlayersInRoom;
        else
            throw(new Exception("GetAllPlayersInRoomWithPlayer room not exist"));
    }

    private static GameRoom FindRatingRelevanceRoom(List<GameRoom> relevanceRoomList, int rate)
    {
        int bestRateDelta = int.MaxValue;
        int rateDelta;
        GameRoom moreRelevanceRoom = null;
        foreach (RatingGameRoom room in relevanceRoomList)
        {
            rateDelta = Math.Abs(rate - room.AverageRating);
            if (!room.IsFull && rateDelta < bestRateDelta)
            {
                moreRelevanceRoom = (GameRoom)room;
                bestRateDelta = rateDelta;
            }
        }
        if (moreRelevanceRoom == null) throw (new Exception("FindRatingRelevanceRoom() something wrong"));
        return moreRelevanceRoom;
    }

    private static GameRoom FindDefaultRelevanceRoom(List<GameRoom> relevanceRoomList)
    {
        foreach (GameRoom room in relevanceRoomList)
        {
            if (!room.IsFull) return room;
        }
        throw (new Exception("FindDefaultRelevanceRoom() something wrong"));
    }
}