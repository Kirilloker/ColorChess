using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static public class DefaultGameRoomManager
{
    static private DefaultGame? connectionsHub;
    static private Dictionary<string,Room> rooms = new Dictionary<string,Room>();
    static private string? waitingPlayerID = null;
    static private object locker = new();

    static public void SetConnectionsHub(DefaultGame _connectionsHub)
    {
        connectionsHub = _connectionsHub;
    }

    static public void FindRoom(string playerId)
    {
        lock (locker)
        {
            if (waitingPlayerID == null)
            {
                waitingPlayerID = playerId;
                
            }
            else
            {
                Room room = new Room(waitingPlayerID, playerId, connectionsHub);
                rooms.Add(playerId, room);
                rooms.Add(waitingPlayerID, room);
                rooms[playerId].StartGame();

                waitingPlayerID = null;
                
            }
        }
    }

    static public void SendPlayerStepToRoom(string playerID, string step)
    {
        rooms[playerID].ApplyPlayerStep(playerID, step); 
    }
}
