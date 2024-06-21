namespace GameServer.Tools
{
    public static class Tools
    {
        public static string Convert(ErrorType errorType) 
        {
            return errorType switch
            {
                ErrorType.NotFound => "Not Found",
                ErrorType.UnknownAttribute => "Unknown Attribute",
                ErrorType.NotFoundOpponent => "Not Found Opponent",
                ErrorType.NotFoundEnum => "Not Found Enum",
                ErrorType.UserExist => "This user exist",
                ErrorType.RoomExist => "This room exist",
                ErrorType.UserStatisticExist => "his user statistic exist",
                ErrorType.UserInLobbyExist => "This user in Lobby exist",
                _ => "Unknown type of error"
            };
             
        }
    }
}

